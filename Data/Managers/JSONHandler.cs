using Newtonsoft.Json.Linq;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class JSONHandler : IJSONManager
    {
        public JSONViewModel GetJsonVM(string jsonText, string fileName)
        {
            JToken json = JObject.Parse(jsonText);

            return new JSONViewModel(json, fileName);
        }

        public JToken GetJtoken(HttpRequest request)
        {
            throw new NotImplementedException();
        }


    }
}
