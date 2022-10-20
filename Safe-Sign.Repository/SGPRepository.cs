using Microsoft.AspNetCore.Http;

using Safe_Sign.Util;
using Safe_Sign.DTO.SGP;
using Safe_Sign.DAL.DAO;
using Safe_Sign.DAL.Models;
using Safe_Sign.Repository.Interfaces;

namespace Safe_Sign.Repository
{
    public class SGPRepository : ISGPRepository
    {
        private readonly DocumentDAO _documentDAO;
        private readonly PersonDAO _personDAO;
        private readonly SignatureDAO _signatureDAO;
        private readonly UserDAO _userDAO;

        public SGPRepository(DocumentDAO documentDAO, PersonDAO personDAO, SignatureDAO signatureDAO, UserDAO userDAO)
        {
            _documentDAO = documentDAO;
            _personDAO = personDAO;
            _signatureDAO = signatureDAO;
            _userDAO = userDAO;
        }

        public void DeleteTempDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) throw new DirectoryNotFoundException();

            Directory.Delete(directoryPath, false);

            if (Directory.Exists(directoryPath)) throw new Exception("Error on delete the directory");
        }

        public void DeleteTempFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("The requested file was not found");

            File.Delete(filePath);

            if (File.Exists(filePath)) throw new Exception("Error on delete the file");
        }

        public string InsertSignature(string fileDirectoryPath, string filePath, int signaturesNumber, Document document, IList<Signature> signatures, IList<Person> persons)
        {
            try
            {
                string signedFilePath = filePath.Insert(filePath.Length - 4, "_signed.pdf");
            
                SignTools.SignPDF(filePath, signedFilePath, signaturesNumber, document, signatures, persons);

                return signedFilePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        public SGPSignedDocumentDTO PrepareDocumentToExport(string signedFilePath, bool signed = false)
        {
            string[] splittedPath = signedFilePath.Split("\\");

            string fileName = splittedPath[splittedPath.Length - 1];

            string base64File = SGPTools.ConvertFileToBase64(signedFilePath);

            SGPSignedDocumentDTO document = new()
            {
                Base64File = base64File,
                FileName = fileName,
                Signed = signed
            };

            return document;
        }

        public (string, string) SaveFileAsTemp(IFormFile file)
        {
            Guid newGuid = Guid.NewGuid();
            string fileName = file.FileName;
            string currentPath = Directory.GetCurrentDirectory();
            string sgpFilesPath = Path.Combine(currentPath, "wwwroot\\SGPFiles\\");

            if (!Directory.Exists(sgpFilesPath)) Directory.CreateDirectory(sgpFilesPath);

            string newTempFileDirectory = Path.Combine(sgpFilesPath, newGuid.ToString());

            if (Directory.Exists(newTempFileDirectory)) throw new Exception("This directory is already created");

            Directory.CreateDirectory(newTempFileDirectory);

            string newTempFilePath = Path.Combine(newTempFileDirectory, fileName);

            using (FileStream fileStream = File.Create(newTempFilePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            return (newTempFileDirectory, newTempFilePath);
        }
       
        public SGPSignedDocumentDTO SignSGPDocument(IFormFile sgpFile, string userLogin)
        {
            string fileDirectoryPath, filePath;

            (fileDirectoryPath, filePath) = SaveFileAsTemp(sgpFile);

            User? userToSign = _userDAO.GetAll().FirstOrDefault(u => u.Username.ToLower().Trim().Equals(userLogin.ToLower().Trim()));

            if (userToSign is null) throw new NullReferenceException("The user was not found");

            Person? personToSign = _personDAO.GetByEmail(userToSign.Email);

            if (personToSign is null) throw new NullReferenceException("The informations about this person was not found");

            List<Person> persons = new()
            {
                personToSign
            };

            Signature signature = new()
            {
                KeyHash = Guid.NewGuid(),
                SignatureDate = DateTime.Now,
                SignatureType = DAL.Enumerators.SignatureTypeEnum.CredenciaisInternas,
                IdUser = userToSign.Id
            };

            Signature _signature = _signatureDAO.Create(signature);

            List<Signature> signatures = new()
            {
                _signature
            };

            Document document = new()
            {
                Description = "Documento provindo do SGP",
                KeyHash = Guid.NewGuid(),
                Status = DAL.Enumerators.DocumentStatusEnum.Pendente,
                CreationDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IdUser = userToSign.Id,
                Signatures = new List<Signature>()
            };

            Document? _documentAlreadyRegistred = _documentDAO.GetAll().FirstOrDefault(d => d.IdUser.Equals(userToSign.Id));

            if (_documentAlreadyRegistred is not null) throw new Exception("This document is already registred in SafeSign");

            Document _document = _documentDAO.Create(document);

            _document.Signatures.Add(_signature);

            _documentDAO.Update(_document);

            string signedFilePath = InsertSignature(fileDirectoryPath, filePath, 1, _document, signatures, persons);

            if (string.IsNullOrEmpty(signedFilePath)) throw new Exception("Error on sign document");

            Document _signedDocument = _documentDAO.GetById(_document.Id);

            _signedDocument.Status = DAL.Enumerators.DocumentStatusEnum.Finalizado;
            _signedDocument.LastModifiedDate = DateTime.Now;

            _documentDAO.Update(_signedDocument);

            SGPSignedDocumentDTO signedDocument = PrepareDocumentToExport(signedFilePath, true);

            DeleteTempFile(filePath);
            DeleteTempFile(signedFilePath);
            DeleteTempDirectory(fileDirectoryPath);

            return signedDocument;
        }
    }
}
