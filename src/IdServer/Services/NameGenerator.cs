using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace IdServer.Services
{
    public class NameGenerator : INameGenerator
    {
        public int NumberOfPossibleNames { get; set; }
        private NameData data;
        public NameGenerator()
        {
            string json = File.ReadAllText("./JSON/NameData.json");
            data = JsonSerializer.Deserialize<NameData>(json);
            NumberOfPossibleNames = GetNumberOfPossibleResults();
        }

        public string GenerateName()
        {
            string name = "";
            List<string> randomPattern = GetRandomPattern();
            randomPattern.ForEach(currentType => {
                string namePiece = currentType == "Two" ? GetRandomPart("Two")
                  : currentType == "Three" ? GetRandomPart("Three")
                    : currentType == "Mid" ? GetRandomPart("Mid")
                      : "_";
                name += namePiece;
            });
            return name;
        }

        public string FilterForView(string name)
        {
            string str = name.Replace('_', ' ');
            char[] arr = str.ToCharArray();
            int length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                int nextI = i + 1;
                if (Char.IsLetter(arr[i]) && i == 0)
                {
                    arr[i] = char.ToUpper(arr[i]);
                }
                else if (!Char.IsLetter(arr[i]) && nextI < length - 1 && Char.IsLetter(arr[nextI]))
                {
                    arr[nextI] = char.ToUpper(arr[nextI]);
                }
            }
            return new string(arr);
        }

        public string FilterForDb(string name)
        {
            if (Char.IsLetter(name[0]) && Char.IsLetter(name[name.Length - 1]))
            {
                return DecapitalizaAndRemoveSpaces(name);
            }

            char[] arr = name.ToCharArray();
            arr = Array.FindAll<char>(arr, (it => (char.IsLetterOrDigit(it)
                                              || char.IsWhiteSpace(it))));
            name = new string(arr);
            string result = name.Trim(' ');
            return DecapitalizaAndRemoveSpaces(result);
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

        private string DecapitalizaAndRemoveSpaces(string name)
        {
            string str = name.Replace(' ', '_');
            return str.ToLower();
        }

        private List<string> GetRandomPattern()
        {
            int index = new Random().Next(0, data.Patterns.Count - 1);
            return data.Patterns[index];
        }

        private string GetRandomPart(string key)
        {
            List<string> part = data.Parts[key];
            int index = new Random().Next(0, part.Count - 1);
            return part[index];
        }
    }
}
