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

        // Constructors

        public Parser(ILexer lexer)
        {
            this._lexer = lexer;
            this._currentToken = null;
            this._peekToken = null;
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
        }

        public void Statement()
        {
            IEnumerable<Action> actions = (_currentToken?.TokenType) switch
            {
                TokenType.PRINT => [Print, NewLine],
                TokenType.IF => [If, NewLine],
                TokenType.REPEAT => [Repeat, NewLine],
                TokenType.LABEL => [Label, NewLine],
                TokenType.INPUT => [Input, NewLine],
                TokenType.GOTO => [GoTo,  NewLine],
                TokenType.LET => [Let, NewLine],             
            };
            foreach (var action in actions) action();
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
            Match(TokenType.IDENT);
        }

        private void GoTo()
        {
            OnStepParsed("STATEMENT-GOTO");
            NextToken();
            Match(TokenType.IDENT);
        }

        private void Let()
        {
            OnStepParsed("STATEMENT-LET");
            NextToken();
            Match(TokenType.IDENT);
            Match(TokenType.EQ);
            Expression();
        }

        private void Input()
        {
            OnStepParsed("STATEMENT-INPUT");
            NextToken();
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

            if(!CheckToken(TokenType.NUMBER) && !CheckToken(TokenType.IDENT)) throw new Exception();
            NextToken();
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
