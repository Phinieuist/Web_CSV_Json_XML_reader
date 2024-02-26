using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Security.Claims;
using System.Xml;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.Models;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileSaveManager _fileSaveManager;
        private readonly IViewModelsCreator _viewModelsCreator;

        public HomeController(ILogger<HomeController> logger, IFileSaveManager fileSaveManager, IViewModelsCreator viewModelsCreator)
        {
            _logger = logger;
            _fileSaveManager = fileSaveManager;
            _viewModelsCreator = viewModelsCreator;
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

                if (string.IsNullOrEmpty(model.CSVSeparator))
                    model.CSVSeparator = CSVDataTable.DefaultSeparator;

                switch (model.Type)
                {
                    case FileType.CSV:
                        return View("CSV", _viewModelsCreator.GetCsvVM(model.Text, model.Name, model.CSVSeparator));

                    case FileType.XML:
                        return View("XML", _viewModelsCreator.GetXmlVM(model.Text, model.Name));

                    case FileType.JSON:
                        return View("JSON", _viewModelsCreator.GetJsonVM(model.Text, model.Name));
                }

                return View("TextInput");
            }
            catch (Exception ex)
            {
                return View("_Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CSVDownload()
        {
            try
            {
                FileSaveHelper fileSaveHelper = _fileSaveManager.Download(FileType.CSV, Request);
                return DownloadFile(fileSaveHelper.DataToSave, fileSaveHelper.FileName);
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [HttpPost]
        public IActionResult JSONDownload()
        {
            try
            {
                FileSaveHelper fileSaveHelper = _fileSaveManager.Download(FileType.JSON, Request);
                return DownloadFile(fileSaveHelper.DataToSave, fileSaveHelper.FileName);
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [HttpPost]
        public IActionResult XMLDownload()
        {
            try
            {
                FileSaveHelper fileSaveHelper = _fileSaveManager.Download(FileType.XML, Request);
                return DownloadFile(fileSaveHelper.DataToSave, fileSaveHelper.FileName);
            }
            catch (Exception ex)
            {
                return ViewResultCreator.Error(ex);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}