namespace ItemFormatter.Classes
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Edibility { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public Item(int id, string data)
        {
            Id = id;

            var dataArr = data.Split('/');
            Name = ReplaceInvalidChars(dataArr[0]);
            Price = int.Parse(dataArr[1]);
            Edibility = int.Parse(dataArr[2]);
            Type = dataArr[3];
            DisplayName = dataArr[4];
            Description = dataArr[5];
        }

        public static bool IsInvalid(string name, int id)
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
                .Replace("-", "");
        }
    }
}
