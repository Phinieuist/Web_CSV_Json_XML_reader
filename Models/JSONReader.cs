using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace Web_CSV_Json_XML_reader.Models
{
    public class JSONExamlpes
    {
        //public static string SquidGame => @"
        //{
        //"Name": "Squid Game",
        //"Genre"": "Thriller",
        //"Rating": {
        //    "Imdb": 8.1,
        //    "Rotten Tomatoes": 0.94
        //},
        //"Year": 2021,
        //"Stars": ["Lee Jung-jae", "Park Hae-soo"],
        //"Language": "Korean",
        //"Budget": "$21.4 million"
        //}";

        public static string test2 => @"{
        ""Name"": ""warqqq"",
        ""Genre"": ""lol"",
        ""Rating"": {
            ""Imdb"": 234234,
            ""Tomatoes"": 0.94
         },
         ""Year"": ""2021"",
         ""Stars"": [
            ""Lee Jung-jae"",
              ""Park Hae-soo""
                  ],
            ""Language"": ""Korean_01010101"",
             ""Budget"": ""$21.4 million""
        }";

        public static string test3 => @"{
  ""name"": ""John"",
  ""age"": 30,
  ""city"": ""New York"",
  ""pets"": [
    {
      ""type"": ""dog"",
      ""name"": ""Buddy""
    },
    {
      ""type"": ""cat"",
      ""name"": ""Fluffy""
    }
  ],
  ""family"": {
    ""father"": {
      ""name"": ""Tom"",
      ""age"": 55
    },
    ""mother"": {
      ""name"": ""Jane"",
      ""age"": 50
    }
  },
    ""bool test"": true
}";

        public static string test4 => @"{
  ""name"": ""John"",
  ""age"": 30,
  ""city"": ""New York"",
  ""pets"": [
    {
      ""type"": ""dog"",
      ""name"": ""Buddy""
    },
    {
      ""type"": ""cat"",
      ""name"": ""Fluffy""
    }
  ]
}";
        public static string test5 => @"{
   ""name"":""Jack"",
   ""age"":30,
   ""contactNumbers"":[
      {
         ""type"":""Home"",
         ""number"":""123 123-123""
      },
      {
         ""type"":""Office"",
         ""number"":""321 321-321""
      }
   ],
   ""favoriteSports"":[
      ""Football"",
      ""Cricket""
   ]
}";

        public static string test6 => @"[{""downoaded JSON"":""6"",""Glucose"":""downoaded JSON"",""BloodPressure"":""72"",""SkinThickness"":""35"",""Insulin"":""0"",""BMI"":""33.6"",""DiabetesPedigreeFunction"":""0.627"",""Age"":""50"",""Outcome"":""1""},{""downoaded JSON"":""1"",""Glucose"":""85"",""BloodPressure"":""downoaded JSON"",""SkinThickness"":""29"",""Insulin"":""0"",""BMI"":""26.6"",""DiabetesPedigreeFunction"":""0.351"",""Age"":""31"",""Outcome"":""0""},{""downoaded JSON"":""8"",""Glucose"":""183"",""BloodPressure"":""64"",""SkinThickness"":""downoaded JSON"",""Insulin"":""0"",""BMI"":""23.3"",""DiabetesPedigreeFunction"":""0.672"",""Age"":""32"",""Outcome"":""1""},{""downoaded JSON"":""1"",""Glucose"":""89"",""BloodPressure"":""66"",""SkinThickness"":""23"",""Insulin"":""downoaded JSON"",""BMI"":""28.1"",""DiabetesPedigreeFunction"":""0.167"",""Age"":""21"",""Outcome"":""0""},{""downoaded JSON"":""0"",""Glucose"":""137"",""BloodPressure"":""40"",""SkinThickness"":""35"",""Insulin"":""168"",""BMI"":""downoaded JSON"",""DiabetesPedigreeFunction"":""2.288"",""Age"":""33"",""Outcome"":""1""},{""downoaded JSON"":""5"",""Glucose"":""116"",""BloodPressure"":""74"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""25.6"",""DiabetesPedigreeFunction"":""downoaded JSON"",""Age"":""30"",""Outcome"":""0""},{""downoaded JSON"":""3"",""Glucose"":""78"",""BloodPressure"":""50"",""SkinThickness"":""32"",""Insulin"":""88"",""BMI"":""31"",""DiabetesPedigreeFunction"":""0.248"",""Age"":""downoaded JSON"",""Outcome"":""1""},{""downoaded JSON"":""10"",""Glucose"":""115"",""BloodPressure"":""0"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""35.3"",""DiabetesPedigreeFunction"":""0.134"",""Age"":""29"",""Outcome"":""downoaded JSON""},{""downoaded JSON"":""2"",""Glucose"":""197"",""BloodPressure"":""70"",""SkinThickness"":""45"",""Insulin"":""543"",""BMI"":""30.5"",""DiabetesPedigreeFunction"":""0.158"",""Age"":""53"",""Outcome"":""1""},{""downoaded JSON"":""8"",""Glucose"":""125"",""BloodPressure"":""96"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""0"",""DiabetesPedigreeFunction"":""0.232"",""Age"":""54"",""Outcome"":""1""},{""downoaded JSON"":""4"",""Glucose"":""110"",""BloodPressure"":""92"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""37.6"",""DiabetesPedigreeFunction"":""0.191"",""Age"":""30"",""Outcome"":""0""},{""downoaded JSON"":""10"",""Glucose"":""168"",""BloodPressure"":""74"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""38"",""DiabetesPedigreeFunction"":""0.537"",""Age"":""34"",""Outcome"":""1""},{""downoaded JSON"":""10"",""Glucose"":""139"",""BloodPressure"":""80"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""27.1"",""DiabetesPedigreeFunction"":""1.441"",""Age"":""57"",""Outcome"":""0""},{""downoaded JSON"":""1"",""Glucose"":""189"",""BloodPressure"":""60"",""SkinThickness"":""23"",""Insulin"":""846"",""BMI"":""30.1"",""DiabetesPedigreeFunction"":""0.398"",""Age"":""59"",""Outcome"":""1""},{""downoaded JSON"":""5"",""Glucose"":""166"",""BloodPressure"":""72"",""SkinThickness"":""19"",""Insulin"":""175"",""BMI"":""25.8"",""DiabetesPedigreeFunction"":""0.587"",""Age"":""51"",""Outcome"":""1""},{""downoaded JSON"":""7"",""Glucose"":""100"",""BloodPressure"":""0"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""30"",""DiabetesPedigreeFunction"":""0.484"",""Age"":""32"",""Outcome"":""1""},{""downoaded JSON"":""0"",""Glucose"":""118"",""BloodPressure"":""84"",""SkinThickness"":""47"",""Insulin"":""230"",""BMI"":""45.8"",""DiabetesPedigreeFunction"":""0.551"",""Age"":""31"",""Outcome"":""1""},{""downoaded JSON"":""7"",""Glucose"":""107"",""BloodPressure"":""74"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""29.6"",""DiabetesPedigreeFunction"":""0.254"",""Age"":""31"",""Outcome"":""1""},{""downoaded JSON"":""1"",""Glucose"":""103"",""BloodPressure"":""30"",""SkinThickness"":""38"",""Insulin"":""83"",""BMI"":""43.3"",""DiabetesPedigreeFunction"":""0.183"",""Age"":""33"",""Outcome"":""0""},{""downoaded JSON"":""1"",""Glucose"":""115"",""BloodPressure"":""70"",""SkinThickness"":""30"",""Insulin"":""96"",""BMI"":""34.6"",""DiabetesPedigreeFunction"":""0.529"",""Age"":""32"",""Outcome"":""1""},{""downoaded JSON"":""3"",""Glucose"":""126"",""BloodPressure"":""88"",""SkinThickness"":""41"",""Insulin"":""235"",""BMI"":""39.3"",""DiabetesPedigreeFunction"":""0.704"",""Age"":""27"",""Outcome"":""0""},{""downoaded JSON"":""8"",""Glucose"":""99"",""BloodPressure"":""84"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""35.4"",""DiabetesPedigreeFunction"":""0.388"",""Age"":""50"",""Outcome"":""0""},{""downoaded JSON"":""7"",""Glucose"":""196"",""BloodPressure"":""90"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""39.8"",""DiabetesPedigreeFunction"":""0.451"",""Age"":""41"",""Outcome"":""1""},{""downoaded JSON"":""9"",""Glucose"":""119"",""BloodPressure"":""80"",""SkinThickness"":""35"",""Insulin"":""0"",""BMI"":""29"",""DiabetesPedigreeFunction"":""0.263"",""Age"":""29"",""Outcome"":""1""},{""downoaded JSON"":""11"",""Glucose"":""143"",""BloodPressure"":""94"",""SkinThickness"":""33"",""Insulin"":""146"",""BMI"":""36.6"",""DiabetesPedigreeFunction"":""0.254"",""Age"":""51"",""Outcome"":""1""},{""downoaded JSON"":""10"",""Glucose"":""125"",""BloodPressure"":""70"",""SkinThickness"":""26"",""Insulin"":""115"",""BMI"":""31.1"",""DiabetesPedigreeFunction"":""0.205"",""Age"":""41"",""Outcome"":""1""},{""downoaded JSON"":""7"",""Glucose"":""147"",""BloodPressure"":""76"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""39.4"",""DiabetesPedigreeFunction"":""0.257"",""Age"":""43"",""Outcome"":""1""},{""downoaded JSON"":""1"",""Glucose"":""97"",""BloodPressure"":""66"",""SkinThickness"":""15"",""Insulin"":""140"",""BMI"":""23.2"",""DiabetesPedigreeFunction"":""0.487"",""Age"":""22"",""Outcome"":""0""},{""downoaded JSON"":""13"",""Glucose"":""145"",""BloodPressure"":""82"",""SkinThickness"":""19"",""Insulin"":""110"",""BMI"":""22.2"",""DiabetesPedigreeFunction"":""0.245"",""Age"":""57"",""Outcome"":""0""},{""downoaded JSON"":""5"",""Glucose"":""117"",""BloodPressure"":""92"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""34.1"",""DiabetesPedigreeFunction"":""0.337"",""Age"":""38"",""Outcome"":""0""},{""downoaded JSON"":""5"",""Glucose"":""109"",""BloodPressure"":""75"",""SkinThickness"":""26"",""Insulin"":""0"",""BMI"":""36"",""DiabetesPedigreeFunction"":""0.546"",""Age"":""60"",""Outcome"":""0""},{""downoaded JSON"":""3"",""Glucose"":""158"",""BloodPressure"":""76"",""SkinThickness"":""36"",""Insulin"":""245"",""BMI"":""31.6"",""DiabetesPedigreeFunction"":""0.851"",""Age"":""28"",""Outcome"":""1""},{""downoaded JSON"":""3"",""Glucose"":""88"",""BloodPressure"":""58"",""SkinThickness"":""11"",""Insulin"":""54"",""BMI"":""24.8"",""DiabetesPedigreeFunction"":""0.267"",""Age"":""22"",""Outcome"":""0""},{""downoaded JSON"":""6"",""Glucose"":""92"",""BloodPressure"":""92"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""19.9"",""DiabetesPedigreeFunction"":""0.188"",""Age"":""28"",""Outcome"":""0""},{""downoaded JSON"":""10"",""Glucose"":""122"",""BloodPressure"":""78"",""SkinThickness"":""31"",""Insulin"":""0"",""BMI"":""27.6"",""DiabetesPedigreeFunction"":""0.512"",""Age"":""45"",""Outcome"":""0""},{""downoaded JSON"":""4"",""Glucose"":""103"",""BloodPressure"":""60"",""SkinThickness"":""33"",""Insulin"":""192"",""BMI"":""24"",""DiabetesPedigreeFunction"":""0.966"",""Age"":""33"",""Outcome"":""0""},{""downoaded JSON"":""11"",""Glucose"":""138"",""BloodPressure"":""76"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""33.2"",""DiabetesPedigreeFunction"":""0.42"",""Age"":""35"",""Outcome"":""0""},{""downoaded JSON"":""9"",""Glucose"":""102"",""BloodPressure"":""76"",""SkinThickness"":""37"",""Insulin"":""0"",""BMI"":""32.9"",""DiabetesPedigreeFunction"":""0.665"",""Age"":""46"",""Outcome"":""1""},{""downoaded JSON"":""2"",""Glucose"":""90"",""BloodPressure"":""68"",""SkinThickness"":""42"",""Insulin"":""0"",""BMI"":""38.2"",""DiabetesPedigreeFunction"":""0.503"",""Age"":""27"",""Outcome"":""1""},{""downoaded JSON"":""4"",""Glucose"":""111"",""BloodPressure"":""72"",""SkinThickness"":""47"",""Insulin"":""207"",""BMI"":""37.1"",""DiabetesPedigreeFunction"":""1.39"",""Age"":""56"",""Outcome"":""1""},{""downoaded JSON"":""3"",""Glucose"":""180"",""BloodPressure"":""64"",""SkinThickness"":""25"",""Insulin"":""70"",""BMI"":""34"",""DiabetesPedigreeFunction"":""0.271"",""Age"":""26"",""Outcome"":""0""},{""downoaded JSON"":""7"",""Glucose"":""133"",""BloodPressure"":""84"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""40.2"",""DiabetesPedigreeFunction"":""0.696"",""Age"":""37"",""Outcome"":""0""},{""downoaded JSON"":""7"",""Glucose"":""106"",""BloodPressure"":""92"",""SkinThickness"":""18"",""Insulin"":""0"",""BMI"":""22.7"",""DiabetesPedigreeFunction"":""0.235"",""Age"":""48"",""Outcome"":""0""},{""downoaded JSON"":""9"",""Glucose"":""171"",""BloodPressure"":""110"",""SkinThickness"":""24"",""Insulin"":""240"",""BMI"":""45.4"",""DiabetesPedigreeFunction"":""0.721"",""Age"":""54"",""Outcome"":""1""},{""downoaded JSON"":""7"",""Glucose"":""159"",""BloodPressure"":""64"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""27.4"",""DiabetesPedigreeFunction"":""0.294"",""Age"":""40"",""Outcome"":""0""},{""downoaded JSON"":""0"",""Glucose"":""180"",""BloodPressure"":""66"",""SkinThickness"":""39"",""Insulin"":""0"",""BMI"":""42"",""DiabetesPedigreeFunction"":""1.893"",""Age"":""25"",""Outcome"":""1""},{""downoaded JSON"":""1"",""Glucose"":""146"",""BloodPressure"":""56"",""SkinThickness"":""0"",""Insulin"":""0"",""BMI"":""29.7"",""DiabetesPedigreeFunction"":""0.564"",""Age"":""29"",""Outcome"":""0""},{""downoaded JSON"":""2"",""Glucose"":""71"",""BloodPressure"":""70"",""SkinThickness"":""27"",""Insulin"":""0"",""BMI"":""28"",""DiabetesPedigreeFunction"":""0.586"",""Age"":""22"",""Outcome"":""0""},{""downoaded JSON"":""7"",""Glucose"":""103"",""BloodPressure"":""66"",""SkinThickness"":""32"",""Insulin"":""0"",""BMI"":""39.1"",""DiabetesPedigreeFunction"":""0.344"",""Age"":""31"",""Outcome"":""1""}]";

        public static string test7 => @"{""UserProfile"":{""gdprSeen"":true,""gdprAccept"":true,""areModsPermitted"":1,""lastAgreement"":1,""modTags"":{""SslSubtype"":[""ModBrowserFilterTagGroup""],""SslType"":""array"",""SslValue"":[{""isHidden"":false,""tagGroupName"":""Type"",""tags"":[""Vehicle New"",""Vehicle Tweak"",""Map""],""isDropDownType"":true},{""isHidden"":false,""tagGroupName"":""Vehicle"",""tags"":[""Motorbike"",""Car"",""4x4"",""Truck"",""Other""],""isDropDownType"":true},{""isHidden"":false,""tagGroupName"":""Map"",""tags"":[""Winter"",""Summer"",""Autumn"",""Spring""],""isDropDownType"":true},{""isHidden"":false,""tagGroupName"":""Players"",""tags"":[""Multiplayer"",""Singleplayer""],""isDropDownType"":false},{""isHidden"":false,""tagGroupName"":""Changes"",""tags"":[""Appearance"",""Crane"",""Engine"",""Names"",""Physics"",""Settings"",""Sound"",""Trailer"",""Transmission"",""Tires"",""Winch""],""isDropDownType"":false}]},""lastSaves"":1,""modDependencies"":{""SslType"":""ModDependencies"",""SslValue"":{""dependencies"":{""723284"":[],""1107710"":[]}}},""modFilter"":{""user0"":{""SslType"":""ModBrowserConfigData"",""SslValue"":{""tags"":[],""sortField"":""popular"",""isEnabledMode"":false,""isConsoleApprovedMode"":false,""isSubscriptionsMode"":false,""isConsoleForbiddenMode"":false,""sortIsAsc"":false}}},""modStateList"":[{""modId"":1107710,""modState"":true},{""modId"":723284,""modState"":true}]},""cfg_version"":1}";
    }

    public class JSONReader
    {
        // рекурсивная функция для обработки JSON-элементов и вывода на веб-страницу
        public static string ReadJsonForWeb(JToken token, string prefix = "")
        {
            if (token is JValue)
            {
                // если элемент является простым значением (не объектом или массивом),
                // выводим его значение в тег <input> для возможности редактирования
                JValue val = (JValue)token;
                string name = prefix;
                string type = "text";
                string addition = "";
                string tag = "";

                if (val.Type == JTokenType.String)
                {
                    return $"<textarea name='{name}' class=\"form-control textInp\">{val.Value}</textarea>";
                }
                else if (val.Type == JTokenType.Integer || val.Type == JTokenType.Float)
                {
                    type = "number";
                }
                else if (val.Type == JTokenType.Boolean)
                {
                    type = "checkbox";
                    if ((bool)val.Value)
                        addition = "checked";

                    //return $"<input type='{type}' class=\"btn-check\" id=\"ID_{name}\" {addition} name='{name}' value='{val.Value}' autocomplete=\"off\"/><label class=\"btn btn-outline-primary\" for=\"ID_{name}\">___</label>";
                }
                else if (val.Type == JTokenType.Date)
                {
                    type = "datetime-local";
                }
                return $"{tag}<input type='{type}' {addition} name='{name}' value='{val.Value}'/>{(tag!=""?tag.Insert(1, "/"):"")}";
            }
            else if (token is JArray)
            {
                // если элемент является массивом, создаем тег <details>,
                // в который будут добавляться все элементы массива
                JArray arr = (JArray)token;
                string name = prefix;
                StringBuilder sb = new StringBuilder();
                sb.Append($"<p><details><summary>Array [{arr.Count}]</summary>");
                sb.Append("<ul>");
                for (int i = 0; i < arr.Count; i++)
                {
                    sb.Append("<li>");
                    sb.Append(ReadJsonForWeb(arr[i], $"{name}[{i}]"));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
                sb.Append("</details></p>");
                return sb.ToString();
            }
            else if (token is JObject)
            {
                // если элемент является объектом, создаем тег <details>,
                // в который будут добавляться все свойства объекта
                JObject obj = (JObject)token;
                string name = prefix;
                StringBuilder sb = new StringBuilder();
                sb.Append($"<p><details><summary>Object [{obj.Count}]</summary>");
                sb.Append("<ul>");
                foreach (JProperty prop in obj.Properties())
                {
                    sb.Append("<li>");
                    sb.Append($"<div class=\"input-group mb-3\"><div class=\"input-group-prepend\"><span class=\"input-group-text\">{prop.Name}</span></div>");
                    sb.Append(ReadJsonForWeb(prop.Value, $"{name}.{prop.Name}"));
                    sb.Append("</div></li>");
                }
                sb.Append("</ul>");
                sb.Append("</details></p>");
                return sb.ToString();
            }
            else
            {
                // в случае неизвестного типа элемента, выбрасываем исключение
                throw new Exception("Unknown JSON element type");
            }
        }
    }
}
