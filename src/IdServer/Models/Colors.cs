namespace IdServer.Models
{
    public class Colors
    {
        public string first { get; set; }
        public string second { get; set; }
        public Colors(string first, string second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
