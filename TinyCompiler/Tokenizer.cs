using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class Tokenizer : ITokenizer
    {
        public (IToken, SourceCode) TokenFor(Char character, Char peekAhead, SourceCode sourceCode)
        {
            Func<TokenType, TokenType> movOnOperator = tokenType =>
            {
                sourceCode.NextChar();
                return tokenType;
            };

            TokenType tokenType = (character, peekAhead) switch
            {
                ('+', _) => TokenType.PLUS,
                ('-', _) => TokenType.MINUS,
                ('*', _) => TokenType.ASTERISK,
                ('/', _) => TokenType.SLASH,
                ('\n', _) => TokenType.NEWLINE,
                ('\0', _) => TokenType.EOF,
                ('=', '=') => movOnOperator(TokenType.EQEQ),
                ('=', _) => TokenType.EQ,
                ('>', '=') => movOnOperator(TokenType.GTEQ),
                ('>', _) => TokenType.GT,
                ('<', '=') => movOnOperator(TokenType.LTEQ),
                ('<', _) => TokenType.LT,
                ('!', '=') => movOnOperator(TokenType.NOTEQ),
                ('!', _) => throw new ArgumentException(),
                _ => throw new ArgumentException()
            }; 
           
            return (new Token(character.ToString(), tokenType), sourceCode);
        }

        
    }
}
