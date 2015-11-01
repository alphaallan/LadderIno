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
