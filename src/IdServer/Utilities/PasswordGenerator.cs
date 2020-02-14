using IdServer.Data;
using IdServer.Utilities.Data;
using System;

namespace IdServer.Utilities
{
    public class PasswordGenerator
    {
        private static readonly Random getRandom = new Random();
        public static string GeneratePassword()
        {
            string password = "";
            string[][] passwordParts = PasswordData.Parts;
            Array.ForEach(passwordParts, part => {
                double random = getRandom.NextDouble();
                double index = random * (part.Length - 1);
                int roundedIndex = (int)Math.Round(index);
                password += part[roundedIndex];
            });
            return password;
        }

    }
}
