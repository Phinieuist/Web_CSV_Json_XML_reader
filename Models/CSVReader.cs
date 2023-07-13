using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace Web_CSV_Json_XML_reader.Models
{
 
    public class CSVExample
    {
        public static string DiabetsFile => @"C:\Users\user\Desktop\2\универ\магистратура\1 курс 2 семестр\Интеллектуальный анализ данных\Сем2\diabetesShort.csv";
        public static string Diabets { get => File.ReadAllText(@"C:\Users\user\Desktop\2\универ\магистратура\1 курс 2 семестр\Интеллектуальный анализ данных\Сем2\diabetesShort.csv"); }
    }

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

        public static CSVDataTable TFPReadText(string csvtext, string outputName = "temp", string separator = ",")
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

        public static CSVDataTable TFPRead(string filePath, string separator = ",")
        {
            string text = File.ReadAllText(filePath);
            TFPRead(text, Path.GetFileName(filePath));

            return TFPReadText(text, Path.GetFileName(filePath), separator);
        }

    }
}
