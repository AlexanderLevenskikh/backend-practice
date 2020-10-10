using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            return text
                .Split(new[] {".", "!", "?", ";", ":", "(", ")"},
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Regex.Split(x, @"[^a-z']+",
                        RegexOptions.IgnoreCase,
                        TimeSpan.FromMilliseconds(500))
                    .Select(w => w.ToLower())
                    .Where(w1 => !String.IsNullOrEmpty(w1))
                    .ToList()
                )
                .Where(l => l.Any())
                .ToList();
        }
    }
}