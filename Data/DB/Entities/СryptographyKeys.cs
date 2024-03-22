using System.ComponentModel.DataAnnotations;

namespace Web_CSV_Json_XML_reader.Data.DB.Entities
{
    public class СryptographyKey
    {
        [Key]
        public Guid UserId { get; set; }

        public string? PrivateKey { get; set; }
        
        public string? PublicKey { get; set; }

        public DateTime? Created { get; set; }
    }
}
