using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDFile
{
    public static partial class DiagramCompiler
    {
        private static void CompileRungs(IEnumerable<Rung> pins, CompilerBuffer codeBuffer)
        {
            codeBuffer.BoolTempCount = 1;
            codeBuffer.OSRCount = 2;

            codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X1;", "Y1 = (((" + OSR_FN + "(0, " + RATD + "[0]) && !Y2) || Y1) && !X2);" });
            codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X2;", "Y2 = (((" + OSR_FN + "(1, " + RATD + "[0]) && !Y1) || Y2) && !X1);" });
        }
    }
}
