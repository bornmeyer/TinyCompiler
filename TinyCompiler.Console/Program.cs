using TinyCompiler;

internal class Program
{
    private static void Main(string[] args)
    {
        var rawsourceCode = "LET foobar = 123";
        var sourceCode = new SourceCode(rawsourceCode);
        var lexer = new Lexer(sourceCode, null);

        while(lexer.Peek != '\0')
        {
            Console.WriteLine(lexer.CurrentChar);
            lexer.NextChar();
        }

        Console.ReadLine();
    }
}