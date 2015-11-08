using Core.Components;
using System;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compile a single output component
        /// </summary>
        /// <param name="component">Component to be compiled</param>
        /// <param name="codeBuffer">Compiler's code buffer</param>
        /// <returns>If and else statements</returns>
        internal static Tuple<string, string> CompileOutputComponent(ComponentBase component, CompilerBuffer codeBuffer)
        {
            string ifCommand = string.Empty, elseCommand = string.Empty;

            if (component is Coil)
            {
                Coil coil = component as Coil;
                switch (coil.Mode)
                {
                    case Coil.CoilMode.Negated:
                        ifCommand = coil.FullName + " = false;";
                        elseCommand = coil.FullName + " = true;";
                        break;

                    case Coil.CoilMode.Normal:
                        ifCommand = coil.FullName + " = true;";
                        elseCommand = coil.FullName + " = false;";
                        break;

                    case Coil.CoilMode.Reset:
                        ifCommand = coil.FullName + " = false;";
                        break;

                    case Coil.CoilMode.Set:
                        ifCommand = coil.FullName + " = true;";
                        break;
                }
            }
            else if (component is ADC)
            {
                ifCommand = (component as ADC).Destination + " = analogRead(" + (component as ADC).FullName + ");";
            }
            else if (component is PWM)
            {
                ifCommand = "analogWrite(" + (component as ADC).FullName + ", " + (component as PWM).DudyCycle + ");";
                elseCommand = "analogWrite(" + (component as ADC).FullName + ", 0);";
            }
            else if (component is RES)
            {
                ifCommand = (component as RES).FullName + " = 0;";
            }
            else if (component is CTC)
            {
                CTC ctc = component as CTC;
                ifCommand = "if (" + OSR_FN + "(" + codeBuffer.OSRCount + ", true)) " + ctc.FullName + " = (" + ctc.FullName + " >= " + ctc.Limit + ") ? 0 : " + ctc.FullName + " + 1;";
                elseCommand = OSR_FN + "(" + codeBuffer.OSRCount + ", false));";
                codeBuffer.OSRCount++;
            }
            else if (component is MathComponent)
            {
                MathComponent mc = component as MathComponent;

                if (component is ADD)
                {
                    ifCommand = mc.Destination + " = " + mc.VarA + " + " + mc.VarB + ";";
                }
                else if (component is DIV)
                {
                    ifCommand = mc.Destination + " = " + mc.VarA + " / " + mc.VarB + ";";
                }
                else if (component is MUL)
                {
                    ifCommand = mc.Destination + " = " + mc.VarA + " * " + mc.VarB + ";";
                }
                else if (component is SUB)
                {
                    ifCommand = mc.Destination + " = " + mc.VarA + " - " + mc.VarB + ";";
                }
                else if (component is MOV)
                {
                    ifCommand = mc.Destination + " = " + mc.VarA + ";";
                }
            }

            return new Tuple<string, string>(ifCommand, elseCommand);
        }
    }
}
