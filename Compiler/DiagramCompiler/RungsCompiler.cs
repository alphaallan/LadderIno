using Core.Components;
using System;
using System.Collections.Generic;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compile the diagram's rung collection
        /// </summary>
        /// <param name="rungs"></param>
        /// <param name="codeBuffer"></param>
        internal static void CompileRungs(IEnumerable<Rung> rungs, CompilerBuffer codeBuffer)
        {
            foreach (Rung rung in rungs)
            {
                #region Buffers
                List<string> tempStatements = new List<string>();
                List<NodeExpression> nodes = new List<NodeExpression>().RunAnalysis(rung);
                List<NodeOutputs> outputs = new List<NodeOutputs>();
                #endregion

                #region Analysis Loop
                foreach (ComponentBase component in rung.Components)
                {
                    NodeExpression NodeA = nodes.GetNodeConnections(component.LeftLide);
                    NodeExpression NodeB = nodes.GetNodeConnections(component.RightLide);

                    if(component.GetCompilerClass() == ComponentCompilerClass.Input)
                    {
                        if (!string.IsNullOrEmpty(NodeA.Expression))
                        {
                            NodeB += string.Format(NODE_NUMBER_MARKER + "{0} && {1}", nodes.IndexOf(NodeA), CompileInputComponent(component));
                        }
                        else
                        {
                            NodeB += CompileInputComponent(component);
                        }
                    }
                    else
                    {
                        NodeExpression lastParallel = nodes.GetLastParallel(NodeA);

                        if (!string.IsNullOrEmpty(NodeA.Expression) && !lastParallel.IsTempValue)
                        {
                            tempStatements.Add(string.Format(RATD + "[{0}]" + " = {1};", tempStatements.Count, lastParallel.Expression));
                            lastParallel.Expression = string.Format(RATD + "[{0}]", tempStatements.Count - 1);
                            lastParallel.IsTempValue = true;
                        }

                        if (component.GetCompilerClass() == ComponentCompilerClass.InputFunction)
                        {
                            if (!string.IsNullOrEmpty(NodeA.Expression))
                            {
                                NodeB += CompileInputFunctionComponent(component, NODE_NUMBER_MARKER + nodes.IndexOf(NodeA), codeBuffer);
                            }
                            else
                            {
                                NodeB += CompileInputFunctionComponent(component, TRUE, codeBuffer);
                            }
                        }
                        else
                        {
                            outputs.AddOutputs(NodeA.Node, CompileOutputComponent(component, codeBuffer));
                        }
                    }
                }
                #endregion

                if (codeBuffer.BoolTempCount < tempStatements.Count) codeBuffer.BoolTempCount = tempStatements.Count;

                List<string> sRung = BuildRung(tempStatements, nodes, outputs);
                if (!string.IsNullOrEmpty(rung.Comment)) sRung.Insert(0, "/*" + rung.Comment + "*/");

                codeBuffer.Rungs.Add(sRung);
            }
        }

        /// <summary>
        /// Add output statements to the NodeOutputs collection
        /// </summary>
        /// <param name="list">NodeOutputs buffer list</param>
        /// <param name="node">Reference node</param>
        /// <param name="statements">if and else statements</param>
        /// <returns></returns>
        internal static List<NodeOutputs> AddOutputs(this List<NodeOutputs> list, Node node, Tuple<string, string> statements)
        {
            int nodeIndex = list.IndexOf(new NodeOutputs(node));
            NodeOutputs nodeOutputs = null;

            if (nodeIndex == -1)
            {
                nodeOutputs = new NodeOutputs(node);
                list.Add(nodeOutputs);
            }
            else
            {
                nodeOutputs = list[nodeIndex];
            }

            nodeOutputs.IfStatements.Add(statements.Item1);
            nodeOutputs.ElseStatements.Add(statements.Item2);

            return list;
        }
    }
}
