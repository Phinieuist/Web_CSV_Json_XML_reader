using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IViewModelsCreator
    {
        public XMLViewModel GetXmlVM(string xmlText, string fileName);

        public XMLViewModel GetXmlVM(string xmlText, string fileName, bool isExistsInDB, Guid fileId);

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator);

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator, bool isExistsInDB, Guid fileId);

        public JSONViewModel GetJsonVM(string jsonText, string fileName);

        public JSONViewModel GetJsonVM(string jsonText, string fileName, bool isExistsInDB, Guid fileId);
    }
}
