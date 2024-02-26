using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Web_CSV_Json_XML_reader.Data.DB;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.Models;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class FileManager : IFileManager
    {
        private readonly EFContext _dbContext;
        private readonly IViewModelsCreator _viewModelsCreator;

        public async Task<IReadOnlyCollection<DB.Entities.File>> GetAllFiles()
        {
            return await _dbContext.Files.Select(q => q).AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyCollection<DB.Entities.File>> GetFiles(Guid userID)
        {
            return await _dbContext.Files.Where(q => q.UserId == userID).AsNoTracking().ToListAsync();
        }

        public async Task<DB.Entities.File> GetFile(Guid fileId)
        {
            return await _dbContext.Files.AsNoTracking().FirstOrDefaultAsync(file => file.FileId == fileId);
        }

        public async Task<bool> DeleteFile(Guid fileId)
        {
            DB.Entities.File? file = await _dbContext.Files.FirstOrDefaultAsync(file => file.FileId == fileId);

            if (file is null)
                return false; //вместо фолсов добавить ошибки

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else
            {
                return false;
            }

            _dbContext.Files.Remove(file);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> DeleteUserFiles(Guid userId)
        {
            List<DB.Entities.File> files = await _dbContext.Files.Where(file => file.UserId == userId).ToListAsync();

            if ((files is null) || (files.Count == 0))
                return false;

            foreach (DB.Entities.File file in files)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                    continue;
                
                _dbContext.Files.Remove(file);
            }

            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<ViewResult> OpenFile(Guid fileId)
        {
            DB.Entities.File file = await _dbContext.Files.AsNoTracking().FirstOrDefaultAsync(file => file.FileId == fileId);

            if (file is null)
            {
                return ViewResultCreator.Create("~/Views/Account/SavedFiles.cshtml");
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            string fileContent = string.Empty;

            if (System.IO.File.Exists(path))
            {
                fileContent = System.IO.File.ReadAllText(path);
            }
            else
            {
                return ViewResultCreator.Create("~/Views/Account/SavedFiles.cshtml");

            }

            string extension = Path.GetExtension(file.FileName).Trim().ToLower();
            string rawHTML = string.Empty;
            ViewResult viewResult = null;

            switch (extension)
            {
                case ".json":
                    viewResult = ViewResultCreator.Create("~/Views/Home/JSON.cshtml", _viewModelsCreator.GetJsonVM(fileContent, file.FileName, true, file.FileId));
                    break;

                case ".xml":
                    viewResult = ViewResultCreator.Create("~/Views/Home/XML.cshtml", _viewModelsCreator.GetXmlVM(fileContent, file.FileName, true, file.FileId));
                    break;

                case ".csv":
                    string[] parts = Path.GetFileNameWithoutExtension(file.FileName).Split("%sep%");
                    string separator = parts.Last();
                    string outputName = string.Empty;

                    if (parts.Length == 2)
                    {
                        outputName = string.Concat(parts[0], ".csv");
                    }
                    else
                    {
                        outputName = string.Concat(string.Join("%sep%", parts.SkipLast(1).ToArray()), ".csv");
                    }

                    CSVDataTable csvVM = _viewModelsCreator.GetCsvVM(fileContent, outputName, separator, true, file.FileId);

                    viewResult = ViewResultCreator.Create("~/Views/Home/CSV.cshtml", csvVM);

                    break;
            }

            if (viewResult is not null)
                return viewResult;
            else
                return ViewResultCreator.Error($"Ошибка при открытии файла в OpenFile. UserID: {file.UserId} | FileId: {file.FileId} | FileName: {file.FileName} | PathToFile: {path} | ExtensionOfFile: {extension}");
        }

        public async Task<bool> UpdateFile(FileSaveHelper fileSaveHelper, Guid fileId)
        {
            DB.Entities.File file = await _dbContext.Files.FirstOrDefaultAsync(file => file.FileId == fileId);

            if (file is null) return false;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else
            {
                return false;
            }

            using (FileStream fileStream = System.IO.File.Create(path))
            {
                fileSaveHelper.DataToSave.CopyTo(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
            }

            fileSaveHelper.DataToSave.Dispose();

            file.FileName = fileSaveHelper.FileName;
            file.LastChanged = DateTime.Now;

            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> SaveFile(FileSaveHelper fileSaveHelper, string userEmail)
        {
            return await SaveFile(fileSaveHelper.DataToSave, fileSaveHelper.FileName, userEmail);
        }

        public async Task<bool> SaveFile(Stream fileMemoryStream, string originalName, string userEmail)
        {
            User? user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == userEmail);

            if (user is null)
                return false;

            DB.Entities.File file = new(Guid.NewGuid(), user.UserId, originalName, DateTime.Now);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            using (FileStream fileStream = System.IO.File.Create(path))
            {
                fileMemoryStream.CopyTo(fileStream);
                await fileStream.FlushAsync();
                fileStream.Close();
            }

            fileMemoryStream.Dispose();

            _dbContext.Files.Add(file);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public FileManager(EFContext dbContext, IViewModelsCreator viewModelsCreator)
        {
            _dbContext = dbContext;
            _viewModelsCreator = viewModelsCreator;
        }
    }
}
