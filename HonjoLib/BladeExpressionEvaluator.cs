namespace HonjoLib
{
    public class BladeExpressionEvaluator : IBladeExpressionEvaluator
    {
        public string Evaluate(string expression)
        {
            /*VsaEngine*/
            var engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
            var result = Microsoft.JScript.Eval.JScriptEvaluate(expression, engine);

            return result.ToString();
        }
    }
}