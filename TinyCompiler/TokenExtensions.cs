using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    internal static class TokenExtensions
    {
        internal static Boolean IsComparisonOperator(this IToken token) 
        {
            return token?.TokenType.IsComparisonOperator() ?? false;
        }
    }
}
