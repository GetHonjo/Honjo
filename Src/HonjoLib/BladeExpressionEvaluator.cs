using System;
using System.Collections.Generic;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;

namespace HonjoLib
{
    public class BladeExpressionEvaluator : IBladeExpressionEvaluator
    {
        public string Evaluate(string expression, List<Type> types, List<Tuple<string, Type>> namedTypes)
        {
            /*VsaEngine*/
            var engine = VsaEngine.CreateEngine();
            var result = Eval.JScriptEvaluate(expression, engine);

            return result.ToString();
        }
    }
}