using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var minutes = visits
                .GroupBy(v => v.UserId)
                .SelectMany(gr => gr
                    .OrderBy(x => x.DateTime)
                    .Bigrams()
                    .Where(x => x.Item1.SlideType == slideType &&
                                x.Item2.DateTime - x.Item1.DateTime >= TimeSpan.FromMinutes(1) &&
                                x.Item2.DateTime - x.Item1.DateTime <= TimeSpan.FromHours(2))
                    .Select(x => (x.Item2.DateTime - x.Item1.DateTime).TotalMinutes));

            if (!minutes.Any())
            {
                return 0;
            }

            return minutes.Median();
        }
    }
}