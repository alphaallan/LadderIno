using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Generate code from data table variables
        /// </summary>
        /// <param name="table"></param>
        /// <param name="codeBuffer"></param>
        internal static void CompileDataTable(LadderDataTable table, CompilerBuffer codeBuffer)
        {
            List<Tuple<string, Type, LDVarClass, object>> tuples = table.ListAllData().OrderBy(x => x.Item1).ToList();

            codeBuffer.Globals.Add("//Inputs");
            StringBuilder inputList = new StringBuilder();
            foreach (var tuple in tuples.Where(x => x.Item3 == LDVarClass.Input)) codeBuffer.Globals.Add("boolean " + tuple.Item1 + ";");

            codeBuffer.Globals.Add(string.Empty);
            codeBuffer.Globals.Add("//Outputs");
            foreach (var tuple in tuples.Where(x => x.Item3 == LDVarClass.Output)) codeBuffer.Globals.Add("boolean " + tuple.Item1 + ";");

            codeBuffer.Globals.Add(string.Empty);
            codeBuffer.Globals.Add("//Data");
            foreach (var tuple in tuples.Where(x => x.Item3 == LDVarClass.Data))
            {
                switch (tuple.Item2.ToString().Replace("System.", string.Empty))
                {
                    case "Boolean":
                        codeBuffer.Globals.Add("boolean " + tuple.Item1 + " = " + (((bool)tuple.Item4) ? "true;" : "false;"));
                        break;

                    case "Int16":
                        codeBuffer.Globals.Add("int " + tuple.Item1 + " = " + tuple.Item4.ToString() + ";");
                        break;

                    case "Byte":
                        codeBuffer.Globals.Add("byte " + tuple.Item1 + " = " + tuple.Item4.ToString() + ";");
                        break;

                    default:
                        throw new FormatException("Unrecognized variable type in data table");
                }
            }


            foreach (var tuple in tuples.Where(x => x.Item3 == LDVarClass.InFunction))
            {
                List<string> buffer = new List<string>();
                buffer.Add("boolean " + tuple.Item1 + "(boolean input)");
                buffer.Add("{");
                foreach (string line in tuple.Item4.ToString().Split('\n')) buffer.Add(INDENT + line);
                buffer.Add("}");
                codeBuffer.Functions.Add(buffer);
            }

            foreach (var tuple in tuples.Where(x => x.Item3 == LDVarClass.OutFunction))
            {
                List<string> buffer = new List<string>();
                buffer.Add("void " + tuple.Item1 + "()");
                buffer.Add("{");
                foreach (string line in tuple.Item4.ToString().Split('\n')) buffer.Add(INDENT + line);
                buffer.Add("}");
                codeBuffer.Functions.Add(buffer);
            }
        }
    }
}
