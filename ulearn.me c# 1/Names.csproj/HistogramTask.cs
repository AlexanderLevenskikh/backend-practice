using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var xLabels = Enumerable.Range(1, 31).Select(x => x.ToString()).ToArray();
            var yLabels = new double[31];
            
            names
                .Where(x => x.Name == name)
                .GroupBy(x => x.BirthDate.Day)
                .ToList()
                .ForEach(x =>
                {
                    if (x.Key != 1)
                    {
                        yLabels[x.Key - 1] = x.Count();
                    }
                });
            
            return new HistogramData(
                string.Format("Рождаемость людей с именем '{0}'", name),
                xLabels,
                yLabels);
        }
    }
}