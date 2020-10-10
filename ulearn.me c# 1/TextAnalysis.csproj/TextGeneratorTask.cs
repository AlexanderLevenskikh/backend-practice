using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            var builder = new StringBuilder(phraseBeginning);

            for (var i = 0; i < wordsCount; i++)
            {
                var words = builder.ToString().Split(' ');
                var lastTwoWords = "";
                var lastWord = words[words.Length - 1];

                if (words.Length > 1)
                    lastTwoWords = words[words.Length - 2] + " " + lastWord;

                if (lastTwoWords != "" && nextWords.ContainsKey(lastTwoWords))
                {
                    builder.Append(" ");
                    builder.Append(nextWords[lastTwoWords]);
                }
                else if (nextWords.ContainsKey(lastWord))
                {
                    builder.Append(" ");
                    builder.Append(nextWords[lastWord]);
                }
                else
                    break;
            }

            return builder.ToString();
        }
    }
}