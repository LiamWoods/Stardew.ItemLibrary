using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ItemFormatter.Classes.JSON
{
    public class ObjectInformation
    {
        public Header Header { get; set; }
        public List<Reader> Readers { get; set; }
        public Dictionary<int, string> Content { get; set; }

        public static ObjectInformation LoadFromFile(string filePath)
        {
            return JsonConvert.DeserializeObject<ObjectInformation>(File.ReadAllText(filePath));
        }
    }
}
