using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Build rung instructions from semi-compiled parts
        /// </summary>
        /// <param name="tempStatements">Temp variables statements</param>
        /// <param name="nodes">NodeExpression collection</param>
        /// <param name="outputs">NodeOutputs collection</param>
        /// <returns></returns>
        internal static List<string> BuildRung(List<string> tempStatements, List<NodeExpression> nodes, List<NodeOutputs> outputs)
        {
            List<string> buffer = new List<string>();
            //tempStatements.Add(string.Format(RATD + "[{0}]" + " = {1};", tempStatements.Count, lastParallel.Expression));

            //foreach (string item in tempStatements) sRung.Add(item);
            //sRung.Add("if (" + expBuffer + ")");
            //sRung.Add("{");
            //foreach (string item in ifStatements) sRung.Add(INDENT + item);
            //sRung.Add("}");
            //sRung.Add("else");
            //sRung.Add("{");
            //foreach (string item in elseStatements) sRung.Add(INDENT + item);
            //sRung.Add("}");


            return buffer;
        }

    }
}
