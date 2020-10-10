using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private Dictionary<string, Dictionary<int, List<int>>> index =
            new Dictionary<string, Dictionary<int, List<int>>>();

        public void Add(int id, string documentText)
        {
            var wordBeginIndex = 0;
            var currentIndex = 0;
            var delimiters = new List<char>(new[] {' ', '.', ',', '!', '?', ':', '-', '\r', '\n'});

            while (currentIndex <= documentText.Length)
            {
                if (currentIndex == documentText.Length || delimiters.Contains(documentText[currentIndex]))
                {
                    if (wordBeginIndex < currentIndex)
                    {
                        ProcessWord(
                            id,
                            documentText.Substring(wordBeginIndex, currentIndex - wordBeginIndex),
                            wordBeginIndex
                        );
                    }

                    currentIndex++;
                    wordBeginIndex = currentIndex;
                }
                else
                {
                    currentIndex++;
                }
            }
        }

        private void ProcessWord(int id, string word, int position)
        {
            if (!index.ContainsKey(word))
            {
                var dict = new Dictionary<int, List<int>>();
                dict.Add(id, new List<int>(new[] {position}));
                index.Add(word, dict);
            }
            else
            {
                if (!index[word].ContainsKey(id))
                {
                    index[word].Add(id, new List<int>(new[] {position}));
                }
                else
                {
                    var list = index[word][id];
                    if (!list.Contains(position))
                    {
                        list.Add(position);
                    }
                }
            }
        }

        public List<int> GetIds(string word)
        {
            return index.ContainsKey(word) ? index[word].Keys.ToList() : new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            return index.ContainsKey(word) && index[word].ContainsKey(id) ? index[word][id] : new List<int>();
        }

        public void Remove(int id)
        {
            foreach (var key in index.Keys.ToArray())
            {
                if (index[key].ContainsKey(id))
                {
                    index[key].Remove(id);
                }
            }
        }
    }
}