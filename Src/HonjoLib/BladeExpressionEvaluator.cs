using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace HonjoLib
{
    public class BladeExpressionEvaluator : IBladeExpressionEvaluator
    {
        public string Evaluate(string expression)
        {

            /*VsaEngine*/
            var engine = VsaEngine.CreateEngine();
            var result = Eval.JScriptEvaluate(expression, engine);

            return result.ToString();
        }
    }
}