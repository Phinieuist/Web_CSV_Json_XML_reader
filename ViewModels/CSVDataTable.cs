using Microsoft.AspNetCore.Routing.Constraints;
using System.Data;
using Web_CSV_Json_XML_reader.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_CSV_Json_XML_reader.ViewModels
{
    public class CSVDataTable
    {
        public static readonly string DefaultSeparator = ",";

        public List<string[]> Data { get; set; }
        public string Name { get; set; }
        public string Separator { get; set; }
        public bool IsExistsInDB { get; set; } = false;
        public Guid? FileId { get; set; } = null;

        public DataTable ToDataTable(bool FirstRowIsColumnName = true)
        {
            DataTable result = new DataTable();
            if (Data.Count > 0)
            {
                int startNum = 0;

                if (FirstRowIsColumnName)
                {
                    startNum = 1;
                    foreach (string columnName in Data[0])
                    {
                        result.Columns.Add(columnName);
                    }
                }

                for (int i = startNum; i < Data.Count; i++)
                {
                    result.Rows.Add(Data[i]);
                }
            }
            return result;
        }


        public CSVDataTable()
        {
            Data = new List<string[]>();
        }

        public CSVDataTable(List<string[]> data)
        {
            Data = data;
        }

        public CSVDataTable(List<string[]> data, string name)
        {
            Data = data;
            Name = name;
        }

        public CSVDataTable(List<string[]> data, string name, string separator)
        {
            Data = data;
            Name = name;
            Separator = separator;
        }

        public CSVDataTable(List<string[]> data, string name, string separator, bool IsExistsInDB)
        {
            Data = data;
            Name = name;
            Separator = separator;
            this.IsExistsInDB = IsExistsInDB;
        }

        public CSVDataTable(List<string[]> data, string name, string separator, bool IsExistsInDB, Guid FileID)
        {
            Data = data;
            Name = name;
            Separator = separator;
            this.IsExistsInDB = IsExistsInDB;
            FileId = FileID;
        }
    }
}
