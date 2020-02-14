using System;
using System.Collections.Generic;

namespace IdServer.Utilities.Data
{
    public class NameData
    {
        private static Dictionary<string, string[]> Parts = new Dictionary<string, string[]>
        {
            {"two", new string[] {
                "aa", "ai", "ao", "au",
                "bb", "bo", "ba", "be", "bu", "bl",
                "co", "ci",
                "da", "de", "du",
                "ff", "fu", "fa", "fi", "fo",
                "go", "ga", "ge", "gi",
                "ho", "hu", "ha",
                "jo", "ju",
                "kk", "ko", "ki", "kt", "ku",
                "lo", "la", "li", "lu",
                "mm", "mo", "ma", "me", "mi", "mu",
                "no", "na", "ne",
                "ou", "on", "om",
                "po", "pa", "pu", "pe",
                "so", "su",
                "to", "ta", "ti", "tu",
                "vv", "vo", "va", "vu",
                "zz", "zo", "za", "zu", "zi"
            }},
            {"three", new string[] {
              "ann", "aki", "auk", "avo", "ave",
              "bob", "ban", "buo", "buu", "blu",
              "can", "coa", "cun", "cai", "cek",
              "dao", "dun", "dek", "dan",
              "fao", "fue", "fan", "foo",
              "gao", "gue", "gau", "gon", "gin",
              "hao", "hei", "hun", "hei", "hon",
              "jao", "jou", "jin", "jag",
              "kao", "kou", "kai", "kan", "keg", "kas",
              "lao", "lua", "lee", "lou", "lin", "lon", "laa",
              "mas", "mao", "mug", "mog", "mub", "meb",
              "nan", "nuu", "non", "noi", "nah",
              "ova", "oni", "ono", "oma",
              "pau", "pon", "poi",
              "roo", "rau", "roi",
              "sai", "soa", "sau", "son", "sei",
              "tau", "toi", "tan", "tao",
              "umu", "umi", "ujo", "uhu", "usi",
              "vau", "vam", "von", "vio",
              "zau", "zoi", "zen", "zak", "zam"
            }},
            {"mid", new string[] {
              "an", "aus", "auu",
              "bi", "ben", "buu", "bui",
              "cao", "cuo",
              "fon",
              "gen",
              "han",
              "iss",
              "jiu",
              "kon",
              "los", "luu",
              "mee", "mao",
              "non", "nai",
              "o",
              "pon", "pan",
              "ru", "ros", "rau",
              "so", "sao", "son",
              "to", "tan",
              "u",
              "van", "von",
              "zan", "zon", "zai"
            }}
        };

        private static string[][] Patterns = {
            new string[] {"two"},
            new string[] {"three"},
            new string[] {"mid"},
            new string[] {"two", "", "two"},
            new string[] {"two", "", "three"},
            new string[] {"two", "", "mid"},
            new string[] {"two", "two"},
            new string[] {"two", "three"},
            new string[] {"two", "two", "two"},
            new string[] {"two", "three", "two"},
            new string[] {"two", "", "mid", "", "two"},
            new string[] {"two", "", "mid", "", "three"},
            new string[] {"two", "two", "", "two"},
            new string[] {"two", "two", "", "three"},
            new string[] {"two", "two", "", "mid", "", "three"},
            new string[] {"three", "", "two"},
            new string[] {"three", "", "three"},
            new string[] {"three", "", "mid"},
            new string[] {"three", "two"},
            new string[] {"three", "three"},
            new string[] {"three", "two", "two"},
            new string[] {"three", "three", "two"},
            new string[] {"three", "", "mid", "", "two"},
            new string[] {"three", "", "mid", "", "three"},
            new string[] {"mid", "", "mid"}
        };

        public static string[] GetRandomPattern()
        {
            int index = new Random().Next(0, Patterns.Length - 1);
            return Patterns[index];
        }

        public static string GetRandomPart(string key)
        {
            string[] part = Parts[key];
            int index = new Random().Next(0, part.Length - 1);
            return part[index];
        }

        public static int GetNumberOfPossibleResults()
        {
            int result = 0;
            Array.ForEach(Patterns, pattern => {
                int patternReturnNum = 1;
                Array.ForEach(pattern, patternPart => {
                    int length = string.IsNullOrEmpty(patternPart) ? 1 : Parts[patternPart].Length;
                    patternReturnNum *= length;
                });
                result += patternReturnNum;
            });
            return result;
        }
    }
}
