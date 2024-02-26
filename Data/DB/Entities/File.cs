using System.ComponentModel.DataAnnotations;

namespace Web_CSV_Json_XML_reader.Data.DB.Entities
{
    public class File
    {

        [Key]
        public Guid FileId { get; set; }

        public Guid UserId { get; set; }

        public string FileName { get; set; }

        public DateTime LastChanged { get; set; }

        public File() { }

        public File(Guid fileId, Guid userId, string fileName, DateTime lastChanged)
        {
            FileId = fileId;
            UserId = userId;
            FileName = fileName;
            LastChanged = lastChanged;
        }
    }
}
