using ItemFormatter.Classes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemFormatter.Classes.JSON;
using System.Reflection;

namespace ItemFormatter
{
    class Program
    {
        const string _steamObjectInfoPath = @"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Content\Data";
        const string _xnbCliPath = @"C:\Users\Liam\Desktop\xnbcli-windows\xnbcli";
        const string _inputFileName = "ObjectInformation.json";
        const string _outputFilePath = @"ItemLibrary\Items.cs";
        const string _assemblyName = "Stardew.ItemLibrary";

        static void Main(string[] args)
        {
            var unpacker = new XnbUnpacker(_steamObjectInfoPath, _xnbCliPath);
            if (!unpacker.Unpack())
            {
                return;
            }

            var unpackedDir = Path.Combine(_xnbCliPath, "unpacked");
            var objInfo = ObjectInformation.LoadFromFile(Path.Combine(unpackedDir, _inputFileName));
                        
            var items = new List<ItemInfo>();
            foreach (var content in objInfo.Content)
            {
                var item = new ItemInfo(content.Key, content.Value);
                if (item.IsInvalid())
                {
                    continue;
                }

                item.UniquifyName(items);

                items.Add(item);
            }

            var fileTxt = Formatter.FormatOutputText(items);

            var assemblyLocation = Assembly.GetExecutingAssembly().Location;

            var outputFilePath = Path.Combine(assemblyLocation.Substring(0, assemblyLocation.IndexOf(_assemblyName)), _assemblyName, _outputFilePath);

            File.WriteAllText(outputFilePath, fileTxt);
        }
    }
}
