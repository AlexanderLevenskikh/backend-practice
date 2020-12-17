using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    public interface IBuilder
    {
        string MakeCaption(string caption);
        string BeginList();
        string MakeItem(string valueType, string entry);
        string EndList();
    }
    
    public static class BuilderExtensions
    {
        public static string MakeReport(this IBuilder builder,
            Func<IEnumerable<double>, object> statisticsCalculator,
            string caption,
            IEnumerable<Measurement> measurements)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append(builder.MakeCaption(caption));
            result.Append(builder.BeginList());
            result.Append(builder.MakeItem("Temperature", statisticsCalculator(data.Select(z => z.Temperature)).ToString()));
            result.Append(builder.MakeItem("Humidity", statisticsCalculator(data.Select(z => z.Humidity)).ToString()));
            result.Append(builder.EndList());
            return result.ToString();
        }
    }

    public class HtmlBuilder : IBuilder
    {
        public string MakeCaption(string caption)
        {
            return $"<h1>{caption}</h1>";
        }

        public string BeginList()
        {
            return "<ul>";
        }

        public string EndList()
        {
            return "</ul>";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $"<li><b>{valueType}</b>: {entry}";
        }
    }
    
    public class MarkdownBuilder : IBuilder
    {
        public string MakeCaption(string caption)
        {
            return $"## {caption}\n\n";
        }

        public string BeginList()
        {
            return "";
        }

        public string EndList()
        {
            return "";
        }

        public string MakeItem(string valueType, string entry)
        {
            return $" * **{valueType}**: {entry}\n\n";
        }
    }

    public static class ReportMakerHelper
    {
        private static IBuilder htmlBuilder = new HtmlBuilder();
        private static IBuilder markdownBuilder = new MarkdownBuilder();
     
        private static Func<IEnumerable<double>, MeanAndStd> MeanAndStdCalc = data =>
        {
            var mean = data.Average();
            return new MeanAndStd
            {
                Mean = mean,
                Std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count() - 1)),
            };
        };

        private static Func<IEnumerable<double>, object> Median = data =>
        {
            var list = data.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

            return list[list.Count / 2];
        };

        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        {
            return htmlBuilder.MakeReport(MeanAndStdCalc, "Mean and Std", data);
        }

        public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        {
            return markdownBuilder.MakeReport(Median, "Median", data);
        }

        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data)
        {
            return markdownBuilder.MakeReport(MeanAndStdCalc, "Mean and Std", data);
        }

        public static string MedianHtmlReport(IEnumerable<Measurement> data)
        {
            return htmlBuilder.MakeReport(Median, "Median", data);
        }
    }
}