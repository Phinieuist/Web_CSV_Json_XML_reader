using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
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

        public static Stream Save(FileType original, FileType saveTo, CSVDataTable? CSVDataToSave, JObject? JSONDataToSave, string? XMLDataToSave)
        {
            Stream result = null;

            switch (original)
            {
                case FileType.CSV:
                    {
                        switch (saveTo)
                        {
                            case FileType.CSV:
                                {
                                    result = CSVReader.Save(CSVDataToSave);
                                    break;
                                }
                            case FileType.JSON:
                                {
                                    string json = JsonConvert.SerializeObject(CSVDataToSave.ToDataTable());
                                    result = new MemoryStream();
                                    StreamWriter writer = new StreamWriter(result, Encoding.UTF8);

                                    writer.Write(json);

                                    writer.Flush();
                                    result.Position = 0;

                                    break;
                                }
                            case FileType.XML:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                case FileType.JSON: break;
                case FileType.XML: break;
            }



            return result;
        }

        public static Stream SaveCSV(CSVDataTable CSVDataToSave)
        {
            Stream result = null;
            result = CSVReader.Save(CSVDataToSave);
            return result;
        }

        public static Stream SaveJSON(JObject JSONDataToSave)
        {
            Stream result = null;

            
            
            return result;
        }

        public enum FileType
        {
            CSV,
            JSON,
            XML
        }
    }
}
