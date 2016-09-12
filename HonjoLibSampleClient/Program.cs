using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonjoLibSampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://www.nuget.org/packages/Honjo/0.0.4-pre

            var result = new HonjoLib.Honjo().Compile("{{var x=200}}{{x+Amount}}", "{\"Amount\":100}");

            Console.WriteLine(result);

            result = new HonjoLib.Honjo().Compile("{{var x=200}}{{x+Amount}}", new { Amount = 100 });
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
