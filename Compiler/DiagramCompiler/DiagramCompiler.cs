using Core.Components;
using System;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compile a Ladder Diagram to Arduino C++
        /// </summary>
        /// <param name="diagram"></param>
        /// <returns></returns>
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
