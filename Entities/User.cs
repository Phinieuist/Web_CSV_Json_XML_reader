using System.ComponentModel.DataAnnotations;

namespace Web_CSV_Json_XML_reader.Entities
{
    public class User
    {

        [Key]
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public User() { }

        public User(Guid UserId, string Email, string Password)
        {
            this.UserId = UserId;
            this.Email = Email;
            this.Password = Password;
        }
    }
}
