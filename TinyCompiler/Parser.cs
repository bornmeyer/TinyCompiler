using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class Parser
    {
        // Events

        public event Action<String>? StepParsed;

        // Fields

        private readonly ILexer _lexer;
        private IToken? _currentToken;
        private IToken? _peekToken;
        private List<String> _symbols;
        private List<String> _labelsDeclared;
        private List<String> _labelsJumpedTo;

        // Constructors

        public Parser(ILexer lexer)
        {
            this._lexer = lexer;
            this._currentToken = null;
            this._peekToken = null;
            _symbols = [];
            _labelsDeclared = [];
            _labelsJumpedTo = [];
            NextToken();
            NextToken();
        }

        // Methods

        private Boolean CheckToken(IToken token) => _currentToken?.TokenType == token.TokenType;
        private Boolean CheckPeekToken(IToken token) => _peekToken?.TokenType == token.TokenType;

        public void NextToken()
        {
            _currentToken = _peekToken;
            _peekToken = _lexer.GetToken();
        }

        public Boolean CheckToken(TokenType tokenType) => tokenType == _currentToken?.TokenType;

        public Boolean CheckPeek(TokenType tokenType) => tokenType == _peekToken?.TokenType;   

        public void Match(TokenType tokenType)
        {
            if (!CheckToken(tokenType)) throw new Exception("wrong tokentype");
            NextToken();
        }

        public void Parse()
        {
            OnStepParsed("Parse started");

            while (CheckToken(TokenType.NEWLINE))
            {
               NextToken();
            }

            while (!CheckToken(TokenType.EOF))
            {
                Statement();
            }

            if (!_labelsDeclared.All(x => _labelsJumpedTo.Contains(x))) throw new Exception($"labels and gotos dont match");
        }

        public void Statement()
        {
            Action action = (_currentToken?.TokenType) switch
            {
                TokenType.PRINT => Print,
                TokenType.IF => If,
                TokenType.WHILE => Repeat,
                TokenType.LABEL => Label,
                TokenType.INPUT => Input,
                TokenType.GOTO => GoTo,
                TokenType.LET => Let,             
            };
            action();
            NewLine();
        }

        private void Print()
        {
            OnStepParsed("STATEMENT-PRINT");
            NextToken();
            if(CheckToken(TokenType.STRING)) 
                NextToken();
            else
                Expression();
        }

        private void If()
        {
            OnStepParsed("STATEMENT-IF");
            NextToken();
            Comparison();
            Match(TokenType.THEN);
            NewLine();
            while(!CheckToken(TokenType.ENDIF)) Statement();
            Match(TokenType.ENDIF);
        }

        private void Repeat()
        {
            OnStepParsed("STATEMENT-WHILE");
            NextToken();
            Comparison();
            Match(TokenType.REPEAT);
            NewLine();
            while (!CheckToken(TokenType.ENDWHILE)) Statement();
            Match(TokenType.ENDWHILE);
        }

        private void Label()
        {
            OnStepParsed("STATEMENT-LABEL");
            NextToken();
            if (_labelsDeclared.Contains(_currentToken?.Text ?? throw new Exception()))
                throw new Exception($"label \"{_currentToken.Text}\" already exists");
            _labelsDeclared.Add(_currentToken.Text);
            Match(TokenType.IDENT);
        }

        private void GoTo()
        {
            OnStepParsed("STATEMENT-GOTO");
            NextToken();
            _labelsJumpedTo.Add(_currentToken?.Text ?? throw new Exception());
            Match(TokenType.IDENT);
        }

        private void Let()
        {
            OnStepParsed("STATEMENT-LET");
            NextToken();
            if(!_symbols.Contains(_currentToken?.Text ?? throw new Exception()))
                _symbols.Add(_currentToken.Text);

            Match(TokenType.IDENT);
            Match(TokenType.EQ);
            Expression();
        }

        private void Input()
        {
            OnStepParsed("STATEMENT-INPUT");
            NextToken();
            if (!_symbols.Contains(_currentToken?.Text ?? throw new Exception()))
                _symbols.Add(_currentToken.Text);
            Match(TokenType.IDENT);
        }

        private void Comparison()
        {
            OnStepParsed("COMPARISON");
            Expression();
            if (!_currentToken.IsComparisonOperator()) throw new Exception();
            NextToken();
            Expression();     
            
            while(_currentToken.IsComparisonOperator())
            {
                NextToken();
                Expression();
            }
        }


        private void NewLine ()
        {
            OnStepParsed("NEWLINE");
            Match(TokenType.NEWLINE);
            while(CheckToken(TokenType.NEWLINE)) NextToken();
        }

        private void OnStepParsed(String message)
        {
            StepParsed?.Invoke(message);
        }

        public void Expression()
        {
            OnStepParsed("EXPRESSION");
            Term();
            while (CheckToken(TokenType.PLUS) || CheckToken(TokenType.MINUS))
            {
                NextToken();
                Term();
            }
            
        }

        private void Primary()
        {
            OnStepParsed($"PRIMARY ({_currentToken?.Text})");

            switch (_currentToken?.TokenType)
            {
                case TokenType.NUMBER:
                    NextToken();
                    break;
                case TokenType.IDENT:
                    if (!_symbols.Contains(_currentToken.Text)) throw new Exception($"Referencing variable {_currentToken.Text} before assignment");
                    NextToken();
                    break;
                default:
                    throw new Exception();
            }            
        }

        public void Term()
        {
            OnStepParsed("TERM");
            Unary();
            while(CheckToken(TokenType.ASTERISK) || CheckToken(TokenType.SLASH))
            {
                NextToken();
                Unary();
            }
        }

        private void Unary()
        {
            OnStepParsed("UNARY");
            if(CheckToken(TokenType.PLUS) || CheckToken(TokenType.MINUS)) NextToken();
            Primary();
        }
    }
}
