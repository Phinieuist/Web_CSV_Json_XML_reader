using Newtonsoft.Json.Linq;

namespace Web_CSV_Json_XML_reader.Models
{
    public class JSONViewModel
    {
        public JObject Data { get; set; }
        
        public JSONViewModel() { }

        public JSONViewModel(JObject data)
        {
            Data = data;
        }
    }

    public class JSONSaveObj
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public JSONValueType Type { get; set; }

        public enum JSONValueType
        {
            None,
            Array,
            Object
        }

        public JSONSaveObj() { }

        public JSONSaveObj(string name, object value, JSONValueType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }
}
