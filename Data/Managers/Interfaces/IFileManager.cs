using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web_CSV_Json_XML_reader.Models;

namespace Web_CSV_Json_XML_reader.Data.Managers.Interfaces
{
    public interface IFileManager
    {
        public Task<IReadOnlyCollection<DB.Entities.File>> GetAllFiles();

        public Task<IReadOnlyCollection<DB.Entities.File>> GetFiles(Guid userID);

        public Task<DB.Entities.File> GetFile(Guid fileId);

        public Task<bool> SaveFile(FileSaveHelper fileSaveHelper, string userEmail);

        public Task<bool> SaveFile(Stream fileMemoryStream, string originalName, string userEmail);

        public Task<bool> DeleteFile(Guid fileId);

        public Task<bool> DeleteUserFiles(Guid userId);

        public Task<bool> UpdateFile(FileSaveHelper fileSaveHelper, Guid fileId);

        public Task<string> GetFileContent(Guid fileId);
    }
}
