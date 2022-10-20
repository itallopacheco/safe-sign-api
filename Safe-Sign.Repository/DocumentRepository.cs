using Safe_Sign.Util;
using Safe_Sign.DAL.DAO;
using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Marker;
using Safe_Sign.DAL.Models;
using Safe_Sign.DTO.Document;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.DTO.Signature;
using Safe_Sign.DTO.DocumentFile;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IDAO<DocumentFile> _archiveDAO;
        private readonly DocumentDAO _documentDAO;
        private readonly IDAO<Marker> _markerDAO;
        private readonly PersonDAO _personDAO;
        private readonly SignatureDAO _signatureDAO;
        private readonly UserDAO _userDAO;

        public DocumentRepository(DocumentDAO documentDAO, 
                        PersonDAO personDAO, 
                    UserDAO userDAO, 
                SignatureDAO signatureDAO, 
            IDAO<DocumentFile> archiveDAO, 
        IDAO<Marker> markerDAO)
        {
            _archiveDAO = archiveDAO;
            _documentDAO = documentDAO;
            _markerDAO = markerDAO;
            _personDAO = personDAO;
            _signatureDAO = signatureDAO;
            _userDAO = userDAO;
        }

        public DocumentDTO UpdateDocument(UpdateDocumentDTO updateDocumentDTO)
        {
            Document oldDocument = _documentDAO.GetById(updateDocumentDTO.Id);

            if (oldDocument is null) throw new NullReferenceException();

            DocumentFile archive = _archiveDAO.GetById((ulong)updateDocumentDTO.File.Id);

            oldDocument.File = !(oldDocument.Equals(archive)) ? archive : oldDocument.File;

            _documentDAO.Update(oldDocument);

            DocumentFileDTO archiveDTO = new()
            {
                Id = oldDocument.File.Id,
                FileName = oldDocument.File.FileName,
                FilePath = oldDocument.File.FilePath,
                CreatedDate = oldDocument.File.CreatedDate,
                IdDocument = oldDocument.File.IdDocument,
            };

            DocumentDTO finalDTO = new()
            {
                Id = oldDocument.Id,
                KeyHash = oldDocument.KeyHash,
                Description = oldDocument.Description,
                Status = oldDocument.Status,
                StatusDescription = oldDocument.Status.ToString(),
                IdUser = oldDocument.IdUser,
                File = archiveDTO,
                CreationDate = oldDocument.CreationDate,
            };

            return finalDTO;
        }

        public DocumentDTO CreateDocument(DocumentDTO documentDTO, string virtualPath)
        {
            ICollection<Marker> markers = new List<Marker>();

            if (documentDTO.Markers is not null) foreach (MarkerDTO marker in documentDTO.Markers) markers.Add(_markerDAO.GetById(marker.Id));

            ICollection<Signature> Signatues = new List<Signature>();

            Document documentEntity = new()
            {
                Description = documentDTO.Description,
                KeyHash = Guid.NewGuid(),
                Status = documentDTO.Status,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IdUser = documentDTO.IdUser,
                Markers = markers,
                Signatures = Signatues,
                
            };

            Document _document = _documentDAO.Create(documentEntity);

            if (documentDTO.File is null || documentDTO.File.FormFile.Length <= 0) throw new Exception("");

            if (!Directory.Exists(virtualPath + @"\\Files\\")) Directory.CreateDirectory(virtualPath + @"\\Files\\");

            Guid guid = Guid.NewGuid();
            string fileFolderPath = virtualPath + @"\\Files\\" + $@"\\{guid}\\";
            Directory.CreateDirectory(fileFolderPath);

            string filePath = fileFolderPath + documentDTO.File.FormFile.FileName;

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                documentDTO.File.FormFile.CopyTo(fileStream);
            }

            DocumentFile archiveEntity = new()
            {
                FileName = documentDTO.File.FormFile.FileName,
                FilePath = filePath,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IdDocument = _document.Id
            };

            DocumentFile _archive = _archiveDAO.Create(archiveEntity);
            DocumentFileDTO fileDTO = new()
            {
                Id = _archive.Id,
                FileName = _archive.FileName,
                FilePath = _archive.FilePath,
                CreatedDate = _archive.CreatedDate,
                IdDocument = _archive.IdDocument
            };

            UpdateDocumentDTO updateDocumentDTO = new()
            {
                Id = _document.Id,
                File = fileDTO,
            };

            UpdateDocument(updateDocumentDTO);


            DocumentDTO finalDTO = new()
            {
                Id = _document.Id,
                Description = _document.Description,
                Status = _document.Status,
                StatusDescription = _document.Status.ToString(),
                CreationDate = _document.CreationDate,
                LastModifiedDate=_document.LastModifiedDate,
                File = fileDTO,
                IdUser = _document.IdUser,
            };

            return finalDTO;
        }


        public void SignDocument(ulong idDocument) 
        {
            // Get the document in the database
            Document document = _documentDAO.GetDocumentWithDependeciesById(idDocument);
           
            // Start a signatureDTO list 
            IList<SignatureDTO> signaturesDTO = new List<SignatureDTO>();

            if (document.Signatures is null) throw new NullReferenceException("This document don't have signatures");

            // For each signature in the Icollection document.Signatures
            // create a signatureDTO and add to the SignatureDTO list.
            foreach (Signature s in document.Signatures)
            {
                SignatureDTO signatureDTO = new()
                {
                    Id = s.Id,
                    KeyHash = s.KeyHash,
                    SignatureDate = s.SignatureDate,
                    IdUser = s.IdUser,
                    IdDocument = idDocument,
                    SignatureType = s.SignatureType,
                    SignatureTypeDescription = s.SignatureType.ToString()
                };

                signaturesDTO.Add(signatureDTO);
            }

            // Start a DocumentDTO for passing to the sign function.
            DocumentDTO documentDTO = new()
            {
                Id = document.Id,
                KeyHash = document.KeyHash,
                Description = document.Description,
                Status = document.Status,
                StatusDescription = document.Status.ToString(),
                CreationDate = document.CreationDate,
                LastModifiedDate = document.LastModifiedDate,
                Signatures = signaturesDTO
            };

            // Count the number of Signatures
            int signaturesNumber = documentDTO.Signatures.Count;

            // Get the list of PersonsDTO for passing to the sign function.
            IList<PersonDTO> personsDTO = new List<PersonDTO>();
            foreach (SignatureDTO s in documentDTO.Signatures)
            {
                Person person = _personDAO.GetByIdUser(s.IdUser);

                PersonDTO personDTO = new()
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    CPF = person.CPF,   
                    BirthDate = person.BirthDate,
                    MotherName = person.MotherName
                };

                personsDTO.Add(personDTO);
            }

            if (document.File is null) throw new NullReferenceException("This document don't have a file");

            string originDocumentPath = document.File.FilePath;
            int indexOfInsertion = originDocumentPath.Length -4;
            string targetDocumentPath = originDocumentPath.Insert(indexOfInsertion, "_signed");
            
            SignTools.SignPDF(originDocumentPath, targetDocumentPath, signaturesNumber,documentDTO, signaturesDTO, personsDTO);
        }

        public IList<DocumentDTO> GetAll()
        {
            IEnumerable<Document> documents = _documentDAO.GetAll();

            if (documents is null) throw new NullReferenceException();

            List<DocumentDTO> documentsDTO = new();

            foreach (Document document in documents)
            {
                List<SignatureDTO> signatureDTOs = new();

                if (document.Signatures is not null)
                {
                    foreach (Signature signature in document.Signatures)
                    {
                        SignatureDTO signatureDTO = new()
                        {
                            Id = signature.Id,
                            KeyHash = signature.KeyHash,
                            SignatureDate = signature.SignatureDate,
                            IdUser = signature.IdUser,
                            IdDocument = document.Id,
                            SignatureType = signature.SignatureType,
                            SignatureTypeDescription = signature.SignatureType.ToString(),
                        };
                    }
                }

                List<MarkerDTO> markerDTOs = new();
                if (document.Markers is not null)
                {
                    foreach (Marker marker in document.Markers)
                    {
                        MarkerDTO markerDTO = new()
                        {
                            Id = marker.Id,
                            IsActive = marker.IsActive,
                            IdTheme = marker.IdTheme,
                        };
                    }
                }

                DocumentFileDTO archiveDTO = new()
                {
                    Id = document.File.Id,
                    FileName = document.File.FileName,
                    FilePath = document.File.FilePath,
                    CreatedDate = document.File.CreatedDate,
                    IdDocument = document.File.Id,
                };

                DocumentDTO documentDTO = new()
                {
                    Id = document.Id,
                    KeyHash = document.KeyHash,
                    Description = document.Description,
                    Status = document.Status,
                    StatusDescription = document.Status.ToString(),
                    IdUser = document.IdUser,
                    File = archiveDTO,
                    CreationDate = document.CreationDate,
                    LastModifiedDate = document.LastModifiedDate,
                    Signatures = signatureDTOs,
                    Markers = markerDTOs,

                };

                documentsDTO.Add(documentDTO);
            }

            return documentsDTO;
        }

        public bool GetDocumentByToken(string token)
        {
            Document document = _documentDAO.GetDocumentWithDependeciesByHash(token);

            bool documentIsNull;

            if(document is not null)
            {
                documentIsNull = true;
            }
            else
            {
                documentIsNull = false;
            }


            return documentIsNull;
        }
    }
}
