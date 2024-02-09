using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    internal static class TokenTypeExtensions
    {
        internal static Boolean CheckToken(this TokenType tokenType, TokenType other) => tokenType == other;

        internal static Boolean IsComparisonOperator(this TokenType operatorType)
        {
            return operatorType == TokenType.GT || 
                operatorType == TokenType.LT || 
                operatorType == TokenType.GTEQ || 
                operatorType == TokenType.LTEQ || 
                operatorType == TokenType.EQEQ ||
                operatorType == TokenType.NOTEQ;
        }
    }
}
