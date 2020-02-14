using IdServer.Models;
using System;

namespace IdServer.Utilities
{
    public class ColorGenerator
    {
        private static Random random = new Random();
        private struct MinMax
        {
            public int min;
            public int max;
            public MinMax(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
        }
        static MinMax HUE = new MinMax(0, 360);
        static MinMax SAT = new MinMax(35, 90);
        static MinMax LIG = new MinMax(45, 80);

        public static Colors GenerateColors()
        {
            int primaryHue = GenerateValue(HUE);
            int secondaryHue = (primaryHue + 180 + random.Next(10, 20)) % 360;
            string saturation = Convert.ToString(GenerateValue(SAT), 10);
            string lightness = Convert.ToString(GenerateValue(LIG), 10);
            return new Colors(
              first: "h" + primaryHue + "s" + saturation + "l" + lightness,
              second: "h" + secondaryHue + "s" + saturation + "l" + lightness
            ); ;
        }

        public static string DeconstructColorCode(string str)
        {
            string[] firstSplit = str.Split('s');
            string[] secondSplit = firstSplit[1].Split('l');
            string h = firstSplit[0].Substring(1);
            string s = secondSplit[0];
            string l = secondSplit[1];
            string hsl = $"hsl({h},{s}%,{l}%)";
            return hsl;
        }

        private static int GenerateValue(MinMax val) => random.Next(val.min, val.max);
    }
}
