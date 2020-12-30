using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemFormatter.Classes
{
    public class ItemInfo
    {
        private static readonly char[] InvalidCharacters = { '\'', ' ', ':', '.', '-', '(', ')' };

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int Edibility { get; private set; }
        public string Type { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }

        public ItemInfo(int id, string data)
        {
            Id = id;

            var dataArr = data.Split('/');
            Name = ReplaceInvalidChars(dataArr[0]);

            if (Name == "???")
            {
                Name = "Secret";
            }

            if (IsInvalid(Name, id))
            {
                return;
            }

            Price = int.Parse(dataArr[1]);
            Edibility = int.Parse(dataArr[2]);
            Type = dataArr[3];
            DisplayName = dataArr[4];
            Description = dataArr[5];
        }

        public void UniquifyName(List<ItemInfo> items)
        {
            int unique = 2;
            var tmpName = Name;
            while (items.Any(i => i.Name == Name))
            {
                Name = tmpName + unique;
                unique++;
            }

        }

        private bool IsInvalid(string name, int id)
        {
            return (name == "Weeds" && id != 316) || (name == "Stone" && id != 390);
        }

        public bool IsInvalid() => IsInvalid(Name, Id);

        private string ReplaceInvalidChars(string itemTxt)
        {
            return itemTxt
                .Replace("'", "")
                .Replace(" ", "")
                .Replace(":", "")
                .Replace(".", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
}
