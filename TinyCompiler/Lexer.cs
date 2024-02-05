namespace TinyCompiler
{
    public class Lexer : ILexer
    {
        // Fields

        private readonly SourceCode _sourceCode = default;
        private readonly ITokenizer _tokenizer = default;

        // Properties

        public Char Peek => _sourceCode.Peek();

        public Char CurrentChar => _sourceCode.CurrentChar;

        // Constructors

        public Lexer(SourceCode sourceCode, ITokenizer tokenizer)
        {
            this._sourceCode = sourceCode;
            this._tokenizer = tokenizer;
        }

        // Methods       

        public IToken GetToken()
        {
            _sourceCode.SkipWhitespace().SkipComment();
            var ( result, code) = _tokenizer.TokenFor(_sourceCode.CurrentChar, _sourceCode.Peek(), _sourceCode);            
            _sourceCode.NextChar();
            return result;
        }
    }
}
