using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Xml;
using Web_CSV_Json_XML_reader.Models;

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult DownloadFile(string filePath, string downloadFileName)
        {
            string contentType = "text/plain";
            return File(filePath, contentType, downloadFileName);           
        }

        public ActionResult DownloadFile(Stream file, string downloadFileName)
        {
            string contentType = "text/plain";
            return File(file, contentType, downloadFileName);
        }

        [HttpPost]
        public IActionResult TextSend(TextInputViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                    model.Name = "WebInput";

                string rawHTML;

                switch (model.Type)
                {
                    case FileType.CSV:
                        return View("CSV", CSVReader.TFPRead(model.Text, model.Name, model.CSVSeparator));

                    case FileType.XML:
                        XmlDocument xmlD;
                        rawHTML = XMLReader.Read(model.Text, out xmlD);
                        return View("XML", new XMLViewModel(rawHTML, xmlD, model.Name));

                    case FileType.JSON:
                        JToken token;
                        rawHTML = JSONReader.ReadForWeb(model.Text, out token);
                        return View("JSON", new JSONViewModel(token, rawHTML, model.Name));
                }

                return View("TextInput");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CSVsave()
        {
            try
            {
                return DownloadFile(FileSaver.SaveCSV(Request), FileSaver.GetFileName(FileType.CSV, Request.Form["Name"].ToString()));
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult JSONSave(JSONViewModel model)
        {
            try
            {
                model.Data = JToken.Parse(Request.Form["Data"]);
                return DownloadFile(FileSaver.SaveJSON(model.Data, Request), FileSaver.GetFileName(FileType.JSON, model.Name));
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult XMLSave(XMLViewModel model)
        {
            try
            {
                model.Data = new XmlDocument();
                model.Data.LoadXml(Request.Form["Data"]);

                return DownloadFile(FileSaver.SaveXML(model, Request), FileSaver.GetFileName(FileType.XML, model.Name));
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}