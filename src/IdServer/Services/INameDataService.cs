using IdServer.Data.ServicesData;
using System.Collections.Generic;

namespace IdServer.Services
{
    public interface INameDataService
    {
        public List<string> GetRandomPattern();
        public string GetRandomPart(string key);
        public int GetNumberOfPossibleResults();
    }
}
