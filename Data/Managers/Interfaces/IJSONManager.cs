using Newtonsoft.Json.Linq;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IJSONManager
    {
        public JToken GetJtoken(HttpRequest request);

        public JSONViewModel GetJsonVM(string jsonText, string fileName);
    }
}
