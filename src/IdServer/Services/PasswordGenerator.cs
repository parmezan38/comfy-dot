using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace IdServer.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private readonly Random getRandom = new Random();
        private PasswordData data;
        public PasswordGenerator()
        {
            string json = File.ReadAllText("./JSON/PasswordData.json");
            data = JsonSerializer.Deserialize<PasswordData>(json);
        }
        public string GeneratePassword()
        {
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
