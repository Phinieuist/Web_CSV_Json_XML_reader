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

        public static CSVDataTable TFPRead(string filePath, string separator = ",")
        {
            List<string[]> result = new List<string[]>();
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(separator);
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    result.Add(fields);
                }
            }
            
            return new CSVDataTable(result, Path.GetFileName(filePath));
        }

        public static void Save(CSVDataTable dataTable, string Path, string separator = ",")
        {
            string Output = "";

            foreach (string[] line in dataTable.Data)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (i != line.Length - 1)
                        Output += '"' + line[i] + '"' + separator;
                    else
                        Output += '"' + line[i] + '"' + "\n";

                }
            }

            Output = Output.Remove(Output.LastIndexOf('\n'));

            File.WriteAllText(Path, Output);
        }

        public static MemoryStream Save(CSVDataTable dataTable, string separator = ",")
        {
            string Output = "";

            foreach (string[] line in dataTable.Data)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (i != line.Length - 1)
                        Output += '"' + line[i] + '"' + separator;
                    else
                        Output += '"' + line[i] + '"' + "\n";

                }
            }

            Output = Output.Remove(Output.LastIndexOf('\n'));

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            writer.Write(Output);

            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
