using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var comparisonResult = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count; i++)
            for (var j = i + 1; j < documents.Count; j++)
                comparisonResult.Add(new ComparisonResult(
                    documents[i],
                    documents[j],
                    ComputeLevenshteinDistance(documents[i], documents[j])
                ));

            return comparisonResult;
        }

        public double ComputeLevenshteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var prevOpt = new double[second.Count + 1];
            var currentOpt = new double[second.Count + 1];

            currentOpt[0] = 1;
            for (var i = 0; i <= second.Count; ++i) prevOpt[i] = i;

            for (var i = 1; i <= first.Count; ++i)
            {
                for (var j = 1; j <= second.Count; ++j)
                {
                    if (first[i - 1] == second[j - 1])
                        currentOpt[j] = prevOpt[j - 1];
                    else
                        currentOpt[j] = Math.Min(
                            1 + prevOpt[j],
                            TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]) +
                            Math.Min(prevOpt[j - 1], currentOpt[j - 1])
                        );
                }

                currentOpt.CopyTo(prevOpt, 0);
                currentOpt[0] = i + 1;
            }

            return currentOpt[second.Count];
        }
    }
}