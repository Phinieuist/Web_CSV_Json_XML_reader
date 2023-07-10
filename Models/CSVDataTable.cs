using Microsoft.AspNetCore.Routing.Constraints;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_CSV_Json_XML_reader.Models
{
    public class CSVDataTable
    {
        public List<string[]> Data { get; set; }
        public string Name {get;set;}

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
    }
}
