using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Helpers;


namespace HonjoLib
{
    internal static class TypeToDictionaryExtention
    {

        internal static Dictionary<string, Tuple<dynamic, Type>> ToDictionary<T>( this  T o, string root = "")
        {
            if (o == null)
            {
                return new Dictionary<string, Tuple<dynamic, Type>>();
            }

            var dic = new Dictionary<string, Tuple<dynamic, Type>>();

            var properties = o.GetType() .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.Default);

            object myVar = o;
            if (o is string && properties.Any())
            {
                myVar = Json.Decode<dynamic>(o.ToString());

                var tttt = myVar as Dictionary<string, object>;


                foreach (var prop in tttt)
                {
                    dic.Add(prop.Key, new Tuple<dynamic, Type>(prop.Value, typeof (object)));
                }
            }
            else
            {
                foreach (var prop in properties)
                {
                    if (
                        prop.PropertyType == typeof(bool))
                    {
                        var data = prop.GetValue(myVar, null);
                        dic.Add(root + prop.Name, new Tuple<dynamic, Type>(data.ToString().ToLower(), prop.PropertyType));
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                             myVar.GetType().IsNonStringEnumerable())
                    {
                        dic.Add(root + prop.Name, new Tuple<dynamic, Type>(myVar, prop.PropertyType));
                    }
                    else if (prop.PropertyType == typeof(string)
                             || prop.PropertyType == typeof(int))
                    {
                        var data = prop.GetValue(myVar, null);
                        dic.Add(root + prop.Name, new Tuple<dynamic, Type>(data, prop.PropertyType));
                    }
                    else if (prop.PropertyType == typeof(char))
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
            }
            return dic;
        }

    }


    public class MatchEvaluators
    {
        internal static Regex IterationRegex = new Regex(@"\{{item(.*?) in (.*?)}}(.*?)\{{/item}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        internal static Regex VariableCreationRegex = new Regex(@"\{{var (.*?)=(.*?)\}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        internal static Regex MatchIfElseRegex = new Regex(@"\{{if (.*?)(==|>|<|>=|<=|!=|&&)(.*?)}}(.*?){{else}}(.*?)\{{/if}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        internal static Regex MatchIfRegex = new Regex(@"\{{if (.*?)(==|>|<|>=|<=|!=|&&)(.*?)}}(.*?)\{{/if}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        internal static Regex MatchNestedIfElseRegex = new Regex(@"\{{if (.*?)(==|>|<|>=|<=|!=|&&)(.*?)}}(.*?){{else}}(.*?)\{{/if}}(.*?){{/if}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        internal static Regex InterpolRegex = new Regex(@"\{{(.*?)\}}", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


        public MatchEvaluators(Dictionary<string, Tuple<dynamic, Type>> dic, object o, bool trimEveryResult, IBladeExpressionEvaluator bladeExpressionEvaluator)
        {
            IterationRegexEvaluator = m =>
            {

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
                                ? new Dictionary<string, Tuple<dynamic, Type>>
                                {
                                    {"SomeScope", new Tuple<dynamic, Type>(o1, typeof (string))}
                                }
                                : TypeToDictionaryExtention. ToDictionary(new
                                {
                                    Scope = o1
                                });

                            foreach (var keyValuePair in tmpDic)
                            {
                                var scope = Guid.NewGuid().ToString().Replace("-", "");
                                var itemRef = scope + "." + codeString1 + "_" + i;
                                dic.Add(itemRef,
                                    new Tuple<dynamic, Type>(keyValuePair.Value.Item1, typeof(string)));
                                newBuild +=
                                    (codeString3 == null
                                        ? codeString4
                                        : codeString4.Replace(codeString3, i.ToString())).Replace(codeString1,
                                            itemRef);
                                i++;
                            }
                        }
                        if (trimEveryResult)
                        {
                            newBuild = Regex.Replace(newBuild.Trim(),
                                @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", string.Empty,
                                RegexOptions.Compiled);
                        }
                        return newBuild;
                    }
                }

                return m.Value;
            };
            VariableCreationRegexEvaluator = m =>
            {
                var symb = m.Groups[1].Value.Trim();
                var value = m.Groups[2].Value.Trim();


                int j;
                Tuple<dynamic, Type> val;
                if (int.TryParse(value, out j))
                {
                    val = new Tuple<dynamic, Type>(j, typeof(int));
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
            };
            MatchNestedIfElseRegexEvaluator = m =>
            {
                var codeString1 = dic.ContainsKey(m.Groups[1].Value.Trim())
                    ? dic[m.Groups[1].Value.Trim()].Item1
                    : m.Groups[1].Value.Trim();
                var sign = m.Groups[2].Value;
                var codeString2 = dic.ContainsKey(m.Groups[3].Value.Trim())
                    ? dic[m.Groups[3].Value.Trim()].Item1
                    : m.Groups[3].Value.Trim();
                var codeString3 = m.Groups[4].Value;
                var nest = m.Groups[5].Value + "{{/if}}" + m.Groups[6].Value;
                var codeString4 = new Honjo().Compile(nest, o, trimEveryResult);


                return "{{  (" + codeString1 + sign + codeString2 + ")?" +
                       " \"" + codeString3.Replace("\"", "\\\"") + "\":" + " \"" + codeString4.Replace("\"", "\\\"") +
                       "\""
                       + " }}";
            };
            MatchIfElseRegexEvaluator = m =>
            {
                var codeString1 = dic.ContainsKey(m.Groups[1].Value.Trim())
                    ? dic[m.Groups[1].Value.Trim()].Item1
                    : m.Groups[1].Value.Trim();
                var sign = m.Groups[2].Value;
                var codeString2 = dic.ContainsKey(m.Groups[3].Value.Trim())
                    ? dic[m.Groups[3].Value.Trim()].Item1
                    : m.Groups[3].Value.Trim();
                var codeString3 = m.Groups[4].Value;
                var codeString4 = m.Groups[5].Value;

                return "{{  (" + codeString1 + sign + codeString2 + ")?" +
                       " \"" + codeString3.Replace("\"", "\\\"") + "\":" + " \"" + codeString4.Replace("\"", "\\\"") +
                       "\""
                       + " }}";
            };
            MatchIfRegexEvaluator = m =>
            {
                var codeString1 = dic.ContainsKey(m.Groups[1].Value.Trim())
                    ? dic[m.Groups[1].Value.Trim()].Item1
                    : m.Groups[1].Value.Trim();
                var sign = m.Groups[2].Value;
                var codeString2 = dic.ContainsKey(m.Groups[3].Value.Trim())
                    ? dic[m.Groups[3].Value.Trim()].Item1
                    : m.Groups[3].Value.Trim();
                var codeString3 = m.Groups[4].Value;
                var codeString4 = "";

                return "{{  (" + codeString1 + sign + codeString2 + ")?" +
                       " \"" + codeString3.Replace("\"", "\\\"") + "\":" + " \"" + codeString4.Replace("\"", "\\\"") +
                       "\""
                       + " }}";
            };
            InterpolRegexEvaluator = m =>
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
                    foreach (var c in new List<char> { '+', '-', '/', '|', '*' })
                    {
                        foreach (var s1 in tmpResult.Split(c).ToList().OrderByDescending(ss => ss.Split('.').Length)
                            )
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
                        tmpResult = bladeExpressionEvaluator.Evaluate(codeString);
                    }
                    catch (Exception te)
                    {
                        Console.WriteLine(te.Message + " " + te.InnerException?.Message + " " + te);
                        tmpResult = ""; // "[ERROR COMPILLING TEMPLATE]";
                    }
                }


                return tmpResult;
            };
        }

        internal MatchEvaluator InterpolRegexEvaluator { set; get; }
        internal MatchEvaluator MatchIfRegexEvaluator { set; get; }
        internal MatchEvaluator MatchIfElseRegexEvaluator { set; get; }
        internal MatchEvaluator MatchNestedIfElseRegexEvaluator { set; get; }
        internal MatchEvaluator VariableCreationRegexEvaluator { set; get; }
        internal MatchEvaluator IterationRegexEvaluator { set; get; }
      
    }
}