using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public class CLangEmitter : IEmitter
    {
        // Events

        public event Action<String>? CompilationCompleted;

        // Fields

        private String _code;
        private String _header;

        // Constructors

        public CLangEmitter()
        {
            _code = String.Empty;
            _header = String.Empty;
        }

        // Methods

        public void Emit(String code)
        {
            _code = String.Concat(_code, code, "\n");
        }

        public void EmitLine(String line)
        {
            _code = String.Concat(_code, line, "\n");
        }

        public void HeaderLine(String line)
        {
            _header = String.Concat(_header, line, "\n");
        }

        public void OnCompilationCompleted()
        {
            CompilationCompleted?.Invoke(_header + _code);
        }
    }
}
