namespace Safe_Sign.DTO.SGP
{
    public class SGPSignedDocumentDTO
    {
        public string Base64File { get; set; }

        public string FileName { get; set; }

        public bool Signed { get; set; }
    }
}
