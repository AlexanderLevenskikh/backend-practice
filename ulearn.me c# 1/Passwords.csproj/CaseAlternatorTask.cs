using System;
using System.Collections.Generic;
using System.Linq;

namespace Passwords
{
    public class CaseAlternatorTask
    {
        //Тесты будут вызывать этот метод
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                result.Add(new string (word));
                return;
            }

            if (!char.IsLetter(word[startIndex]))
            {
                AlternateCharCases(word, startIndex + 1, result);
            }
            else
            {
                var ch = word[startIndex];
                var lowerCh = Char.ToLower(ch);
                var upperCh = Char.ToUpper(ch);

                if (lowerCh == upperCh)
                {
                    AlternateCharCases(word, startIndex + 1, result);
                }
                else
                {
                    word[startIndex] = lowerCh;
                    AlternateCharCases(word, startIndex + 1, result);
                    word[startIndex] = upperCh;
                    AlternateCharCases(word, startIndex + 1, result);
                }
            }
        }

        static char TransformCase(char ch) => Char.IsUpper(ch) ? Char.ToLower(ch) : Char.ToUpper(ch);
    }
}