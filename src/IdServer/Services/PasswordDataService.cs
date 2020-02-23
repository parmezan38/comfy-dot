using System.IO;
using System.Text.Json;
using IdServer.Data.ServicesData;

namespace IdServer.Services
{
    public class PasswordDataService : IPasswordDataService
    {
        public PasswordData GetPasswordData()
        {
            string json = File.ReadAllText("./Data/ServicesData/PasswordData.json");
            return JsonSerializer.Deserialize<PasswordData>(json);
        }
    }
}
