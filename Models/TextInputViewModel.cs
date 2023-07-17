namespace Web_CSV_Json_XML_reader.Models
{
    public class TextInputViewModel
    {
        public string Text { get; set; }
        public string CSVSeparator { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }

        public TextInputViewModel() { }
    }

    public enum FileType
    {
        CSV,
        JSON,
        XML
    }
}
