using Xunit;

namespace TinyCompiler.Tests
{
    public class SourceCodeTests
    {
        private static String code = "LET foobar = 123";

        [Fact]
        public void TestThatYouCanIterate()
        {
            // Assign

            var expected = code[1];
            var systemUnderTest = new SourceCode(code);

            // Act

            var actual = systemUnderTest.NextChar();

            // Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestThatYouGetATermiantorWhenExceedingCodeLength()
        {
            // Assign

            var expected = '\0';
            var systemUnderTest = new SourceCode(code);

            // Act

            while (systemUnderTest.CurrentChar != '\n') systemUnderTest.NextChar();
            var actual = systemUnderTest.NextChar();

            // Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestThatYouCanPeek() 
        {
            // Assign

            var expected = code[1];
            String current = String.Empty;
            var systemUnderTest = new SourceCode(code);

            // Act

            var actual = systemUnderTest.Peek();

            // Assert

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestThatYouGetATerminatorWhenPeekingAboveLength()
        {
            // Assign

            var expected = '\0';
            var systemUnderTest = new SourceCode(code);

            // Act

            while (systemUnderTest.CurrentChar != '\n') systemUnderTest.NextChar();
            var actual = systemUnderTest.Peek();

            // Assert

            Assert.Equal(expected, actual);
        }
    }
}
