﻿using System;
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
    }
}