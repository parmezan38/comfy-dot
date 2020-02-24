using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IdServer.Data.ServicesData;

namespace IdServer.Services
{
    public class NameDataService : INameDataService
    {
        private readonly NameData data;

        public NameDataService()
        {
            string json = File.ReadAllText("./Data/ServicesData/NameData.json");
            data = JsonSerializer.Deserialize<NameData>(json);
        }

        public List<string> GetRandomPattern()
        {
            int index = new Random().Next(0, data.Patterns.Count);
            return data.Patterns[index];
        }

        public string GetRandomPart(string key)
        {
            List<string> part = data.Parts[key];
            int index = new Random().Next(0, part.Count);
            return part[index];
        }

        public int GetNumberOfPossibleResults()
        {
            int result = 0;
            data.Patterns.ForEach(pattern => {
                int patternReturnNum = 1;
                pattern.ForEach(patternPart => {
                    int length = string.IsNullOrEmpty(patternPart) ? 1 : data.Parts[patternPart].Count;
                    patternReturnNum *= length;
                });
                result += patternReturnNum;
            });
            return result;
        }
    }
}
