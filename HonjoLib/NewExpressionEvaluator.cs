using System;
using ExpressionEvaluator;

namespace HonjoLib
{
    public class NewExpressionEvaluator : IBladeExpressionEvaluator
    {
        private static TypeRegistry registry;

        public string Evaluate(string expression)
        {
            if (registry == null)
            {
                registry = new TypeRegistry();
                registry.RegisterType<DateTime>();
            }

            var exp = new CompiledExpression(expression)
            {
                TypeRegistry = registry
            };
            var result = exp.Eval();
            return result.ToString();
        }
    }
}