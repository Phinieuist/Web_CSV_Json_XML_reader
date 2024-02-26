using Web_CSV_Json_XML_reader.Models;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IFileSaveManager
    {
        //public FileSaveHelper SaveCSV();

        //public FileSaveHelper DownloadCSV();

        //public FileSaveHelper SaveCSV();

        //public FileSaveHelper SaveCSV();

        public FileSaveHelper Save(FileType fileType, HttpRequest request);

        public FileSaveHelper Download(FileType fileType, HttpRequest request);
    }
}
