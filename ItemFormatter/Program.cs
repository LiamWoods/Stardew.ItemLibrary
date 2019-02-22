using ItemFormatter.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ItemFormatter.Classes.JSON;

namespace ItemFormatter
{
    class Program
    {
        const string steamObjectInfoPath = @"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Content\Data";
        const string xnbCliPath = @"C:\Users\Liam\Desktop\xnbcli-windows\xnbcli";
        const string inputFileName = "ObjectInformation.json";
        const string outputFile = @"C:\Users\Liam\source\repos\Stardew.ItemLibrary\ItemLibrary\Items.cs";

        static void Main(string[] args)
        {            
            var unpacker = new XnbUnpacker(steamObjectInfoPath, xnbCliPath);
            if (!unpacker.Unpack())
            {
                return;
            }

            var unpackedDir = Path.Combine(xnbCliPath, "unpacked");
            var objInfo = ObjectInformation.LoadFromFile(Path.Combine(unpackedDir, inputFileName));

            var trimChars = new char[] { ' ', '"' };

            var items = new List<Item>();
            foreach (var content in objInfo.Content)
            {
                var item = new Item(content.Key, content.Value);
                if (item.IsInvalid())
                {
                    continue;
                }

                int unique = 2;
                var tmpName = item.Name;
                while (items.Any(i => i.Name == item.Name))
                {
                    item.Name = tmpName + unique;
                    unique++;
                }

                items.Add(item);
            }

            var fileTxt = Formatter.FormatOutputText(items);
            File.WriteAllText(outputFile, fileTxt);
        }
    }
}
