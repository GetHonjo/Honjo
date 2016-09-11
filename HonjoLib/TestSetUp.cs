using System;
using System.Diagnostics;
using System.Linq;

namespace HonjoLib
{
    public class TestSetUp
    {
        public TestSetUp(string template  , object model, string expected, int totalNumberOfTimesToRun=1, TimeSpan? maxAllowedExecutionTime = null)
        {
            this.Honjo = new Honjo();
            this.Template = template;
            this.Model =model;
            this.ExpectedResult =  expected;
            this.MaxAllowedExecutionTime = maxAllowedExecutionTime?? TimeSpan.FromSeconds(1);
            this.ActualResult = string.Empty;
            this.TotalNumberOfIterationa = totalNumberOfTimesToRun;
        }
        public Honjo Honjo;
        public string Template;
        public object Model;
        public string ExpectedResult;
        public TimeSpan MaxAllowedExecutionTime;
        public string ActualResult;
        public int TotalNumberOfIterationa;
        public TimeSpan RunManyTimes(int iter, Action operation)
        {
            Stopwatch stopwatch = new Stopwatch();
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