using System.Collections.Generic;

namespace IdServer.Services
{
    public class NameData
    {
        public Dictionary<string, List<string>> Parts { get; set; }
        public List<List<string>> Patterns { get; set; }
    }
}
