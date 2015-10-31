using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDFile
{
    public static partial class LDFile
    {

        public static string CompileDiagram(Diagram diagram)
        {
            if (diagram == null) throw new ArgumentNullException("diagram", "Null Diagram");

            #region Startup
            CompilerBuffer mainBuffer = new CompilerBuffer();
            #endregion Startup

            ProcessDataTable(diagram.DataTable, mainBuffer);
            ProcessPinout(diagram.Pins, mainBuffer);

            mainBuffer.BoolTempCount = 1;
            mainBuffer.OSRCount = 2;
            

            mainBuffer.Rungs.Add(new List<string> { RATD + "[0] = X1;", "Y1 = (((" + OSR_FN + "(0, " + RATD + "[0]) && !Y2) || Y1) && !X2);" });
            mainBuffer.Rungs.Add(new List<string> { RATD + "[0] = X2;", "Y2 = (((" + OSR_FN + "(1, " + RATD + "[0]) && !Y1) || Y2) && !X1);" });

            return CodeBuilder(mainBuffer);
        }

    }
}
