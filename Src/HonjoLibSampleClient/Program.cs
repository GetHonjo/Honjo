using System;
using HonjoLib;

namespace HonjoLibSampleClient
{
    public class MyClass
    {
        public static int Tripple(int i)
        {
            return i + 1000;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            //https://www.nuget.org/packages/Honjo/0.0.4-pre

            var result = new Honjo(typeof(MyClass)).Compile("{{var x=200}}{{MyClass.Tripple(x+Amount)}}", "{\"Amount\":100}");

            Console.WriteLine(result);

            result = new Honjo().Compile("{{var x=200}}{{x+Amount}}", new {Amount = 100});
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}