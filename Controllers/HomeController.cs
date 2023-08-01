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

        public string TestIn()
        {
            string text = Request.Query["text"];
            return "Inputted text: " + text;
        }

        public IActionResult csvtest()
        {
            //CSVDataTable dataTable = CSVReader.Read(CSVExample.Diabets, ",");
            CSVDataTable dataTable = CSVReader.TFPRead(CSVExample.DiabetsFile);
            return View("test", dataTable);
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
        public IActionResult csvsave()
        {
            return DownloadFile(FileSaver.SaveCSV(Request), FileSaver.GetFileName(FileType.CSV, Request.Form["Name"].ToString()));
        }


        public IActionResult JSON()
        {
            return View();
        }

        
        public IActionResult TextInput()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TextSend(TextInputViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
                model.Name = "WebInput";

            switch (model.Type)
            {
                case FileType.CSV:
                    string separator;
                    if (string.IsNullOrEmpty(model.CSVSeparator)) separator = ",";
                    else separator = model.CSVSeparator;

                    CSVDataTable dataTable = CSVReader.TFPReadText(model.Text, model.Name, separator);
                    dataTable.Separator = model.CSVSeparator;
                    dataTable.Name = model.Name;

                    return View("test", dataTable);
                
                case FileType.XML:
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(model.Text);

                    return View("XML", new XMLViewModel(xmlDoc, XMLReader.Read(xmlDoc), model.Name));
                
                case FileType.JSON:
                    JToken token = JToken.Parse(model.Text);
                    return View("JSON", new JSONViewModel(token, JSONReader.ReadJsonForWeb(token), model.Name));
            }

            return View("TextInput");
        }

        public IActionResult JSONSave(JSONViewModel model)
        {
            model.Data = JToken.Parse(Request.Form["Data"]);
            return DownloadFile(FileSaver.SaveJSON(model.Data, Request), FileSaver.GetFileName(FileType.JSON, model.Name));
        }

        public IActionResult XMLSave(XMLViewModel model)
        {
            model.Data = new XmlDocument();
            model.Data.LoadXml(Request.Form["Data"]);

            return DownloadFile(FileSaver.SaveXML(model, Request), FileSaver.GetFileName(FileType.XML, model.Name));
        }

        public IActionResult test()
        {
            return View("FileSave");
        }

        public object FileSave()
        {
            return Request.Form["FileType"].ToString();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}