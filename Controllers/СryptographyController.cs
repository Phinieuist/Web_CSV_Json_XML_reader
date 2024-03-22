using Microsoft.AspNetCore.Mvc;
using System.Text;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.Models;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class СryptographyController : Controller
    {
        private readonly IСryptographyManager _cryptographyManager;

        public СryptographyController(IСryptographyManager сryptographyManager)
        {
            _cryptographyManager = сryptographyManager;
        }

        public IActionResult Index()
        {
            return View(new СryptographyViewModel() { ServerPublicKey = ServerKeys.PublicKey });
        }

        public IActionResult GetNewKeyPair()
        {
            var pair = _cryptographyManager.GetNewKeyPair();
            var publicKey = pair.ExportRSAPublicKey();
            var privateKey = pair.ExportRSAPrivateKey();

            return Json(new object[] { publicKey, privateKey });
        }

        public IActionResult GetSignature(СryptographyViewModel model)
        {
            try
            {
                if (model.PrivateKey is null)
                {
                    model.ErrorMessage = "Необходимо ввести закрытый ключ";
                    return View("Index", model);
                }
                if (model.MessageText is null)
                {
                    model.ErrorMessage = "Необходимо ввести сообщение для получения подписи";
                    return View("Index", model);
                }
                model.ResetServiceMessages();
                
                byte[] privateKey = Convert.FromBase64String(model.PrivateKey);
                model.SignatureText = Convert.ToBase64String(_cryptographyManager.GetSignature(model.MessageText, privateKey));
                return View("Index", model);
            }
            catch(FormatException ex)
            {
                return ViewResultCreator.Error(new Exception("При передаче закрытого ключа возникла ошибка. Проверьте правильность введённых данных.", ex));
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        public IActionResult CheckSignature(СryptographyViewModel model)
        {
            try
            {
                if (model.PublicKey is null)
                {
                    model.ErrorMessage = "Необходимо ввести публичный ключ";
                    return View("Index", model);
                }
                if (model.MessageText is null)
                {
                    model.ErrorMessage = "Необходимо ввести сообщение, которое необходимо проверить";
                    return View("Index", model);
                }
                if (model.SignatureText is null)
                {
                    model.ErrorMessage = "Необходимо предоставить подпись для сообщения";
                    return View("Index", model);
                }
                model.ResetServiceMessages();
                
                byte[] publicKey = Convert.FromBase64String(model.PublicKey);
                byte[] signature = Convert.FromBase64String(model.SignatureText);

                if (_cryptographyManager.CheckSignature(model.MessageText, publicKey, signature))
                    model.AlertMessage = "Верефикация пройдена! Сообщение не было изменено.";
                else
                    model.AlertMessage = "Верефикация не пройдена.";

                return View("Index", model);
            }
            catch (FormatException ex)
            {
                return ViewResultCreator.Error(new Exception("При передаче ключей или подписи возникла ошибка. Проверьте правильность введённых данных.", ex));
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        public IActionResult GetMessageFromServer(СryptographyViewModel model)
        {
            try
            {
                
                string message = "Привет! Это сервер, далее пойдёт строка с рандомными символами";
                char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=+_()*&^%$#@!,.; \n".ToCharArray();

                int charCount = new Random().Next(30, 100);
                StringBuilder sb = new ();

                Random rand = new Random();
                for (int i = 0; i < charCount; i++)
                {
                    sb.Append(letters[rand.Next(0, letters.Length - 1)]);
                }
                message += sb.ToString();

                message = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(message));
                model.MessageText = message;
                model.ResetServiceMessages();

                byte[] privateKey = Convert.FromBase64String(ServerKeys.PrivateKey);
                model.SignatureText = Convert.ToBase64String(_cryptographyManager.GetSignature(message, privateKey));
                return View("Index", model);
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }
    }
}
