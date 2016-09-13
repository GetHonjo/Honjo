using System;
using System.Collections.Generic;

namespace HonjoLib
{
    public interface IBladeExpressionEvaluator
    {
        string Evaluate(string expression, List<Type> types, List<Tuple<string, Type>> namedTypes);
    }
}