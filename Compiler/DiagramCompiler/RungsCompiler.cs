using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        internal static void CompileRungs(IEnumerable<Rung> rungs, CompilerBuffer codeBuffer)
        {
            foreach (Rung rung in rungs)
            {
                List<string> tempStatements = new List<string>();
                List<string> ifStatements = new List<string>();
                List<string> elseStatements = new List<string>();

                Node outputFrontier = rung.GetOutputFrontier();
                int outputBorder = rung.Components.Where(x => x.RightLide == outputFrontier).Max(x => rung.Components.IndexOf(x));


                List<NodeExpression> nodes = new List<NodeExpression>().RunAnalisys(rung);
                List<string> sRung = new List<string>();
                if (!string.IsNullOrEmpty(rung.Comment)) sRung.Add("/*" + rung.Comment + "*/");

                Stack<int> CircuitModeStack = new Stack<int>();
                CircuitModeStack.Push(-1);

                for (int pos = 0; pos <= outputBorder; pos++)
                {
                    ComponentBase component = rung.Components[pos];
                    NodeExpression NodeA = nodes.GetNodeConnections(component.LeftLide);
                    NodeExpression NodeB = nodes.GetNodeConnections(component.RightLide);

                    //Decide when add a parallel sub-circuit
                    if (NodeA.OutComponents.Count > 1)
                    {
                        if (CircuitModeStack.Peek() == -1)
                        {
                            CircuitModeStack.Push(NodeA.OutComponents.Count - 1);
                        }
                        else CircuitModeStack.Push(CircuitModeStack.Pop() - 1);
                    }

                    // Decide when add a serial sub-circuit
                    if (NodeB.InComponents.Count == 1 && CircuitModeStack.Peek() != -1)
                    {
                        CircuitModeStack.Push(-1);
                    }


                    if (component is Contact)
                    {
                        Contact contact = (component as Contact);
                    }
                    else if (component is OSR)
                    {

                    }

                    //Close sub-circuit
                    if ((NodeB.InComponents.Count > 1 || NodeA.InComponents.Count > 1) && CircuitModeStack.Count > 1 && CircuitModeStack.Peek() <= 0)
                    {
                        CircuitModeStack.Pop();
                    }
                }


                //foreach (string item in tempStatements) sRung.Add(item);
                //sRung.Add("if (" + expBuffer + ")");
                //sRung.Add("{");
                //foreach (string item in ifStatements) sRung.Add(INDENT + item);
                //sRung.Add("}");
                //sRung.Add("else");
                //sRung.Add("{");
                //foreach (string item in elseStatements) sRung.Add(INDENT + item);
                //sRung.Add("}");

                codeBuffer.Rungs.Add(sRung);
            }


            //codeBuffer.BoolTempCount = 1;

            //codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X1;", "Y1 = (((" + OSR_FN + "(0, " + RATD + "[0]) && !Y2) || Y1) && !X2);" });
            //codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X2;", "Y2 = (((" + OSR_FN + "(1, " + RATD + "[0]) && !Y1) || Y2) && !X1);" });
        }

        private static Tuple<string, string> GetOutComponentActions(this ComponentBase component, CompilerBuffer codeBuffer)
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
