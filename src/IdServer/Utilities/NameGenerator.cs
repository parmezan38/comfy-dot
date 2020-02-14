using IdServer.Data;
using IdServer.Utilities.Data;
using System;

namespace IdServer.Utilities
{
    public class NameGenerator
    {
        public static int NumberOfPossibleNames;
        static NameGenerator()
        {
            NumberOfPossibleNames = NameData.GetNumberOfPossibleResults();
        }

        public static string GenerateName()
        {
            string name = "";
            string[] randomPattern = NameData.GetRandomPattern();
            Array.ForEach(randomPattern, currentType => {
                string namePiece = currentType == "two" ? NameData.GetRandomPart("two")
                  : currentType == "three" ? NameData.GetRandomPart("three")
                    : currentType == "mid" ? NameData.GetRandomPart("mid")
                      : "_";
                name += namePiece;
            });
            return name;
        }

        public static string FilterForView(string name)
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

        public static string FilterForDb(string name)
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

        private static string DecapitalizaAndRemoveSpaces(string name)
        {
            string str = name.Replace(' ', '_');
            return str.ToLower();
        }
    }
}
