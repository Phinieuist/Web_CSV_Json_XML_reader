using System.ComponentModel.DataAnnotations;

namespace Web_CSV_Json_XML_reader.Entities
{
    public class File
    {

        [Key]
        public Guid FileId { get; set; }

        public Guid UserId { get; set; }    

        public string FileName { get; set; }

        public DateTime LastChanged { get; set; }
    }
}
