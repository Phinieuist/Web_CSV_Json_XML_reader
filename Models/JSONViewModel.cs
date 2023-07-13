using Newtonsoft.Json.Linq;

namespace Web_CSV_Json_XML_reader.Models
{
    public class JSONViewModel
    {
        public JToken Data { get; set; }
        public string RawHTML { get; set; }
        public string Name { get; set; }

        public JSONViewModel() { }

        public JSONViewModel(JToken data)
        {
            Data = data;
        }

        public JSONViewModel(JToken data, string RawHTML)
        {
            Data = data;
            this.RawHTML = RawHTML;
        }
    }
}
