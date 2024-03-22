namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class СryptographyViewModel
    {
        public string? PrivateKey { get; set; }  
        public string? PublicKey { get; set; }
        public string? ServerPublicKey {  get; set; }   
        public string? ErrorMessage { get; set; }
        public string? MessageText { get; set; }
        public string? SignatureText { get; set; }
        public string? CreatedPublicKey { get; set; }
        public string? CreatedPrivateKey {  get; set; }
        public string? AlertMessage {  get; set; }

        public void ResetServiceMessages()
        {
            AlertMessage = null;
            ErrorMessage = null;
        }
    }
}
