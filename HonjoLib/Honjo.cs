namespace HonjoLib
{
    public class Honjo
    {
        public Honjo(IBladeExpressionEvaluator bladeExpressionEvaluator = null)
        {
            BladeExpressionEvaluator = bladeExpressionEvaluator 
                ?? new DynamicExpressoExpressionEvaluator();
            //?? new NewExpressionEvaluator();
            // ?? new BladeExpressionEvaluator();
        }

        internal IBladeExpressionEvaluator BladeExpressionEvaluator { set; get; }

      

        private MatchEvaluators MatchEvaluators { set; get; }

        public string Compile(string s, object o, bool trimEveryResult = false)
        {
            var dic = MatchEvaluators.ToDictionary(o);

            MatchEvaluators=  new MatchEvaluators(dic, o, trimEveryResult,BladeExpressionEvaluator);

            var hasUpdate = false;
            do
            {
                hasUpdate = MatchEvaluators. IterationRegex.Match(s).Success;
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