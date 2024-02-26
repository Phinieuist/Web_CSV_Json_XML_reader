using System.Xml;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class ViewModelsCreator : IViewModelsCreator
    {
        private readonly ICSVManager _csvManager;
        private readonly IJSONManager _jsonManager;
        private readonly IXMLManager _xmlManager;

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator)
        {
            return _csvManager.GetCsvVM(csvText, fileName, separator);
        }

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator, bool isExistsInDB, Guid fileId)
        {
            CSVDataTable viewModel = _csvManager.GetCsvVM(csvText, fileName, separator);
            viewModel.IsExistsInDB = isExistsInDB;
            viewModel.FileId = fileId;

            return viewModel;
        }

        public JSONViewModel GetJsonVM(string jsonText, string fileName)
        {
            return _jsonManager.GetJsonVM(jsonText, fileName);
        }

        public JSONViewModel GetJsonVM(string jsonText, string fileName, bool isExistsInDB, Guid fileId)
        {
            JSONViewModel viewModel = _jsonManager.GetJsonVM(jsonText, fileName);
            viewModel.IsExistsInDB = isExistsInDB;
            viewModel.FileId = fileId;

            return viewModel;
        }

        public XMLViewModel GetXmlVM(string xmlText, string fileName)
        {
            return _xmlManager.GetXmlVM(xmlText, fileName);
        }

        public XMLViewModel GetXmlVM(string xmlText, string fileName, bool isExistsInDB, Guid fileId)
        {
            XMLViewModel viewModel = _xmlManager.GetXmlVM(xmlText, fileName);
            viewModel.IsExistsInDB = isExistsInDB;
            viewModel.FileId = fileId;

            return viewModel;
        }

        public ViewModelsCreator(ICSVManager csvManager, IJSONManager jsonManager, IXMLManager xmlManager)
        {
            _csvManager = csvManager;
            _jsonManager = jsonManager;
            _xmlManager = xmlManager;
        }
    }
}
