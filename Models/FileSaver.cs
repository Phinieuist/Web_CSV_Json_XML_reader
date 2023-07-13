using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace Web_CSV_Json_XML_reader.Models
{
    public class FileSaver
    {
        public static FileType GetFileType(string fileType)
        {
            FileType type = new FileType();
            switch(fileType)
            {
                case "CSV": type = FileType.CSV; break;
                case "JSON": type = FileType.JSON; break;
                case "XML": type = FileType.XML; break;
            }

            return type;
        }

        public static string GetFileName(FileType fileType, string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            switch (fileType)
            {
                case FileType.CSV: name += ".csv"; break;
                case FileType.JSON: name += ".json"; break;
                case FileType.XML: name += ".xml"; break;
            }

            return name;
        }

        public static Stream SaveCSV(HttpRequest Request)
        {
            CSVDataTable dataTable = new CSVDataTable();
            int i_lenght = Convert.ToInt32(Request.Form["i_lenght"]);
            int j_lenght = Convert.ToInt32(Request.Form["j_lenght"]);
            dataTable.Name = Request.Form["Name"].ToString();
            
            dataTable.Separator = Request.Form["Separator"].ToString();
            if (string.IsNullOrEmpty(dataTable.Separator)) dataTable.Separator = ",";

            for (int i = 0; i < i_lenght; i++)
            {
                dataTable.Data.Add(new string[j_lenght]);
                for (int j = 0; j < j_lenght; j++)
                {
                    dataTable.Data[i][j] = Request.Form[$"cell[{i},{j}]"].ToString();
                }
            }

            string Output = "";

            foreach (string[] line in dataTable.Data)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (i != line.Length - 1)
                        Output += '"' + line[i] + '"' + dataTable.Separator;
                    else
                        Output += '"' + line[i] + '"' + "\n";

                }
            }

            Output = Output.Remove(Output.LastIndexOf('\n'));

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.Write(Output);

            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream SaveJSON(JToken JSONDataToSave, HttpRequest Request)
        {
            foreach (var key in Request.Form.Keys)
            {
                JToken token = JSONDataToSave.SelectToken(key);
                if (token != null && token is JValue)
                {
                    JValue val = (JValue)token;
                    switch(val.Type)
                    {
                        case JTokenType.Integer: val.Value = Convert.ToInt32(Request.Form[key]); break;
                        case JTokenType.Float: val.Value = Convert.ToDouble(Request.Form[key]); break;
                        case JTokenType.Boolean: val.Value = Convert.ToBoolean(Request.Form[key]); break;
                        case JTokenType.Date: val.Value = Convert.ToDateTime(Request.Form[key]); break;
                        case JTokenType.String: val.Value = Convert.ToString(Request.Form[key]); break;
                        case JTokenType.Comment: val.Value = Convert.ToString(Request.Form[key]); break;
                    }
                        //val.Value = Request.Form[key];
                }
            }

            MemoryStream result = new MemoryStream();
            StreamWriter writer = new StreamWriter(result, Encoding.UTF8);

            writer.Write(JSONDataToSave.ToString());

            writer.Flush();
            result.Position = 0;
            return result;
        }
    }
}
