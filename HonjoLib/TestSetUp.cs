using System;
using System.Diagnostics;
using System.Linq;

namespace HonjoLib
{
    public class TestSetUp
    {
        public string ActualResult;
        public string ExpectedResult;
        public Honjo Honjo;
        public TimeSpan MaxAllowedExecutionTime;
        public object Model;
        public string Template;
        public int TotalNumberOfIterationa;

        public TestSetUp(string template, object model, string expected, int totalNumberOfTimesToRun = 1,
            TimeSpan? maxAllowedExecutionTime = null)
        {
            Honjo = new Honjo();
            Template = template;
            Model = model;
            ExpectedResult = expected;
            MaxAllowedExecutionTime = maxAllowedExecutionTime ?? TimeSpan.FromSeconds(1);
            ActualResult = string.Empty;
            TotalNumberOfIterationa = totalNumberOfTimesToRun;
        }

        public TimeSpan RunManyTimes(int iter, Action operation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var i in Enumerable.Range(0, iter))
            {
                operation?.Invoke();
            }
            stopwatch.Stop();

            // Write result.
            Console.WriteLine("Time elapsed: {0} for {1} Iterations", stopwatch.Elapsed, iter);
            return stopwatch.Elapsed;
        }
    }
}