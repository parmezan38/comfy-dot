using IdServer.Data.ServicesData;
using System;

namespace IdServer.Services
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private readonly Random getRandom = new Random();
        private readonly IPasswordDataService _passwordDataService;

        public PasswordGeneratorService(IPasswordDataService passwordDataService)
        {
            _passwordDataService = passwordDataService;
        }

        public string GeneratePassword()
        {
            PasswordData data = _passwordDataService.GetPasswordData();
            string password = "";
            data.Parts.ForEach(part => {
                double random = getRandom.NextDouble();
                double index = random * (part.Count - 1);
                int roundedIndex = (int)Math.Round(index);
                password += part[roundedIndex];
            });
            return password;
        }

    }
}
