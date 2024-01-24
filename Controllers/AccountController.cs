using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Text;
using Web_CSV_Json_XML_reader.DB;
using Web_CSV_Json_XML_reader.Managers;
using Web_CSV_Json_XML_reader.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;

        public ActionResult Index()
        {
            return View("Login");
        }

        [Authorize]
        public string Index2()
        {
            var entities = _userManager.GetUsers();

            if (!entities.IsCompleted)
                entities.Start();

            StringBuilder sb = new StringBuilder();

            foreach (var item in entities.Result)
            {
                sb.Append(string.Join(" | ", new string[] { item.UserId.ToString(), item.Email, item.Password }));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string Index3()
        {
            IIdentity? identity = Request.HttpContext.User.Identity;

            if (identity is not null && identity.IsAuthenticated)
            {
                return $"Пользователь аутентифицирован. Тип аутентификации: {identity.AuthenticationType}";
            }
            else
            {
                return "Пользователь НЕ аутентифицирован";
            }
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

            if (!await _userManager.AddUser(email, password))
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

            User? user = await _userManager.GetUser(email, password);

            // если пользователь не найден, отправляем статусный код 401
            if (user is null) return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
            // создаем объект ClaimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            // установка аутентификационных куки
            
            await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Results.Redirect(returnUrl ?? "/");
        }

        [HttpGet]
        public async Task<IResult> Logout(string? returnUrl)
        {
            await Request.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return Results.Redirect("/Account/Index");
            return Results.Redirect(returnUrl ?? "/Account/Index");
        }

        

        public AccountController(IUserManager manager)
        {
            _userManager = manager;
        }
    }
}
