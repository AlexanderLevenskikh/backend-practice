using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{
    public class Profiler : IDisposable
    {
        public long ProfilerValue { get; private set; }
        public string Name { get; }
        public int Level { get; }

        public List<Profiler> ChildProfilers { get; }
        public Action<Profiler> PerformAction { get; }
        private Stopwatch sw;

        public Profiler(Action<Profiler> performAction, string name = "*", int level = 0)
        {
            PerformAction = performAction;
            Level = level;
            Name = name;
            ChildProfilers = new List<Profiler>();
            sw = new Stopwatch();
            sw.Start();
        }

        ~Profiler()
        {
            Dispose();
        }

        public Profiler StartChildTimer(string name = "*")
        {
            var childProfiler = new Profiler(PerformAction, name, Level + 1);
            ChildProfilers.Add(childProfiler);

            return childProfiler;
        }

        public void Dispose()
        {
            sw.Stop();
            ProfilerValue = sw.ElapsedMilliseconds;

            if (Level == 0)
            {
                PerformAction(this);
            }
        }
    }
    
    public class Timer
    {
        // Use this method in your solution to fit report formatting requirements from the tests
        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }

        public static Profiler Start(StringWriter writer, string name = "*")
        {
            Action<Profiler> performAction = profiler =>
            {
                writer.WriteAsync(FormatReportLine(profiler.Name, profiler.Level, profiler.ProfilerValue));

                long childTime = 0;
                foreach (var childProfiler in profiler.ChildProfilers)
                {
                    childProfiler.PerformAction(childProfiler);
                    childTime += childProfiler.ProfilerValue;
                }

                if (profiler.ChildProfilers.Count > 0)
                {
                    writer.WriteAsync(FormatReportLine("Rest", profiler.Level + 1, profiler.ProfilerValue - childTime));
                }
            };  
            return new Profiler(performAction, name);
        }
    }
}
