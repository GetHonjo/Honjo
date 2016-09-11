using DynamicExpresso;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace HonjoLib
{
    public class DynamicExpressoExpressionEvaluator : IBladeExpressionEvaluator
    {
        public string Evaluate(string expression)
        {
           
            var interpreter = new Interpreter();
            var result = interpreter.Eval(expression);

            return result.ToString();
        }
    }
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