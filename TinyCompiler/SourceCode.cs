using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class SourceCode
    {
        // Fields

        private readonly String _sourceCode = String.Empty;
        private Char _currentChar = '\0';
        private Int32 _currentPosition;

        // Properties

        public Char CurrentChar => _currentChar;

        // Constructors

        public SourceCode(String sourceCode)
        {
            this._sourceCode = String.Concat(sourceCode, '\n');
            this._currentChar = '\0';
            this._currentPosition = -1;
            NextChar();
        }

        // Methods

        public Char NextChar()
        {
            _currentPosition += 1;
            _currentChar = _currentPosition >= _sourceCode.Length ? '\0' : _sourceCode[_currentPosition];
            return _currentChar;
        }

        public Char Peek()
        {
            return _currentPosition + 1 >= _sourceCode.Length ? '\0' : _sourceCode[_currentPosition + 1];
        }

        public SourceCode SkipWhitespace()
        {
            while (_currentChar == (char)32 || _currentChar == '\t' || _currentChar == '\r')
            {
                NextChar();
            }
            return this;
        }

        public SourceCode SkipComment()
        {
            if(_currentChar == '#')
            {
                while(_currentChar != '\n') NextChar();
            }
            return this;
        }

    }
}
