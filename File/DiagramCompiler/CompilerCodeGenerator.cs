using Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LDFile
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Generate code from data table variables
        /// </summary>
        /// <param name="table"></param>
        /// <param name="codeBuffer"></param>
        private static void ProcessDataTable(LadderDataTable table, CompilerBuffer codeBuffer)
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
        }

        /// <summary>
        /// Generate code from pinout
        /// </summary>
        /// <param name="pins"></param>
        /// <param name="codeBuffer"></param>
        private static void ProcessPinout(IEnumerable<LDPin> pins, CompilerBuffer codeBuffer)
        {
            if (pins.Count(x => x.Pin == "NONE") > 0) throw new InvalidOperationException("Can't Compile with unassigned pins in diagram");

            codeBuffer.SetupContent.Add("//Inputs");
            foreach (var pin in pins.Where(x => x.Type == PinType.Input))
            {
                codeBuffer.SetupContent.Add("pinMode(" + pin.Pin + ", INPUT);//" + pin.Variable);
                codeBuffer.InputRefreshContent.Add(pin.Variable + " = digitalRead(" + pin.Pin + ");");
            }

            foreach (var pin in pins.Where(x => x.Type == PinType.Analog)) codeBuffer.SetupContent.Add("//Pin " + pin.Pin + " used as analog input to " + pin.Variable);


            codeBuffer.SetupContent.Add("//Outputs");
            foreach (var pin in pins.Where(x => x.Type == PinType.Output))
            {
                codeBuffer.SetupContent.Add("pinMode(" + pin.Pin + ", OUTPUT);//" + pin.Variable);
                codeBuffer.OutputRefreshContent.Add("digitalWrite(" + pin.Pin + ", " + pin.Variable + ");");
            }
            foreach (var pin in pins.Where(x => x.Type == PinType.PWM)) codeBuffer.SetupContent.Add("//Pin " + pin.Pin + " used as analog output to " + pin.Variable);

        }
    }
}
