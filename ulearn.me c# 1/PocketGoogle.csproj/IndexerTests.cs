using NUnit.Framework;

namespace PocketGoogle
{
    [TestFixture]
    public class IndexerTests
    {
        [TestCase("A B C", "A C", "C", "C", new []{ 0, 1, 2 })]
        [TestCase("A B C", "A C", " CC ", "C", new []{ 0, 1 })]
        public void GetIdsTest(string text1, string text2, string text3, string word, int[] expected)
        {
            var indexer = new Indexer();
            indexer.Add(0, text1);
            indexer.Add(1, text2);
            indexer.Add(2, text3);

            var indexerWords = indexer.GetIds(word);
            Assert.AreEqual(indexerWords.Count, expected.Length);
            for (var i = 0; i < indexerWords.Count; i++)
            {
                Assert.AreEqual(indexerWords[i], expected[i]);
            }
        }

        [TestCase(0, new []{ 4 })]
        [TestCase(1, new []{ 2 })]
        [TestCase(2, new []{ 0 })]
        [TestCase(3, new int[0])]
        [TestCase(4, new []{ 0, 2, 4 })]
        public void GetPositionsTest(int id, int[] expected)
        {
            var indexer = new Indexer();
            var word = "C";
            indexer.Add(0, "A B C");
            indexer.Add(1, "A C B");
            indexer.Add(2, "C A");
            indexer.Add(3, "A B D");
            indexer.Add(4, "C C C");

            var positions = indexer.GetPositions(id, word);
            Assert.AreEqual(positions.Count, expected.Length);
            for (var i = 0; i < positions.Count; i++)
            {
                Assert.AreEqual(positions[i], expected[i]);
            }
        }
    }
}