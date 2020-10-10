using System.Collections.Generic;
using System;
using System.Text;


namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            Dictionary<string, Dictionary<string, int>> nGrams = FillNGrams(text, new[]{ 2, 3 });
            Dictionary<string, string> mostFrequentNGrams = new Dictionary<string, string>();
            foreach (var firstWord in nGrams)
            {
                var maxValue = 0;
                string mostFrequentSecondWord = null;

                foreach (var secondWord in firstWord.Value)
                {
                    if (secondWord.Value == maxValue)
                        if (string.CompareOrdinal(mostFrequentSecondWord, secondWord.Key) > 0)
                            mostFrequentSecondWord = secondWord.Key;
                    if (secondWord.Value > maxValue)
                    {
                        mostFrequentSecondWord = secondWord.Key;
                        maxValue = secondWord.Value;
                    }
                }

                mostFrequentNGrams.Add(firstWord.Key, mostFrequentSecondWord);
            }

            return mostFrequentNGrams;
        }

        public static Dictionary<string, Dictionary<string, int>> FillNGrams(
            List<List<string>> text,
            int[] sizes)
        {
            Dictionary<string, Dictionary<string, int>> nGrams = new Dictionary<string, Dictionary<string, int>>();

            foreach (var size in sizes)
            {
                foreach (var line in text)
                {
                    for (int i = 0; i < line.Count - size + 1; i++)
                    {
                        var keyBuilder = new StringBuilder();
                        for (int j = i; j < i + size - 1; j++)
                        {
                            keyBuilder.Append(line[j]);
                            if (j < i + size - 2)
                            {
                                keyBuilder.Append(" ");
                            }
                        }

                        var key = keyBuilder.ToString();
                    
                        if (!nGrams.ContainsKey(key))
                            nGrams.Add(key, new Dictionary<string, int>());
                        if (!nGrams[key].ContainsKey(line[i + size - 1]))
                            nGrams[key].Add(line[i + size - 1], 1);
                        else nGrams[key][line[i + size - 1]] += 1;
                    }
                }
            }

            return nGrams;
        }
    }
}