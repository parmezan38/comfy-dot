using IdServer.Data;
using IdServer.Data.ServicesData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdServer.Services
{
    public class NameGeneratorService : INameGeneratorService
    {
        private int NumberOfPossibleNames { get; set; }
        private readonly ApplicationDbContext _db;
        private readonly INameDataService _nameDataService;

        public NameGeneratorService(INameDataService nameDataService, ApplicationDbContext db)
        {
            _db = db;
            _nameDataService = nameDataService;
            NumberOfPossibleNames = GetNumberOfPossibleResults();
        }

        public async Task<string> GetName()
        {
            bool exists = false;
            string name;
            int timeout = 0;

            do
            {
                name = GenerateName();
                var existingUser = await _db.Users.FirstOrDefaultAsync(e => e.UserName == name);

                if (existingUser != null)
                {
                    exists = true;
                    timeout++;
                } else
                {
                    exists = false;
                }
            } while (exists && timeout <= NumberOfPossibleNames);

            if (exists)
            {
                throw new Exception("Error trying to generate Username. Try again later");
            }

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

        private int GetNumberOfPossibleResults()
        {
            NameData data = _nameDataService.GetNameData();
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

        private string GenerateName()
        {
            NameData data = _nameDataService.GetNameData();
            string name = "";
            List<string> randomPattern = GetRandomPattern(data);
            randomPattern.ForEach(currentType => {
                string namePiece = currentType == "Two" ? GetRandomPart("Two", data)
                  : currentType == "Three" ? GetRandomPart("Three", data)
                    : currentType == "Mid" ? GetRandomPart("Mid", data)
                      : "_";
                name += namePiece;
            });
            return name;
        }

        private string DecapitalizaAndRemoveSpaces(string name)
        {
            string str = name.Replace(' ', '_');
            return str.ToLower();
        }

        private List<string> GetRandomPattern(NameData data)
        {
            int index = new Random().Next(0, data.Patterns.Count);
            return data.Patterns[index];
        }

        private string GetRandomPart(string key, NameData data)
        {
            List<string> part = data.Parts[key];
            int index = new Random().Next(0, part.Count);
            return part[index];
        }
    }
}
