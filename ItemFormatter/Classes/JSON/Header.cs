namespace ItemFormatter.Classes.JSON
{
    public class Header
    {
        public string Target { get; set; }
        public int FormatVersion { get; set; }
        public bool Hidef { get; set; }
        public bool Compressed { get; set; }
    }
}
