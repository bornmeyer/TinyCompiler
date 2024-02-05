using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class Token : IToken
    {
        // Fields

        private readonly String _text = default;
        private readonly TokenType _tokenType = default;

        // Properties

        public String Text => _text;

        public TokenType TokenType => _tokenType;

        // Constructors

        public Token(String text, TokenType tokenType)
        {
            _text = text;
            _tokenType = tokenType;
        }

        // Methods

        public override bool Equals(object? obj)
        {
            Boolean result = false;
            if(obj is Token other)
            {
                result = other.Text == _text && other.TokenType == _tokenType;
            }
            return result;
        }

        public override string ToString() => $@"({_text}: {_tokenType})";

        public static TokenType? CheckIfKeyword(String tokenText)
        {
            TokenType? result = null;
            foreach(TokenType current in Enum.GetValues(typeof(TokenType)).Cast<TokenType>()) 
            {
                if(current.ToString() == tokenText && ((Int32)current) >= 100 && ((Int32)current) <= 200) 
                    result = current;
            }

            return result;
        }
    }
}
