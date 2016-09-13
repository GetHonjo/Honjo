using System;
using HonjoLib;

namespace HonjoLibSampleClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //https://www.nuget.org/packages/Honjo/0.0.4-pre

            var result = new Honjo().Compile("{{var x=200}}{{x+Amount}}", "{\"Amount\":100}");

            Console.WriteLine(result);

            result = new Honjo().Compile("{{var x=200}}{{x+Amount}}", new {Amount = 100});
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}