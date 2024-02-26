namespace Web_CSV_Json_XML_reader.Models
{
    public class FileSaveHelper
    {
        public Stream DataToSave { get; set; }  

        public string FileName { get; set; }

        public FileSaveHelper() { }

        public FileSaveHelper(Stream dataToSave, string fileName)
        {
            DataToSave = dataToSave;
            FileName = fileName;
        }
    }
}
