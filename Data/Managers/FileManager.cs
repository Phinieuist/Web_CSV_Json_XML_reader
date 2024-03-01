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
            DB.Entities.File? file = await _dbContext.Files.FindAsync(fileId);

            if (file is null)
            {
                throw new ArgumentNullException("Файл не найден в базе данных");
            }

            return file;
        }

        public async Task<string> GetFileContent(Guid fileId)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", fileId.ToString());
            string fileContent = string.Empty;

            if (System.IO.File.Exists(path))
            {
                fileContent = await System.IO.File.ReadAllTextAsync(path);
            }
            else
            {
                throw new ArgumentException("Файл отсутствует на сервере");
            }

            return fileContent;
        }

        protected async Task<bool> Delete(List<DB.Entities.File> files)
        {
            List<string> paths = new(files.Count);
            for (int i = 0; i < files.Count; i++)
                paths.Add(Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", files[i].FileId.ToString()));

            List<MemoryStream> filesContent = new();

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var path in paths)
                    {
                        if (!System.IO.File.Exists(path))
                        {
                            paths.Remove(path);
                            continue;
                        }

                        filesContent.Add(new MemoryStream());

                        using (FileStream fileStream = new FileStream(path, FileMode.Open))
                        {
                            await fileStream.CopyToAsync(filesContent.Last());
                            await filesContent.Last().FlushAsync();
                            filesContent.Last().Position = 0;
                        }

                        await Task.Run(() => System.IO.File.Delete(path));
                    }

                    _dbContext.Files.RemoveRange(files);
                    int res = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    foreach (var stream in filesContent)
                        stream.Dispose();

                    return res > 0;
                }
                catch
                {
                    if ((filesContent != null) && (filesContent.Count > 0))
                    {
                        for (int i = 0; i < filesContent.Count; i++)
                        {
                            using (FileStream restoreStream = new FileStream(paths[i], FileMode.Create))
                            {
                                filesContent[i].Seek(0, SeekOrigin.Begin);
                                await filesContent[i].CopyToAsync(restoreStream);
                                await restoreStream.FlushAsync();
                            }
                            filesContent[i].Dispose();
                        }

                    }

                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        protected async Task<bool> Delete(DB.Entities.File file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            if (!System.IO.File.Exists(path))
                throw new ArgumentException("Удаляемый файл отсутствует на сервере");

            MemoryStream fileContent = null;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    fileContent = new MemoryStream();

                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(fileContent);
                        await fileContent.FlushAsync();
                        fileContent.Position = 0;
                    }

                    await Task.Run(() => System.IO.File.Delete(path));

                    _dbContext.Files.Remove(file);
                    int res = await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    fileContent.Dispose();
                    return res > 0;
                }
                catch
                {
                    if (fileContent != null)
                    {
                        using (FileStream restoreStream = new FileStream(path, FileMode.Create))
                        {
                            fileContent.Seek(0, SeekOrigin.Begin);
                            await fileContent.CopyToAsync(restoreStream);
                            await restoreStream.FlushAsync();
                        }
                        fileContent.Dispose();
                    }

                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteFile(Guid fileId)
        {
            DB.Entities.File? file = await _dbContext.Files.FindAsync(fileId);

            if (file is null)
                throw new ArgumentNullException("Файл не найден в базе данных");

            return await Delete(file);
        }

        public async Task<bool> DeleteUserFiles(Guid userId)
        {
            List<DB.Entities.File> files = await _dbContext.Files.Where(file => file.UserId == userId).ToListAsync();

            if ((files is null) || (files.Count == 0))
                return true; // файлов у пользователя нет, нечего удалять

            return await Delete(files);
        }

        public async Task<bool> UpdateFile(FileSaveHelper fileSaveHelper, Guid fileId)
        {
            DB.Entities.File? file = await _dbContext.Files.FindAsync(fileId);

            if (file is null) throw new ArgumentNullException("Файл не найден в базе данных");

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());
            if (!System.IO.File.Exists(path))
            {
                throw new ArgumentException("Удаляемый файл отсутствует на сервере");
            }

            MemoryStream fileContent = null;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    fileContent = new MemoryStream();

                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(fileContent);
                        await fileContent.FlushAsync();
                        fileContent.Position = 0;
                    }

                    await Task.Run(() => System.IO.File.Delete(path));

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
                    await transaction.CommitAsync();

                    fileContent.Dispose();
                    return res > 0;
                }
                catch
                {
                    if (fileContent != null)
                    {
                        if (System.IO.File.Exists(path))
                            await Task.Run(() => System.IO.File.Delete(path));

                        using (FileStream restoreStream = new FileStream(path, FileMode.Create))
                        {
                            fileContent.Seek(0, SeekOrigin.Begin);
                            await fileContent.CopyToAsync(restoreStream);
                            await restoreStream.FlushAsync();
                        }
                        fileContent.Dispose();
                    }

                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> SaveFile(FileSaveHelper fileSaveHelper, string userEmail)
        {
            return await SaveFile(fileSaveHelper.DataToSave, fileSaveHelper.FileName, userEmail);
        }

        public async Task<bool> SaveFile(Stream fileMemoryStream, string originalName, string userEmail)
        {
            User? user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == userEmail);

            if (user is null)
                throw new ArgumentNullException("Пользователь не найден в базе данных");

            DB.Entities.File file = new(Guid.NewGuid(), user.UserId, originalName, DateTime.Now);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
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
                    await transaction.CommitAsync();

                    return res > 0;
                }
                catch
                {
                    if (System.IO.File.Exists(path))
                        await Task.Run(() => System.IO.File.Delete(path));

                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public FileManager(EFContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
