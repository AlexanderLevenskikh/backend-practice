using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase(@"'a\'a'", 0, @"a'a", 6)]
        [TestCase(@"'a\\'a'", 0, @"a\", 5)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var startIndexQuote = line[startIndex];
            var shieldingIsActive = false;
            var length = 1;
            var builder = new StringBuilder();

            if (startIndex < line.Length - 1)
            {
                for (int i = startIndex + 1; i < line.Length; i++)
                {
                    length++;
                    var currentChar = line[i];
                    
                    if (currentChar == startIndexQuote && !shieldingIsActive)
                    {
                        break;
                    }

                    shieldingIsActive = !shieldingIsActive && currentChar == '\\';

                    if (shieldingIsActive)
                    {
                        continue;
                    }
                    
                    builder.Append(currentChar);
                }
            }
            
            return new Token(builder.ToString(), startIndex, length);
        }
    }
}
