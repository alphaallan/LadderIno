using Core.Components;
using System;
using System.Collections.Generic;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        public static string CompileDiagram(Diagram diagram)
        {
            if (diagram == null) throw new ArgumentNullException("diagram", "Null Diagram");

            CompilerBuffer codeBuffer = new CompilerBuffer();

            CompileDataTable(diagram.DataTable, codeBuffer);
            CompilePinout(diagram.Pins, codeBuffer);
            CompileRungs(diagram.Rungs, codeBuffer);

            return CodeBuilder(codeBuffer);
        }

    }
}
