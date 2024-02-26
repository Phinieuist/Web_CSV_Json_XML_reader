using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Reflection;
using Web_CSV_Json_XML_reader.ViewModels;

namespace Web_CSV_Json_XML_reader.Models
{
    public class CSVReader
    {
        public static CSVDataTable Read(string input, string separator)
        {
            string[] Lines;
            List<string[]> Columns = new List<string[]>();

            Lines = input.Split("\n");
            
            foreach (string line in Lines)
            {
                Columns.Add(line.Replace("\"", "").Split(separator));
            }

            return new CSVDataTable(Columns);
        }

        public static CSVDataTable TFPReadText(string csvtext, string outputName, string separator = ",")
        {
            try
            {
                List<string[]> result = new List<string[]>();

                StringReader stringReader = new StringReader(csvtext);
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

                return new CSVDataTable(result, outputName);
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

        public static CSVDataTable TFPRead(string csvtext, string outputName, string separator, bool IsExistsInDB, Guid FileID)
        {
            CSVDataTable dataTable = TFPRead(csvtext, outputName, separator);
            dataTable.IsExistsInDB = IsExistsInDB;
            dataTable.FileId = FileID;

            return dataTable;
        }

        public static CSVDataTable TFPRead(string csvtext, string outputName, string separator)
        {
            try
            {
                if (string.IsNullOrEmpty(separator))
                    separator = ",";


                CSVDataTable dataTable = TFPReadText(csvtext, outputName, separator);
                dataTable.Separator = separator;
                dataTable.Name = outputName;

                return dataTable;
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка в процессе чтения CSV-документа. " + ex.Message);
            }          
        }
    }
}
