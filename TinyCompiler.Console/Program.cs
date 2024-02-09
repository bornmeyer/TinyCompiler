using TinyCompiler;

internal class Program
{
    private static void Main(string[] args)
    {
        //var code = @"
        //    PRINT ""hello, world!""
        //    PRINT ""second line""
        //    PRINT ""and a third...""
        //";

        //var code = @"
        //LABEL loop
        //PRINT ""hello, world!""
        //GOTO loop
        //";

        var code = @"LET foo = bar * 3 + 2";

        var sourceCode = new SourceCode(code);
        var lexer = new Lexer(sourceCode, new Tokenizer());
        var parser = new Parser(lexer);
        parser.StepParsed += message => Console.WriteLine(message); 
        parser.Parse();
        Console.ReadLine();
    }
}