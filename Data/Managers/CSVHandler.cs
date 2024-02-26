using Microsoft.VisualBasic.FileIO;
using System.Text;
using Web_CSV_Json_XML_reader.Data.Managers.Interfaces;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Data.Managers
{
    public class CSVHandler : ICSVManager
    {
        public CSVDataTable GetCSVDataTable(HttpRequest Request)
        {
            try
            {
                CSVDataTable dataTable = new CSVDataTable();
                int i_lenght = Convert.ToInt32(Request.Form["i_lenght"]);
                int j_lenght = Convert.ToInt32(Request.Form["j_lenght"]);
                dataTable.Name = Request.Form["Name"].ToString();

                dataTable.Separator = Request.Form["Separator"].ToString();
                if (string.IsNullOrEmpty(dataTable.Separator)) dataTable.Separator = CSVDataTable.DefaultSeparator;

                for (int i = 0; i < i_lenght; i++)
                {
                    dataTable.Data.Add(new string[j_lenght]);
                    for (int j = 0; j < j_lenght; j++)
                    {
                        dataTable.Data[i][j] = Request.Form[$"cell[{i},{j}]"].ToString();
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при формировании CSVDataTable {ex.Message}", ex);
            }
        }

        public CSVDataTable GetCsvVM(string csvText, string fileName, string separator = ",")
        {
            try
            {
                List<string[]> result = new List<string[]>();

                StringReader stringReader = new StringReader(csvText);
                using (TextFieldParser parser = new TextFieldParser(stringReader))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(separator);
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        result.Add(fields);
                    }
                }

                return new CSVDataTable(result, fileName, separator);
            }
            catch (MalformedLineException ex)
            {
                string[] exMessage = ex.Message.Split(' ');
                throw new Exception($"Строка {exMessage[1]}. Неверный разделитель. (\"{separator}\")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
