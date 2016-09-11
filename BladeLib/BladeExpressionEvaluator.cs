namespace BladeLib
{
    public class BladeExpressionEvaluator : IBladeExpressionEvaluator
    {
        public string Evaluate(string expression)
        {
            /*VsaEngine*/
            var engine = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();

            /** Result will be either true or false based on evaluation string*/
            var result = Microsoft.JScript.Eval.JScriptEvaluate(expression, engine);

            return result.ToString();
        }
    }
}