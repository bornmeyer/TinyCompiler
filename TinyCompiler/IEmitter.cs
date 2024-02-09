using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public interface IEmitter
    {
        event Action<String>? CompilationCompleted;

        void Emit(String code);

        void EmitLine(String line);

        void HeaderLine(String line);
        
        void OnCompilationCompleted();
    }
}
