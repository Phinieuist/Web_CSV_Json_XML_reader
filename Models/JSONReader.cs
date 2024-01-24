using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Web_CSV_Json_XML_reader.Models
{
    public class JSONReader
    {
        public static string ReadForWeb(string text, out JToken token)
        {
            try
            {
                token = JToken.Parse(text);
                return ReadJsonForWeb(token);
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка в процессе чтения JSON-документа. " + ex.Message);
            }
        }

        // рекурсивная функция для обработки JSON-элементов и вывода на веб-страницу
        private static string ReadJsonForWeb(JToken token, string prefix = "")
        {
            if (token is JValue)
            {
                // если элемент является простым значением (не объектом или массивом),
                // выводим его значение в тег <input> для возможности редактирования
                JValue val = (JValue)token;
                string name = prefix;
                string type = "text";
                string tag = "";
                string output = string.Empty;

                switch (val.Type)
                {
                    case JTokenType.Integer:
                        output = $"<input type='number' name='{name}' value = '{val.Value}' />";
                        break;
                    case JTokenType.Float:
                        output = $"<input type='number' name='{name}' value = '{ Convert.ToString(val.Value, CultureInfo.InvariantCulture)}' />";
                        break;
                    case JTokenType.String:
                        output = $"<textarea name='{name}' class='form-control textInp'>{val.Value}</textarea>";
                        break;
                    case JTokenType.Boolean:
                        string varTrue = string.Empty;
                        string varFalse = string.Empty;

                        if ((bool)val.Value)
                            varTrue = "selected";
                        else
                            varFalse = "selected";

                        output = $"<select name='{name}' class='custom-select'> <option {varTrue} value='true'>true</option> <option {varFalse} value='false'>false</option> </select>";
                        break;
                    case JTokenType.Date:
                        output = $"<input type='datetime-local' name='{name}' value='{val.Value}'/>";
                        break;
                    default:
                        break;
                }

                return output;
            }
            else if (token is JArray)
            {
                // если элемент является массивом, создаем тег <details>,
                // в который будут добавляться все элементы массива
                JArray arr = (JArray)token;
                string name = prefix;
                StringBuilder sb = new StringBuilder();

                sb.Append($"<p><button class='btn btn-primary' type='button' data-bs-toggle='collapse' data-bs-target='#ID_{name}' aria-expanded='false' aria-controls='ID_{name}'>Array [{arr.Count}]</button></p>");
                sb.Append($"<div class='collapse' id='ID_{name}'>");
                sb.Append("<ul>");
                for (int i = 0; i < arr.Count; i++)
                {
                    sb.Append("<li>");
                    sb.Append(ReadJsonForWeb(arr[i], $"{name}[{i}]"));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                return sb.ToString();
            }
            else if (token is JObject)
            {
                // если элемент является объектом, создаем тег <details>,
                // в который будут добавляться все свойства объекта
                JObject obj = (JObject)token;
                string name = prefix;
                StringBuilder sb = new StringBuilder();
                sb.Append($"<p><button class='btn btn-primary' type='button' data-bs-toggle='collapse' data-bs-target='#ID_{name}' aria-expanded='false' aria-controls='ID_{name}'>Object [{obj.Count}]</button></p>");
                sb.Append($"<div class='collapse' id='ID_{name}'>");
                sb.Append("<ul>");
                foreach (JProperty prop in obj.Properties())
                {
                    sb.Append("<li>");
                    sb.Append($"<div class=\"input-group mb-3\"><div class=\"input-group-prepend\"><span class=\"input-group-text\">{prop.Name}</span></div>");
                    sb.Append(ReadJsonForWeb(prop.Value, $"{name}.{prop.Name}"));
                    sb.Append("</div></li>");
                }
                sb.Append("</ul>");
                sb.Append("</div>");
                return sb.ToString();
            }
            else
            {
                // в случае неизвестного типа элемента, выбрасываем исключение
                throw new Exception("Неизвестный JSON элемент.");
            }
        }
    }
}
