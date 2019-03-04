using System.Collections.Generic;
using System.Text;

namespace ItemFormatter.Classes
{
    class Formatter
    {
        public static string FormatOutputText(List<ItemInfo> itemList)
        {
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
    }
}
