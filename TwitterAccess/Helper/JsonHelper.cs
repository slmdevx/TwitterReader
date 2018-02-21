using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace TwitterAccess
{
    public static class JsonHelper
    {
        public static string SerializeObjectAsJsonString(object objectToSerialize)
        {
            string jsonString = JsonConvert.SerializeObject(objectToSerialize, Newtonsoft.Json.Formatting.Indented
                                    , new JsonSerializerSettings
                                            {
                                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                            }
                                    );
            return jsonString;
        }

        public static void SaveAsJsonToFile(object objectToSerialize, string filePath)
        {
            string jsonString = SerializeObjectAsJsonString(objectToSerialize);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                File.WriteAllText(filePath, jsonString);
            }
        }      

        public static T DeserializeToClass<T>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return default(T);

            T @class = JsonConvert.DeserializeObject<T>(jsonString);
            return @class;
        }

        public static T DeserializeFromFile<T>(string jsonfilePath)
        {
            string jsonString = File.ReadAllText(jsonfilePath);
            if (string.IsNullOrWhiteSpace(jsonString))
                return default(T);

            T @class = JsonConvert.DeserializeObject<T>(jsonString);
            return @class;
        }
    }
}
