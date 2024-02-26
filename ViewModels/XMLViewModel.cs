using Newtonsoft.Json.Linq;
using System.Xml;

namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class XMLViewModel
    {
        public string Name { get; set; }
        public XmlDocument Data { get; set; }
        public bool IsExistsInDB { get; set; } = false;
        public Guid? FileId { get; set; } = null;

        public XMLViewModel() { }

        public XMLViewModel(XmlDocument data)
        {
            Data = data;
        }

        public XMLViewModel(XmlDocument data, string Name)
        {
            Data = data;
            this.Name = Name;
        }

        public XMLViewModel(XmlDocument data, string Name, bool IsExistsInDB)
        {
            Data = data;
            this.Name = Name;
            this.IsExistsInDB = IsExistsInDB;
        }

        public XMLViewModel(XmlDocument data, string Name, bool IsExistsInDB, Guid FileId)
        {
            Data = data;
            this.Name = Name;
            this.FileId = FileId;
            this.IsExistsInDB = IsExistsInDB;
        }
    }
}
