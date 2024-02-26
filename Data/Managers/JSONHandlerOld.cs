using Newtonsoft.Json.Linq;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class JSONHandlerOld : IJSONManager
    {
        public JSONViewModel GetJsonVM(string jsonText, string fileName)
        {
            JToken json = JObject.Parse(jsonText);

            return new JSONViewModel(json, fileName);
        }

        public JToken GetJtoken(HttpRequest Request)
        {
            JToken JSONDataToSave = JToken.Parse(Request.Form["Data"]);

            foreach (var key in Request.Form.Keys)
            {
                JToken token = JSONDataToSave.SelectToken(key);
                if (token != null && token is JValue)
                {
                    JValue val = (JValue)token;
                    switch (val.Type)
                    {
                        case JTokenType.Integer: val.Value = Convert.ToInt32(Request.Form[key]); break;
                        case JTokenType.Float: val.Value = Convert.ToDouble(Request.Form[key].ToString().Replace('.', ',')); break;
                        case JTokenType.Boolean: val.Value = Convert.ToBoolean(Request.Form[key]); break;
                        case JTokenType.Date: val.Value = Convert.ToDateTime(Request.Form[key]); break;
                        case JTokenType.String: val.Value = Convert.ToString(Request.Form[key]); break;
                        case JTokenType.Comment: val.Value = Convert.ToString(Request.Form[key]); break;
                    }
                }
            }

            return JSONDataToSave;
        }
    }
}
