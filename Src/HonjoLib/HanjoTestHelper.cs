using System;
using System.Text.RegularExpressions;

namespace HonjoLib
{
    public class HanjoTestHelper
    {
        public static string Test(TestSetUp testSetUp, bool assertExpectations = true, bool trimResultBeforeAssertion = false,bool trimEveryResult = false)
        {
            Console.WriteLine("-===============INPUT============-");
            Console.WriteLine(testSetUp.Template);

            var totalTimeTaken = testSetUp.RunManyTimes(testSetUp.TotalNumberOfIterationa,
                () =>
                {
                    testSetUp.ActualResult = testSetUp.Honjo.Compile(testSetUp.Template, testSetUp.Model,
                        trimEveryResult);
                });

            if (trimResultBeforeAssertion)
            {
                Console.WriteLine("-===============RESULT HAS BEEN TRIMED============-");
                testSetUp.ActualResult = Regex.Replace(testSetUp.ActualResult.Trim(),@"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", string.Empty, RegexOptions.Compiled);
            }
            else
            {
                Console.WriteLine("-===============RESULT============-");
            }
            Console.WriteLine(testSetUp.ActualResult);

            if (assertExpectations)
            {
                Console.WriteLine("-===============TEST RESULT============-");
                if (testSetUp.ExpectedResult != testSetUp.ActualResult)
                {
                    throw new Exception("Expected " + testSetUp.ExpectedResult + " but got " + testSetUp.ActualResult);
                }
                if (testSetUp.MaxAllowedExecutionTime < totalTimeTaken)
                {
                    throw new Exception("process took " + totalTimeTaken + " which exceeds max duration allowed of " +
                                        totalTimeTaken);
                }
            }
            else
            {
                Console.WriteLine("-===============NO ASSERTION PERFORMED ON RESULT============-");
            }

            return testSetUp.ActualResult;
        }

    }
}