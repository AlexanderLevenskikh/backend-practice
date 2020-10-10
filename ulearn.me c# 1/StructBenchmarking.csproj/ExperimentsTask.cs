using System.Collections.Generic;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var size in Constants.FieldCounts)
            {
                classesTimes.Add(new ExperimentResult(size,
                    benchmark.MeasureDurationInMs(new ClassArrayCreationTask(size), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult(size,
                    benchmark.MeasureDurationInMs(new StructArrayCreationTask(size), repetitionsCount)));
            }

            return new ChartData
            {
                Title = "Create array",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();

            foreach (var size in Constants.FieldCounts)
            {
                classesTimes.Add(new ExperimentResult(size,
                    benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(size), repetitionsCount)));
                structuresTimes.Add(new ExperimentResult(size,
                    benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(size), repetitionsCount)));
            }

            return new ChartData
            {
                Title = "Call method with argument",
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }
    }
}