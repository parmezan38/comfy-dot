using IdServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdServer.Services
{
    public class NameGeneratorService : INameGeneratorService
    {
        private readonly ApplicationDbContext _db;
        private readonly INameDataService _nameDataService;

        public NameGeneratorService(INameDataService nameDataService, ApplicationDbContext db)
        {
            _db = db;
            _nameDataService = nameDataService;
        }

        public async Task<string> GetName()
        {
            int numberOfPossibleNames = _nameDataService.GetNumberOfPossibleResults();
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
            } while (exists && timeout <= numberOfPossibleNames);

            if (exists)
            {
                throw new Exception("Error trying to generate Username. Try again later");
            }

            return name;
        }

        public string FilterForView(string name)
        {
            return NameFilters.FilterForView(name);
        }

        public string FilterForDb(string name)
        {
            return NameFilters.FilterForDb(name);
        }

        private string GenerateName()
        {
            string name = "";
            List<string> randomPattern = _nameDataService.GetRandomPattern();
            randomPattern.ForEach(currentType => {
                string namePiece = currentType == "Two" ? _nameDataService.GetRandomPart("Two")
                  : currentType == "Three" ? _nameDataService.GetRandomPart("Three")
                    : currentType == "Mid" ? _nameDataService.GetRandomPart("Mid")
                      : "_";
                name += namePiece;
            });
            return name;
        }
    }
}
