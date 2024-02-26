using Newtonsoft.Json.Linq;

namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class JSONViewModel
    {
        public JToken Data { get; set; }
        //public string RawHTML { get; set; }
        public string Name { get; set; }
        public bool IsExistsInDB { get; set; } = false;
        public Guid? FileId { get; set; } = null;

        public JSONViewModel() { }

        public JSONViewModel(JToken data)
        {
            Data = data;
        }

        //public JSONViewModel(JToken data, string RawHTML)
        //{
        //    Data = data;
        //    this.RawHTML = RawHTML;
        //}

        public JSONViewModel(JToken data, string Name)
        {
            Data = data;
            //this.RawHTML = RawHTML;
            this.Name = Name;
        }

        public JSONViewModel(JToken data, string Name, bool IsExistsInDB)
        {
            Data = data;
            //this.RawHTML = RawHTML;
            this.Name = Name;
            this.IsExistsInDB = IsExistsInDB;
        }

        public JSONViewModel(JToken data, string Name, bool IsExistsInDB, Guid Id)
        {
            Data = data;
            //this.RawHTML = RawHTML;
            this.Name = Name;
            this.IsExistsInDB = IsExistsInDB;
            FileId = Id;
        }
    }
}
