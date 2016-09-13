using System;
using System.Collections.Generic;
using ExpressionEvaluator;

namespace HonjoLib
{
    public class NewExpressionEvaluator : IBladeExpressionEvaluator
    {
        private TypeRegistry Registry { set; get; }

        public string Evaluate(string expression, List<Type> types, List<Tuple<string, Type>> namedTypes)
        {
            if (Registry == null)
            {
                Registry = new TypeRegistry();
                Registry.RegisterType<DateTime>();

                foreach (var type in types)
                {
                    Registry.RegisterType(type.Name, type);
                }
                foreach (var namedType in namedTypes)
                {
                    if (string.IsNullOrEmpty(namedType.Item1))
                    {
                        throw new Exception("Invalid name '" + namedType.Item1 + "' provided for type '" +
                                            namedType.Item2 + "'");
                    }
                    Registry.RegisterType(namedType.Item1, namedType.Item2);
                }
            }

            var exp = new CompiledExpression(expression)
            {
                TypeRegistry = Registry
            };
            var result = exp.Eval();
            return result.ToString();
        }
    }
}