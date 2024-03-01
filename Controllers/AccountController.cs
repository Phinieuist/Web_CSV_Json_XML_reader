using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Newtonsoft.Json.Linq;
using Web_CSV_Json_XML_reader.Models;
using System.Xml;
using Microsoft.AspNetCore.Identity;
using Web_CSV_Json_XML_reader.Data.DB.Entities;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;
using Microsoft.EntityFrameworkCore;
using Web_CSV_Json_XML_reader.Data.Managers;

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IFileManager _fileManager;
        private readonly IFileSaveManager _fileSaveManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IViewModelsCreator _viewModelsCreator;

        public ActionResult Index()
        {
            return View("Login");
        }

        public async Task<string> DB()
        {
            return await _userManager.DB();
        }

        [Authorize]
        public async Task<IActionResult> SavedFiles()
        {
            try
            {
                string userID = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).First().Value;
                return View(await _fileManager.GetFiles(Guid.Parse(userID)));
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [Authorize]
        public async Task<IActionResult> DeleteFile(Guid FileId)
        {
            try
            {
                if (await _fileManager.DeleteFile(FileId))
                    return View("SavedFiles");
                else
                    return ViewResultCreator.Error("Ошибка при удалении файла");
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [Authorize]
        public async Task<IActionResult> OpenFile(Guid FileId)
        {
            try
            {
                Data.DB.Entities.File file = await _fileManager.GetFile(FileId);
                string fileContent = await _fileManager.GetFileContent(FileId);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "SavedFiles", file.FileId.ToString());

                string extension = Path.GetExtension(file.FileName).Trim().ToLower();
                string rawHTML = string.Empty;

                switch (extension)
                {
                    case ".json":
                        return View("~/Views/Home/JSON.cshtml", _viewModelsCreator.GetJsonVM(fileContent, file.FileName, true, file.FileId));

                    case ".xml":
                        return View("~/Views/Home/XML.cshtml", _viewModelsCreator.GetXmlVM(fileContent, file.FileName, true, file.FileId));

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

                        return View("~/Views/Home/CSV.cshtml", csvVM);
                    default: return View("SavedFiles");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        public async Task<string> AllUsers()
        {
            var entities = await _userManager.GetUsers();

            StringBuilder sb = new StringBuilder();

            foreach (var item in entities)
            {
                sb.Append(string.Join(" | ", new string[] { item.UserId.ToString(), item.Email, item.Password }));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public async Task<string> AllFiles()
        {
            var entities = await _fileManager.GetAllFiles();

            StringBuilder sb = new StringBuilder();

            foreach (var item in entities)
            {
                sb.Append(string.Join(" | ", new string[] { item.FileId.ToString(), item.UserId.ToString(), item.FileName, item.LastChanged.ToString() }));
                sb.AppendLine();
            }

            return sb.ToString();
        }
        
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> SetNewPassword(ChangePasswordViewModel model)
        {
            if (model.newPassword != model.newPasswordRepeat)
            {
                model.errorMessage = "Повторите новый пароль, пароли должны совпадать";
                return View("ChangePassword", model);
            }

            Data.DB.Entities.User user = await _userManager.GetUser(Guid.Parse(User.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).First().Value.ToString()));

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.oldPassword);

            if (verificationResult is PasswordVerificationResult.Failed)
            {
                model.errorMessage = "Введён неверный текущий пароль";
                return View("ChangePassword", model);
            }

            Data.DB.Entities.User newUser = new User(user);
            newUser.Password = _passwordHasher.HashPassword(newUser, model.newPassword);

            try
            {
                bool res = await _userManager.UpdateUser(user, newUser);

                if (res)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    return ViewResultCreator.Error("Ошибка при сохранении или обновлении пользователя. В базу данных записано 0 элементов");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            try
            {
                await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                Guid userId = Guid.Parse(User.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).First().Value.ToString());
                await _fileManager.DeleteUserFiles(userId);

                bool res = await _userManager.DeleteUser(userId);

                if (res)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return ViewResultCreator.Error("Ошибка при удалении пользователя. Удалено 0 элементов");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> RegisterNew(RegisterViewModel model)
        {
            if (model.password != model.passwordRepeat)
            {
                model.errorMessage = "Повторите пароль, пароли должны совпадать";
                return View("Register", model);
            }

            string encryptedPassword = _passwordHasher.HashPassword(null, model.password);

            try
            {
                if (!await _userManager.AddUser(model.email, encryptedPassword))
                    return ViewResultCreator.Error("Произошла ошибка при добавлении пользователя");
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            var form = Request.Form;

            string email = form["email"];
            string password = form["password"];

            User? user = await _userManager.GetUser(email);

            if (user is null) return View(true);

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (verificationResult is PasswordVerificationResult.Failed) return View(true);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // установка аутентификационных куки
            
            await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public async Task<IResult> Logout(string? returnUrl)
        {
            await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return Results.Redirect("/Account/Index");
            string refererUrl = HttpContext.Request.Headers["Referer"].ToString();
            return Results.Redirect(refererUrl);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> JSONSave()
        {
            try
            {
                var email = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.Name).ToList().First().Value;
                bool exists = Convert.ToBoolean(Request.Form["IsExistsInDB"]);
                FileSaveHelper fileSaveHelper = _fileSaveManager.Save(FileType.JSON, Request);
                bool res;

                if (exists)
                {
                    res = await _fileManager.UpdateFile(fileSaveHelper, Guid.Parse(Request.Form["FileId"]));
                }
                else
                {
                    res = await _fileManager.SaveFile(fileSaveHelper, email);
                }

                if (res)
                {
                    return NoContent();
                }
                else
                {
                    throw new ArgumentException("Ошибка при сохранении или обновлении файла. В базу данных записано 0 элементов");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CSVsave()
        {
            try
            {
                var email = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.Name).ToList().First().Value;
                bool exists = Convert.ToBoolean(Request.Form["IsExistsInDB"]);
                FileSaveHelper fileSaveHelper = _fileSaveManager.Save(FileType.CSV, Request);
                bool res;

                if (exists)
                {
                    res = await _fileManager.UpdateFile(fileSaveHelper, Guid.Parse(Request.Form["FileId"]));
                }
                else
                {
                    res = await _fileManager.SaveFile(fileSaveHelper, email);
                }

                if (res)
                {
                    return NoContent();
                }
                else
                {
                    throw new ArgumentException("Ошибка при сохранении или обновлении файла. В базу данных записано 0 элементов");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> XMLSave()
        {
            try
            {
                var email = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.Name).ToList().First().Value;
                bool exists = Convert.ToBoolean(Request.Form["IsExistsInDB"]);
                FileSaveHelper fileSaveHelper = _fileSaveManager.Save(FileType.XML, Request);
                bool res;

                if (exists)
                {
                    res = await _fileManager.UpdateFile(fileSaveHelper, Guid.Parse(Request.Form["FileId"]));
                }
                else
                {
                    res = await _fileManager.SaveFile(fileSaveHelper, email);
                }

                if (res)
                {
                    return NoContent();
                }
                else
                {
                    throw new ArgumentException("Ошибка при сохранении или обновлении файла. В базу данных записано 0 элементов");
                }
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        public AccountController(IUserManager manager, IFileManager fileManager, IFileSaveManager fileSaveManager, IPasswordHasher<User> passwordHasher, IViewModelsCreator viewModelsCreator)
        {
            _userManager = manager;
            _fileManager = fileManager;
            _fileSaveManager = fileSaveManager;
            _passwordHasher = passwordHasher;
            _viewModelsCreator = viewModelsCreator;
        }
    }
}
