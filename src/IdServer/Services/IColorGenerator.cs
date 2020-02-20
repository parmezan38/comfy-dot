using IdServer.Models;
using System;

namespace IdServer.Services
{
    public interface IColorGenerator
    {
        Colors GenerateColors();
        string DeconstructColorCode(string str);
    }
}
