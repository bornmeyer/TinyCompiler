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

        var code = @"
        PRINT ""How many fibonacci numbers do you want?""
        INPUT nums
        PRINT """"

        LET a = 0
        LET b = 1
        WHILE nums > 0 REPEAT
            PRINT a
            LET c = a + b
            LET a = b
            LET b = c
            LET nums = nums - 1
        ENDWHILE
        ";
        
        var sourceCode = new SourceCode(code.Replace(Environment.NewLine, "\n"));
        var lexer = new Lexer(sourceCode, new Tokenizer());
        var emitter = new CLangEmitter();
        emitter.CompilationCompleted += code => Console.WriteLine(code);
        var parser = new Parser(lexer, emitter);        
        parser.Parse();
        Console.ReadLine();
    }
}