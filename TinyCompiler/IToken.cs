namespace TinyCompiler
{
    public interface IToken
    {
        string Text { get; }
        TokenType TokenType { get; }
    }
}