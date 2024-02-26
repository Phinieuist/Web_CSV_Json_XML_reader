using Web_CSV_Json_XML_reader.Models;

namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class TextInputViewModel
    {
        public string Text { get; set; }
        public string CSVSeparator { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }

        public TextInputViewModel() { }
    }
}
