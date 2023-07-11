using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
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

        public IActionResult csvsave(string? str)
        {
            CSVDataTable dataTable = new CSVDataTable();
            int i_lenght = Convert.ToInt32(Request.Form["i_lenght"]);
            int j_lenght = Convert.ToInt32(Request.Form["j_lenght"]);
            dataTable.Name = Request.Form["FileName"].ToString();
            FileSaver.FileType saveFileType = FileSaver.GetFileType(Request.Form["FileType"].ToString());

            for (int i = 0; i < i_lenght; i++)
            {
                dataTable.Data.Add(new string[j_lenght]);
                for (int j = 0; j < j_lenght; j++)
                {
                    dataTable.Data[i][j] = Request.Form[$"cell[{i},{j}]"].ToString();
                }
            }

            return DownloadFile(FileSaver.SaveCSV(dataTable), FileSaver.GetFileName(saveFileType, dataTable.Name));
            //return DownloadFile(FileSaver.Save(FileSaver.FileType.CSV, saveFileType, dataTable, null, null), FileSaver.GetFileName(saveFileType, dataTable.Name));

            //CSVReader.Save(dataTable, @"C:\Users\user\Desktop\2\универ\магистратура\1 курс 2 семестр\Интеллектуальный анализ данных\Сем2\diabetesShort_Saved.csv");
            //CSVReader.Save(dataTable, @".\diabetesShort_Saved.csv");
            //return DownloadFile(@".\diabetesShort_Saved.csv", dataTable.Name);
            //return View("test", dataTable);
        }

        public object jsontest()
        {
            //return JSONReader.Read(JSONExamlpes.SquidGame);
            return JSONReader.Read2();
        }

        public IActionResult JSON()
        {
            //JSONViewModel viewModel = new JSONViewModel(JSONReader.ReadToViewModel(JSONExamlpes.test6));

            string jsonHTML = JSONReader.ReadJsonForWeb(JToken.Parse(JSONExamlpes.test7));

            return View("JSON", jsonHTML);
        }

        public IActionResult TextInput()
        {
            return View();
        }

        public object JSONSave2()
        {
            //JSONViewModel dataTable = new JSONViewModel();
            JObject JSONobject = new JObject();
            int i_lenght = Convert.ToInt32(Request.Form["i_lenght"]);
            int j_lenght = Convert.ToInt32(Request.Form["j_lenght"]);
            List<JSONSaveObj> jSONSaves = new List<JSONSaveObj>();


            for (int i = 0; i < i_lenght; i++)
            {

                string value = "";
                List<string> strings = new List<string>();
                JSONSaveObj.JSONValueType type;

                switch (Request.Form[$"cell[{i},{1}]_type"].ToString())
                {
                    case "Object": value += "{"; type = JSONSaveObj.JSONValueType.Object; break;
                    case "Array": value += "["; type = JSONSaveObj.JSONValueType.Array; break;
                    default: type = JSONSaveObj.JSONValueType.None; break;
                }

                for (int j = 1; j < j_lenght; j++)
                {
                    if (!string.IsNullOrEmpty(Request.Form[$"cell[{i},{j}]"]))
                    {
                        if (Request.Form[$"cell[{i},{1}]_type"].ToString() == "Array")
                            value += Request.Form[$"cell[{i},{j}]"].ToString() + ",";
                        else
                            value += Request.Form[$"cell[{i},{j}]"].ToString() + ",";
                    }
                }

                switch (Request.Form[$"cell[{i},{1}]_type"].ToString())
                {
                    case "Object": jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), value.Substring(0, value.Length - 1) + "}", type)); break;
                    case "Array": jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), value.Substring(0, value.Length - 1) + "]", type)); break;
                    default: jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), value.Substring(0, value.Length - 1), type)); break;
                }
            }

            foreach (JSONSaveObj it in jSONSaves)
            {
                switch (it.Type)
                {
                    case JSONSaveObj.JSONValueType.Object: JSONobject.Add(it.Name, JObject.Parse((string)it.Value)); break;
                    case JSONSaveObj.JSONValueType.Array: JSONobject.Add(it.Name, JObject.Parse((string)it.Value)); break;
                    case JSONSaveObj.JSONValueType.None: JSONobject.Add(it.Name, (string)it.Value); break;
                }
            }

            return JsonConvert.SerializeObject(JSONobject, Formatting.Indented);
        }

        public object JSONSave()
        {
            //JSONViewModel dataTable = new JSONViewModel();
            JObject JSONobject = new JObject();
            int i_lenght = Convert.ToInt32(Request.Form["i_lenght"]);
            int j_lenght = Convert.ToInt32(Request.Form["j_lenght"]);
            List<JSONSaveObj> jSONSaves = new List<JSONSaveObj>();


            for (int i = 0; i < i_lenght; i++)
            {

                string value = "";
                List<string> strings = new List<string>();
                JSONSaveObj.JSONValueType type;

                switch (Request.Form[$"cell[{i},{1}]_type"].ToString())
                {
                    case "Object": value += "{"; type = JSONSaveObj.JSONValueType.Object; break;
                    case "Array": type = JSONSaveObj.JSONValueType.Array; break;
                    default: type = JSONSaveObj.JSONValueType.None; break;
                }

                for (int j = 1; j < j_lenght; j++)
                {                  
                    if (!string.IsNullOrEmpty(Request.Form[$"cell[{i},{j}]"]))
                    {
                        if (Request.Form[$"cell[{i},{1}]_type"].ToString() == "Array")
                            strings.Add(Request.Form[$"cell[{i},{j}]"].ToString());
                        else
                            value += Request.Form[$"cell[{i},{j}]"].ToString() + ",";
                    }
                }

                switch (Request.Form[$"cell[{i},{1}]_type"].ToString())
                {
                    case "Object": jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), value.Substring(0, value.Length - 1) + "}", type)); break;
                    case "Array": jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), strings.ToArray(), type)); break;
                    default: jSONSaves.Add(new JSONSaveObj(Request.Form[$"cell[{i},{0}]"].ToString(), value.Substring(0, value.Length - 1), type)); break;
                }
            }

            foreach (JSONSaveObj it in jSONSaves)
            {
                switch (it.Type)
                {
                    case JSONSaveObj.JSONValueType.Object: JSONobject.Add(it.Name, JObject.Parse((string)it.Value)); break;
                    case JSONSaveObj.JSONValueType.Array: JSONobject.Add(it.Name, JArray.FromObject(it.Value)); break;
                    case JSONSaveObj.JSONValueType.None: JSONobject.Add(it.Name, (string)it.Value); break;
                }
            }

            return JsonConvert.SerializeObject(JSONobject, Formatting.Indented);
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