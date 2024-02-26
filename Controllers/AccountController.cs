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

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IFileManager _fileManager;
        private readonly IFileSaveManager _fileSaveManager;
        private readonly IPasswordHasher<User> _passwordHasher;

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
            string userID = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).First().Value;
            return View(await _fileManager.GetFiles(Guid.Parse(userID)));
        }

        [Authorize]
        public async Task<IResult> DeleteFile(Guid FileId)
        {
            if (await _fileManager.DeleteFile(FileId))
                return Results.Redirect("/Account/SavedFiles");
            else
                return Results.BadRequest("Ошибка при удалении файла");
        }

        [Authorize]
        public async Task<IActionResult> OpenFile(Guid FileId)
        {
            try
            {
                return await _fileManager.OpenFile(FileId);
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
            Web_CSV_Json_XML_reader.Data.DB.Entities.User user = await _userManager.GetUser(Guid.Parse(User.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).First().Value.ToString()));

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.oldPassword);

            if (verificationResult is PasswordVerificationResult.Failed) 
                return ViewResultCreator.Error("Введён неверный текущий пароль при попытке смены пароля");

            Data.DB.Entities.User newUser = new User(user);
            newUser.Password = _passwordHasher.HashPassword(newUser, model.newPassword);

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

        [Authorize]
        public async Task<ActionResult> DeleteUser()
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

        public string testDelete()
        {
            return "UserDeleted";
        }

        public ActionResult Register()
        {
            return View();
        }

        public async Task<IResult> RegisterNew()
        {
            var form = Request.Form;

            if (!form.ContainsKey("email") || !form.ContainsKey("password") || !form.ContainsKey("passwordRepeat"))
                return Results.BadRequest("Email и/или пароль не установлены");

            string email = form["email"];
            string password = form["password"];

            if (password != form["passwordRepeat"])
                return Results.BadRequest("Пароли не совпадают");

            string encryptedPassword = _passwordHasher.HashPassword(null, password);

            if (!await _userManager.AddUser(email, encryptedPassword))
                return Results.BadRequest("Произошла ошибка при добавлении пользователя");
            else 
                return Results.Redirect("/");
        }

        [HttpPost]
        public async Task<IResult> Login(string? returnUrl)
        {
            var form = Request.Form;

            if (!form.ContainsKey("email") || !form.ContainsKey("password"))
                return Results.BadRequest("Email и/или пароль не установлены");

            string email = form["email"];
            string password = form["password"];

            // находим пользователя 
            //Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);

            User? user = await _userManager.GetUser(email);

            if (user is null) return Results.Unauthorized();

            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            // если пользователь не найден, отправляем статусный код 401
            if (verificationResult is PasswordVerificationResult.Failed) return Results.Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // установка аутентификационных куки
            
            await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Results.Redirect(returnUrl ?? "/");
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
            //var claims = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.Name).ToList();
            //model.Data = JToken.Parse(Request.Form["Data"]);
            //if (await _fileManager.SaveFile(FileSaver.SaveJSON(model.Data, Request), FileSaver.GetFileName(FileType.JSON, model.Name), claims.First().Value))
            //{
            //    string refererUrl = HttpContext.Request.Headers["Referer"].ToString();
            //    return Results.Redirect(refererUrl); 
            //}
            //else
            //    return Results.BadRequest("Ошибка при сохранении файла");
            try
            {
                var email = Request.HttpContext.User.Claims.Where(q => q.Type == ClaimTypes.Name).ToList().First().Value;
                bool exists = Convert.ToBoolean(Request.Form["IsExistsInDB"]);
                FileSaveHelper fileSaveHelper = _fileSaveManager.Save(FileType.JSON, Request);
                bool res;

                if (exists)
                {
                    res = await _fileManager.UpdateFile(fileSaveHelper, Guid.Parse(Request.Form["FileId"]));
                    //throw new ArgumentException("IT exists | " + Guid.Parse(Request.Form["FileId"]));
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

        public AccountController(IUserManager manager, IFileManager fileManager, IFileSaveManager fileSaveManager, IPasswordHasher<User> passwordHasher)
        {
            _userManager = manager;
            _fileManager = fileManager;
            _fileSaveManager = fileSaveManager;
            _passwordHasher = passwordHasher;
        }
    }
}
