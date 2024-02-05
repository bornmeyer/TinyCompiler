using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TinyCompiler.Tests
{
    public class LexerTests
    {
        [Fact]
        public void TestThatCommentsAreIngored()
        {
            // Assign
            var code = "+- # This is a comment!\n */";
            var sourceCode = new SourceCode(code);

            var systemUnderTest = new Lexer(sourceCode, new Tokenizer());

            var expected = new List<TokenType>
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.NEWLINE,
                TokenType.ASTERISK,
                TokenType.SLASH,
                TokenType.NEWLINE,
                TokenType.EOF
            };

            // Act

            var tokens = new List<IToken>();

            IToken? token = systemUnderTest.GetToken();
            tokens.Add(token);
            while (token.TokenType != TokenType.EOF)
            {
                token = systemUnderTest.GetToken();
                tokens.Add(token);
            }

            // Assert

            Assert.Equal(expected, tokens.Select(x => x.TokenType).ToList());

        }
    }
}
