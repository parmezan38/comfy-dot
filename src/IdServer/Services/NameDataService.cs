using System.IO;
using System.Text.Json;
using IdServer.Data.ServicesData;

namespace IdServer.Services
{
    public class NameDataService : INameDataService
    {

        public NameData GetNameData()
        {
            string json = File.ReadAllText("./Data/ServicesData/NameData.json");
            return JsonSerializer.Deserialize<NameData>(json);
        }
    }
}
