namespace TinyCompiler
{
    public interface ITokenizer
    {
        (IToken, SourceCode) TokenFor(Char character, Char peekAhead, SourceCode sourceCode);
    }
}