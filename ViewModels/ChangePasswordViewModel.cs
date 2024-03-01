namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string oldPassword {  get; set; }    
        public string newPassword { get; set; }
        public string newPasswordRepeat { get; set; }
        public string errorMessage { get; set; }

        public ChangePasswordViewModel() { }

        public ChangePasswordViewModel(string oldPassword, string newPassword, string newPasswordRepeat, string errorMessage)
        {
            this.oldPassword = oldPassword;
            this.newPassword = newPassword;
            this.newPasswordRepeat = newPasswordRepeat;
            this.errorMessage = errorMessage;
        }
    }
}
