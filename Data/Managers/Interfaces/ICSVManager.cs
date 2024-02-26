using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface ICSVManager
    {
        public CSVDataTable GetCSVDataTable(HttpRequest request);

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator);
    }
}
