using System;
using System.Collections.Generic;
using System.Linq;

namespace HonjoLib
{
    public class Honjo
    {
        private bool UseNamedTypeAsCamelCase { set; get; }
        public Honjo(bool useNamedTypeAsCamelCase, List<Type> types, params Tuple<string, Type>[] namedTypesToUse)
        {
            Types = types.ToList() ?? new List<Type>();
            InitializeWith(useNamedTypeAsCamelCase, namedTypesToUse);
        }
        public Honjo(bool useNamedTypeAsCamelCase, params Tuple<string, Type>[] namedTypesToUse)
        {
            InitializeWith(useNamedTypeAsCamelCase, namedTypesToUse);
        }

        private void InitializeWith(bool useNamedTypeAsCamelCase, Tuple<string, Type>[] namedTypesToUse)
        {
            UseNamedTypeAsCamelCase = useNamedTypeAsCamelCase;
            var namedTypes = new List<Tuple<string, Type>>();
            Types = new List<Type>();
            NamedTypes = namedTypes.ToList() ?? new List<Tuple<string, Type>>();
            foreach (var namedType in namedTypesToUse)
            {
                if (string.IsNullOrEmpty(namedType.Item1))
                {
                    throw new Exception("Invalid name '" + namedType.Item1 + "' provided for type '" + namedType.Item2 +
                                        "'");
                }
                namedTypes.Add(
                    new Tuple<string, Type>(
                        useNamedTypeAsCamelCase
                            ? char.ToLowerInvariant(namedType.Item1[0]) + namedType.Item1.Substring(1)
                            : namedType.Item1, namedType.Item2));
            }
            BladeExpressionEvaluator = new NewExpressionEvaluator();
        }

        public Honjo(params Type[] types)
        {
            NamedTypes = new List<Tuple<string, Type>>();
            Types = types.ToList() ?? new List<Type>();
            BladeExpressionEvaluator = new NewExpressionEvaluator();
        }

        private List<Type> Types { get; set; }
        private List<Tuple<string, Type>> NamedTypes { get; set; }

        internal IBladeExpressionEvaluator BladeExpressionEvaluator { set; get; }


        private MatchEvaluators MatchEvaluators { set; get; }

        public string Compile(string s, object o, bool trimEveryResult = false)
        {
            var dic = o.ToDictionary();
            MatchEvaluators = new MatchEvaluators(dic, o, trimEveryResult, BladeExpressionEvaluator, Types, NamedTypes, UseNamedTypeAsCamelCase);

            var hasUpdate = false;
            do
            {
                hasUpdate = MatchEvaluators.IterationRegex.Match(s).Success;
                if (hasUpdate)
                {
                    s = MatchEvaluators.IterationRegex.Replace(s, MatchEvaluators.IterationRegexEvaluator);
                }
            } while (hasUpdate);

            s = MatchEvaluators.VariableCreationRegex.Replace(s, MatchEvaluators.VariableCreationRegexEvaluator);
            s = MatchEvaluators.MatchNestedIfElseRegex.Replace(s, MatchEvaluators.MatchNestedIfElseRegexEvaluator);
            s = MatchEvaluators.MatchIfElseRegex.Replace(s, MatchEvaluators.MatchIfElseRegexEvaluator);
            s = MatchEvaluators.MatchIfRegex.Replace(s, MatchEvaluators.MatchIfRegexEvaluator);
            var result = MatchEvaluators.InterpolRegex.Replace(s, MatchEvaluators.InterpolRegexEvaluator);
            return result;
        }
    }
}