using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    internal class FunctionHelpers
    {
        internal static Func<T1, TResult> PartiallyApply<T1, T2, TResult>(Func<T1, T2, TResult> function, T2 argument2)
        {
            return argument1 => function(argument1, argument2);
        }
    }
}
