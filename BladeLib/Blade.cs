using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BladeLib
{
    public class Blade
    {
        private IBladeExpressionEvaluator BladeExpressionEvaluator { set; get; }

        public Blade(IBladeExpressionEvaluator bladeExpressionEvaluator=null)
        {
            BladeExpressionEvaluator = bladeExpressionEvaluator?? new NewExpressionEvaluator();// ?? new BladeExpressionEvaluator();
        }
        public string Test(TestSetUp testSetUp, bool assertExpectations = true, bool trimResultBeforeAssertion=false)
        {
            Console.WriteLine("-===============INPUT============-");
            Console.WriteLine(testSetUp.Template);
           
            var totalTimeTaken = testSetUp.RunManyTimes(testSetUp.TotalNumberOfIterationa,
                () =>
                {
                    testSetUp.ActualResult = testSetUp.Blade.Compile(testSetUp.Template, testSetUp.Model);
                });
       
            if (trimResultBeforeAssertion)
            {
                Console.WriteLine("-===============RESULT HAS BEEN TRIMED============-");
                testSetUp.ActualResult = Regex.Replace(testSetUp.ActualResult.Trim(), @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", String.Empty, RegexOptions.Compiled);
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
                    throw new Exception("process took " + totalTimeTaken + " which exceeds max duration allowed of " + totalTimeTaken);
                }
            }
            else
            {
                Console.WriteLine("-===============NO ASSERTION PERFORMED ON RESULT============-");
            }
           
            return testSetUp.ActualResult;
        }


        public string Compile(string s, object o)
        {

            var dic = ToDictionary(o);

       
            Regex iterationRegex = new Regex(@"\{{item(.*?) in (.*?)}}(.*?)\{{/item}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var hasUpdate = false;
            do
            {

                hasUpdate = false;
               
                s = iterationRegex.Replace(s,
                    m =>
                    {
                        hasUpdate = true;
                        var codeString1 = "item"; // m.Groups[1].Value.Trim();


                        var codeString2 = m.Groups[2].Value.Trim();

                        var parts = codeString2.Replace(" at ", "*").Split('*');
                        codeString2 = parts[0] + ".Item";

                        var codeString3 = parts.Length > 1 ? parts[1] : null;


                        var codeString4 = m.Groups[3].Value.Trim();
                      //  var codeString4 = m.Groups[4].Value.Trim();

                        var newBuild = "";

                        if (dic.ContainsKey(codeString2))
                        {
                            var val = Enumerable.ToList<string>(dic[codeString2].Item1);
                            if (val != null)
                            {
                                var i = 0;
                                foreach (var o1 in val)
                                {

                                    var tmpDic = o1 is string
                                        ? new Dictionary<string, Tuple<dynamic, Type>>()
                                        {
                                            {"SomeScope", new Tuple<dynamic, Type>(o1, typeof (string))}
                                        }
                                        : ToDictionary(new
                                        {
                                            Scope = o1
                                        });

                                    foreach (var keyValuePair in tmpDic)
                                    {
                                        var scope = Guid.NewGuid().ToString().Replace("-", "");
                                        var itemRef = scope + "." + codeString1 + "_" + i;
                                        dic.Add(itemRef, new Tuple<dynamic, Type>(keyValuePair.Value.Item1, typeof(string)));
                                        newBuild += (codeString3 == null ? codeString4 : codeString4.Replace(codeString3, i.ToString())).Replace(codeString1, itemRef);
                                        i++;
                                    }
                                }
                                return newBuild;
                            }
                        }

                        return m.Value;
                    });
            } while (hasUpdate);


            Regex VarRegex = new Regex(@"\{{var (.*?)=(.*?)\}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

           

                s = VarRegex.Replace(s,
                    m =>
                    {
                       
                        var symb = m.Groups[1].Value.Trim();
                        var value = m.Groups[2].Value.Trim();

                        
                            int j;
                            Tuple<dynamic, Type> val;
                        if (int.TryParse(value, out j))
                        {
                            val = new Tuple<dynamic, Type>(j, typeof (int));
                        }

                        else
                        {
                            if (dic.ContainsKey(value))
                            {
                                val = dic[value];
                            }
                            else
                            {
                                 val = new Tuple<dynamic, Type>(value, typeof(string));
                            }
                        }

                        if (!dic.ContainsKey(symb))
                        {
                            dic.Add(symb, val);
                        }
                        else
                        {
                            dic[symb] = val;
                        }

                        return "";
                    });
           



            foreach (var sign in new List<string>() { "==", "!=", ">", "<", ">=", "<=" ,"&&"})
            {
                Regex matchIfElseRegex = new Regex(@"\{{if(.*?)" + sign + @"(.*?)}}(.*?){{else}}(.*?)\{{/if}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Regex matchIfRegex = new Regex(@"\{{if(.*?)" + sign + @"(.*?)}}(.*?)\{{/if}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);



                s = matchIfElseRegex.Replace(s,
                 m =>
                 {
                     var codeString1 = dic.ContainsKey(m.Groups[1].Value.Trim()) ? dic[m.Groups[1].Value.Trim()].Item1 : m.Groups[1].Value.Trim();
                     var codeString2 = dic.ContainsKey(m.Groups[2].Value.Trim()) ? dic[m.Groups[2].Value.Trim()].Item1 : m.Groups[2].Value.Trim();
                     var codeString3 = m.Groups[3].Value;
                     var codeString4 = m.Groups[4].Value;

                     return "{{  (" + codeString1 + sign + codeString2 + ")?" +
                    " \"" + codeString3.Replace("\"", "\\\"") + "\":" + " \"" + codeString4.Replace("\"", "\\\"") + "\""
                     + " }}";
                 });
                s = matchIfRegex.Replace(s,
                m =>
                {
                    var codeString1 = dic.ContainsKey(m.Groups[1].Value.Trim()) ? dic[m.Groups[1].Value.Trim()].Item1 : m.Groups[1].Value.Trim();
                    var codeString2 = dic.ContainsKey(m.Groups[2].Value.Trim()) ? dic[m.Groups[2].Value.Trim()].Item1 : m.Groups[2].Value.Trim();
                    var codeString3 = m.Groups[3].Value;
                    var codeString4 = "";

                    return "{{  (" + codeString1 + sign + codeString2 + ")?" +
                   " \"" + codeString3.Replace("\"", "\\\"") + "\":" + " \"" + codeString4.Replace("\"", "\\\"") + "\""
                    + " }}";
                });
            }

            Regex interpolRegex = new Regex(@"\{{(.*?)\}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


            var result = interpolRegex.Replace(s,
                m =>
                {
                    var tmpResult = "";


                    var codeString = m.Groups[1].Value.Trim();
                    tmpResult = codeString;

                    if (dic.ContainsKey(codeString))
                    {
                        tmpResult = dic[codeString].Item1.ToString();
                    }
                    else
                    {
                        foreach (var c in new List<char>() { '+', '-', '/', '|', '*' })
                        {
                            foreach (var s1 in tmpResult.Split(c).ToList().OrderByDescending(ss => ss.Split('.').Length))
                            {
                                if (dic.ContainsKey(s1.ToString().Trim()))
                                {
                                    var avail = dic[s1.ToString().Trim()];
                                    var replacement = avail.Item1.ToString();
                                    var rep = avail.Item2 == typeof(string) ? "'" + replacement + "'" : replacement;
                                    codeString = codeString.Replace(s1.Trim(), rep);
                                }
                            }

                        }


                        try
                        {
                            tmpResult = BladeExpressionEvaluator. Evaluate(codeString);
                        }
                        catch (Exception te)
                        {
                            Console.WriteLine(te.Message+" "+te.InnerException?.Message+" "+te);
                            tmpResult = "";// "[ERROR COMPILLING TEMPLATE]";
                        }
                    }


                    return tmpResult;
                });

            return result;
        }

        
        public Dictionary<string, Tuple<dynamic, Type>> ToDictionary<T>(T myVar, string root = "")
        {
            if (myVar == null)
            {
                return new Dictionary<string, Tuple<dynamic, Type>>();
            }

            var dic = new Dictionary<string, Tuple<dynamic, Type>>();

            foreach (var prop in myVar.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {


                if (
                      prop.PropertyType == typeof(bool))
                {

                    var data = prop.GetValue(myVar, null);
                    dic.Add(root + prop.Name, new Tuple<dynamic, Type>(data.ToString().ToLower(), prop.PropertyType));

                }
                else
                 if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && myVar.GetType().IsNonStringEnumerable())
                {
                    dic.Add(root + prop.Name, new Tuple<dynamic, Type>(myVar, prop.PropertyType));
                }
                else
                 if (prop.PropertyType == typeof(string)
                     || prop.PropertyType == typeof(int))
                {
                    var data = prop.GetValue(myVar, null);
                    dic.Add(root + prop.Name, new Tuple<dynamic, Type>(data, prop.PropertyType));
                }
                else
                 if (prop.PropertyType == typeof(char))
                {

                    dic.Add(root + prop.Name, new Tuple<dynamic, Type>(myVar, prop.PropertyType));
                }
                else
                {
                    var result = ToDictionary(prop.GetValue(myVar, null), root + prop.Name + ".");
                    if (result == null) continue;
                    foreach (var keyValuePair in result)
                    {
                        dic.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            return dic;
        }
    }
}