using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect(); // Эти две строчки нужны, чтобы уменьшить вероятность того,
            GC.WaitForPendingFinalizers(); // что Garbadge Collector вызовется в середине измерений

            task.Run();

            var stopwatch = Stopwatch.StartNew();
            for (var i = 0; i < repetitionCount; i++)
            {
                task.Run();
            }
            stopwatch.Stop();
            
            return (double)stopwatch.ElapsedMilliseconds / repetitionCount;
        }
    }

    public class StringBuilderTask : ITask
    {
        public void Run()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 10000; i++)
                builder.Append('a');

            var x = builder.ToString();
        }
        
        static T Max<T>(T[] source) where T : IComparable
        {
            if(source.Length == 0)
                return default(...);
    
            return source.Max();
        }

    }

    public class StringConstructorTask : ITask
    {
        public void Run()
        {
            var x = new string('a', 10000);
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var stringBuilderBenchmark = new Benchmark();
            var stringBuilderTask = new StringBuilderTask();
            var elapsedMsForStringBuilder = stringBuilderBenchmark.MeasureDurationInMs(
                stringBuilderTask, 10000);
            
            var stringConstructorBenchmark = new Benchmark();
            var stringConstructorTask = new StringConstructorTask();
            var elapsedMsForStringConstructor = stringConstructorBenchmark.MeasureDurationInMs(
                stringConstructorTask, 10000);
            
            Assert.Less(elapsedMsForStringConstructor, elapsedMsForStringBuilder);
        }
    }
}