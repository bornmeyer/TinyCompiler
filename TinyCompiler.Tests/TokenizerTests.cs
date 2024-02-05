using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TinyCompiler.Tests
{
    public class TokenizerTests
    {
        private const String code = "+-*/";

        [Fact]
        public void TestThatTokensGetGenerated()
        {
            // Assign

            var systemUnderTest = new Tokenizer();

            // Act

            var tokens = new List<IToken>();
            foreach (var currentChar in code)
            {
                var (actual, _) = systemUnderTest.TokenFor(currentChar, (char)32, new SourceCode(code));
                tokens.Add(actual);
            }

            // Assert

            Assert.Equal(tokens.Count(), code.Length);
            Assert.Equal(code, tokens.Aggregate("", (acc, x) => acc + x.Text));
        }

        [Fact]
        public void TestThatOperatorTokensGetGenerated()
        {
            // Assign

            var systemUnderTest = new Tokenizer();
            var operators = "+- */ >>= = !=";
            var sourceCode = new SourceCode(operators);

            //  = "+- */ >>= = !="
            var expected = new List<TokenType>
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.ASTERISK,
                TokenType.SLASH,
                TokenType.GT,
                TokenType.GTEQ,
                TokenType.EQ,
                TokenType.NOTEQ,
                TokenType.NEWLINE,
                TokenType.EOF
            };

            // Act

            var tokens = new List<IToken>();

            IToken? token = null;
            (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
            while(token.TokenType != TokenType.EOF)
            {
                sourceCode.SkipWhitespace();
                (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
                sourceCode.NextChar();
                tokens.Add(token);
            }

            Assert.Equal(expected, tokens.Select(x => x.TokenType).ToList());
        }

        [Fact]
        public void TestThatYouCanTokenizeStrings()
        {
            // Assign

            var code = "+- \"This is a string\" # This is a comment!\n */";
            var sourceCode = new SourceCode(code);

            var systemUnderTest = new Tokenizer();

            var expected = new List<TokenType>
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.STRING,
                TokenType.NEWLINE,
                TokenType.ASTERISK,
                TokenType.SLASH,
                TokenType.NEWLINE,
                TokenType.EOF
            };

            var tokens = new List<IToken>();

            IToken? token = null;
            (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
            while (token.TokenType != TokenType.EOF)
            {
                sourceCode.SkipWhitespace().SkipComment();
                (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
                sourceCode.NextChar();
                tokens.Add(token);
            }

            Assert.Equal(expected, tokens.Select(x => x.TokenType).ToList());
        }

        [Fact]
        public void TestThatYouCanTokenizeNumbers()
        {
            // Assign

            var code = "+-123 9.8654*/";
            var sourceCode = new SourceCode(code);

            var systemUnderTest = new Tokenizer();

            var expected = new List<TokenType>
            {
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.NUMBER,
                TokenType.NUMBER,
                TokenType.ASTERISK,
                TokenType.SLASH,
                TokenType.NEWLINE,
                TokenType.EOF
            };

            var tokens = new List<IToken>();

            IToken? token = null;
            (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
            while (token.TokenType != TokenType.EOF)
            {
                sourceCode.SkipWhitespace().SkipComment();
                (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
                sourceCode.NextChar();
                tokens.Add(token);
            }

            Assert.Equal(expected, tokens.Select(x => x.TokenType).ToList());
        }

        [Fact]
        public void TestThatYouCanTokenizeKeywords()
        {
            // Assign

            var code = "IF+-123 foo*THEN/";
            var sourceCode = new SourceCode(code);

            var systemUnderTest = new Tokenizer();

            var expected = new List<TokenType>
            {
                TokenType.IF,
                TokenType.PLUS,
                TokenType.MINUS,
                TokenType.NUMBER,
                TokenType.IDENT,
                TokenType.ASTERISK,
                TokenType.THEN,
                TokenType.SLASH,
                TokenType.NEWLINE,
                TokenType.EOF
            };

            var tokens = new List<IToken>();

            IToken? token = null;
            
            do
            {
                sourceCode.SkipWhitespace().SkipComment();
                (token, _) = systemUnderTest.TokenFor(sourceCode.CurrentChar, sourceCode.Peek(), sourceCode);
                sourceCode.NextChar();
                tokens.Add(token);
            } while (token.TokenType != TokenType.EOF);

            Assert.Equal(expected, tokens.Select(x => x.TokenType).ToList());
        }
    }
}
