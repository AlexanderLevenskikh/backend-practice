using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] {"text"})]
        [TestCase("hello world", new[] {"hello", "world"})]
        [TestCase("hello  world", new[] {"hello", "world"})]
        [TestCase("'hello'", new[] {"hello"})]
        [TestCase("\"hello\"", new[] {"hello"})]
        [TestCase("'hello world'", new[] {"hello world"})]
        [TestCase("'\"hello world\"'", new[] {"\"hello world\""})]
        [TestCase("\"'hello world'\"", new[] {"'hello world'"})]
        [TestCase("", new string[0])]
        [TestCase("'' \"bcd ef\" 'x y'", new[] {"", "bcd ef", "x y"})]
        [TestCase("a'hello'b", new[] {"a", "hello", "b"})]
        [TestCase("'hello", new[] {"hello"})]
        [TestCase("'hel\\'lo'", new[] {"hel'lo"})]
        [TestCase("\"hel\\\"lo\"", new[] {"hel\"lo"})]
        [TestCase("\"a \\\"c\\\"\"", new[] {"a \"c\""})]
        [TestCase("\"\\\\\"", new[] {"\\"})]
        [TestCase(" a ", new[] {"a"})]
        [TestCase("'a ", new[] {"a "})]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        // При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
        // Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
        public static List<Token> ParseLine(string line)
        {
            var index = 0;
            var tokens = new List<Token>();

            while (index < line.Length)
            {
                if (line[index] == ' ')
                {
                    index++;
                    continue;
                }

                var token = line[index] == '\'' || line[index] == '"'
                    ? ReadQuotedField(line, index)
                    : ReadField(line, index);
                
                tokens.Add(token);
                index += token.Length;
            }

            return tokens;
        }

        private static Token ReadField(string line, int startIndex)
        {
            var builder = new StringBuilder();

            for (int i = startIndex; i < line.Length; i++)
            {
                var currentChar = line[i];

                if (currentChar == ' ' || currentChar == '\'' || currentChar == '"')
                {
                    break;
                }

                builder.Append(currentChar);
            }

            return new Token(builder.ToString(), startIndex, builder.Length);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}