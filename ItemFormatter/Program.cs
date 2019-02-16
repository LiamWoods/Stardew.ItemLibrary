using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ItemFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"C:\Users\Liam\Desktop";
            var inputFileName = "ObjectInformation.json";
            var outputFileName = "stardewitemsformatted.txt";
            var objInfo = GetObjectInformation(Path.Combine(basePath, inputFileName));
            var trimChars = new char[] { ' ', '"' };

            var items = new List<Item>();
            foreach (var content in objInfo.Content)
            {
                var item = new Item(content.Key, content.Value);

                if (IsInvalidItem(item.Name, item.Id))
                {
                    continue;
                }

                int unique = 2;
                var tmpName = item.Name;
                while(items.Any(i => i.Name == item.Name))
                {
                    item.Name = tmpName + unique;
                    unique++;
                }
                                
                items.Add(item);
            }
                                   
            var fileTxt = FormatOutputText(items);
            File.WriteAllText(Path.Combine(basePath, outputFileName), fileTxt);
        }
                
        static string FormatOutputText(List<Item> itemList)
        {
            var n = Environment.NewLine;
            
            var sb = new StringBuilder();
            sb.AppendLine("namespace Stardew.ItemLibary");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static class Items");
            sb.AppendLine("\t{");
            foreach (var item in itemList)
            {
                sb.Append("\t\t");
                sb.AppendLine($"public const int {item.Name} = {item.Id};");
            }
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            return sb.ToString();
        }

        static ObjectInformation GetObjectInformation(string filePath)
        {
            return JsonConvert.DeserializeObject<ObjectInformation>(File.ReadAllText(filePath));
        }

        static bool IsInvalidItem(string name, int id)
        {
            return (name == "Weeds" && id != 316) || (name == "Stone" && id != 390);
        }
    }

    public class ObjectInformation
    {
        public Header Header { get; set; }
        public List<Reader> Readers { get; set; }
        public Dictionary<int, string> Content { get; set; }
    }

    public class Header
    {
        public string Target { get; set; }
        public int FormatVersion { get; set; }
        public bool Hidef { get; set; }
        public bool Compressed { get; set; }
    }

    public class Reader
    {
        public string Type { get; set; }
        public int Version { get; set; }
    }

    public class Item
    {
        public Item(int id, string data)
        {
            Id = id;

            var dataArr = data.Split('/');
            Name = dataArr[0].ReplaceInvalidChars();
            Price = int.Parse(dataArr[1]);
            Edibility = int.Parse(dataArr[2]);
            Type = dataArr[3];
            DisplayName = dataArr[4];
            Description = dataArr[5];
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Edibility { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
    
    public static class Extensions
    {
        public static string ReplaceInvalidChars(this string itemTxt)
        {
            return itemTxt
                .Replace("'", "")
                .Replace(" ", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("-", "");
        }
    }
}
