namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class RegisterViewModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string passwordRepeat { get; set; }
        public string errorMessage { get; set; }

        public RegisterViewModel() { }

        public RegisterViewModel(string email, string password, string passwordRepeat, string errorMessage)
        {
            this.email = email;
            this.password = password;
            this.passwordRepeat = passwordRepeat;
            this.errorMessage = errorMessage;
        }
    }
}
