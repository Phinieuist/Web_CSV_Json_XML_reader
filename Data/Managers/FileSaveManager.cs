using Newtonsoft.Json.Linq;
using System.Text;
using System.Xml;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.Models;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class FileSaveManager : IFileSaveManager
    {
        private readonly ICSVManager _csvManager;
        private readonly IJSONManager _jsonManager;
        private readonly IXMLManager _xmlManager;

        public FileSaveHelper Download(FileType fileType, HttpRequest request)
        {
            switch (fileType)
            {
                case FileType.CSV: return DownloadCSV(request);
                case FileType.JSON: return SaveDownloadJSON(request);
                case FileType.XML: return SaveDownloadXML(request);
                default: throw new ArgumentException("Неизвестный FileType");
            }
        }

        public FileSaveHelper Save(FileType fileType, HttpRequest request)
        {
            switch (fileType)
            {
                case FileType.CSV: return SaveCSV(request);
                case FileType.JSON: return SaveDownloadJSON(request);
                case FileType.XML: return SaveDownloadXML(request);
                default: throw new ArgumentException("Неизвестный FileType");
            }
        }

        private FileSaveHelper SaveCSV(HttpRequest request)
        {
            CSVDataTable dataTable = _csvManager.GetCSVDataTable(request);
            string name = Path.GetFileNameWithoutExtension(request.Form["Name"].ToString());

            // при сохранении имя файла будет содержать разделитель: [имя_файла]%sep%[разделитель].csv
            return new FileSaveHelper(GetCSVStream(dataTable), string.Concat(name, "%sep%", dataTable.Separator, ".csv"));
        }

        private FileSaveHelper DownloadCSV(HttpRequest request)
        {
            CSVDataTable dataTable = _csvManager.GetCSVDataTable(request);
            string name = Path.GetFileNameWithoutExtension(request.Form["Name"].ToString());

            return new FileSaveHelper(GetCSVStream(dataTable), string.Concat(name, ".csv"));
        }

        private FileSaveHelper SaveDownloadJSON(HttpRequest request)
        {
            JToken jToken = _jsonManager.GetJtoken(request);
            string name = Path.GetFileNameWithoutExtension(request.Form["Name"].ToString());

            return new FileSaveHelper(GetMemoryStream(jToken.ToString()), string.Concat(name, ".json"));
        }

        private FileSaveHelper SaveDownloadXML(HttpRequest request)
        {
            XmlDocument xmlDocument = _xmlManager.GetXmlDocument(request);
            string name = Path.GetFileNameWithoutExtension(request.Form["Name"].ToString());

            return new FileSaveHelper(GetMemoryStream(xmlDocument.OuterXml), string.Concat(name, ".xml"));
        }

        private Stream GetCSVStream(CSVDataTable dataTable)
        {
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

            return GetMemoryStream(Output);
        }

        private MemoryStream GetMemoryStream(string streamContent)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.Write(streamContent);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }

        public FileSaveManager(ICSVManager csvManager, IJSONManager jsonManager, IXMLManager xmlManager)
        {
            _csvManager = csvManager;
            _jsonManager = jsonManager;
            _xmlManager = xmlManager;
        }
    }
}
