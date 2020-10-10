using System;
using System.Linq;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var xLabels = Enumerable.Range(2, 30).Select(x => x.ToString()).ToArray();
            var yLabels = Enumerable.Range(1, 12).Select(x => x.ToString()).ToArray();

            var heat = new double[30, 12];

            for (var i = 0; i < 30; i++)
            {
                names
                    .Where(x => x.BirthDate.Day == i + 2)
                    .GroupBy(x => x.BirthDate.Month)
                    .ToList()
                    .ForEach(x => heat[i, x.Key - 1] = x.Count());
            }
            
            return new HeatmapData(
                "Тепловая карта",
                heat, 
                xLabels, 
                yLabels);
        }
    }
}