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

            (TokenType tokenType, String value) = (character, peekAhead) switch
            {
                ('+', _) => (TokenType.PLUS, character.ToString()),
                ('-', _) => (TokenType.MINUS, character.ToString()),
                ('*', _) => (TokenType.ASTERISK, character.ToString()),
                ('/', _) => (TokenType.SLASH, character.ToString()),
                ('\n', _) => (TokenType.NEWLINE, character.ToString()),
                ('\0', _) => (TokenType.EOF, character.ToString()),
                ('=', '=') => (movOnOperator(TokenType.EQEQ), character.ToString()),
                ('=', _) => (TokenType.EQ, character.ToString()),
                ('>', '=') => (movOnOperator(TokenType.GTEQ), character.ToString()),
                ('>', _) => (TokenType.GT, character.ToString()),
                ('<', '=') => (movOnOperator(TokenType.LTEQ), character.ToString()),
                ('<', _) => (TokenType.LT, character.ToString()),
                ('!', '=') => (movOnOperator(TokenType.NOTEQ), character.ToString()),
                ('!', _) => throw new ArgumentException(),
                ('\"', _) => (TokenType.STRING, ReadString(sourceCode)),
                _ => throw new ArgumentException()
            }; 
           
            return (new Token(value, tokenType), sourceCode);
        }

        private String ReadString(SourceCode sourceCode)
        {
            sourceCode.NextChar();
            var startingposition = sourceCode.CurrentPosition;
            var illegalCharacters = new Char[] { '\r', '\n', '\t', '\\', '%' };

            while (sourceCode.CurrentChar != '\"')
            {
                if(illegalCharacters.Contains(sourceCode.CurrentChar)) throw new ArgumentException("character not allowed");
                sourceCode.NextChar();
            }
            return sourceCode.GetSlice(startingposition, sourceCode.CurrentPosition);
        }
    }
}
