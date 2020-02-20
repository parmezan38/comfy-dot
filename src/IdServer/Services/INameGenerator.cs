using IdServer.Utilities.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace IdServer.Services
{
    public interface INameGenerator
    {
        int NumberOfPossibleNames { get; set; }
        string GenerateName();
        string FilterForView(string name);
        string FilterForDb(string name);
    }
}
