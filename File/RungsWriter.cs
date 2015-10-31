using Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace LDFile
{
    public static partial class LDFile
    {
        /// <summary>
        /// Write all Rungs
        /// </summary>
        /// <param name="writer">File generator</param>
        /// <param name="diagram">LD diagram</param>
        private static void WriteRungs(XmlWriter writer, IEnumerable<Rung> rungs)
        {
            writer.WriteStartElement("Rungs");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(rungs.Count());
            writer.WriteEndAttribute();

            #region Rung Loop
            foreach (Rung rung in rungs)
            {
                Trace.WriteLine("Rung Started", "DiagramWriter");
                Trace.Indent();

                #region Node Analisys
                //List of the nodes in circuit
                List<NodeConnections> nodes = new List<NodeConnections>();

                Trace.WriteLine("Starting node analysis", "Rung");
                //Generate circuit's node list
                foreach (ComponentBase component in rung.Components)
                {
                    NodeConnections NodeA = new NodeConnections(component.LeftLide);
                    NodeConnections NodeB = new NodeConnections(component.RightLide);

                    if (!nodes.Contains(NodeA))
                    {
                        nodes.Add(NodeA);
                        NodeA.OutComponents.Add(component);
                    }
                    else
                    {
                        int pos = nodes.IndexOf(NodeA);
                        nodes[pos].OutComponents.Add(component);
                    }

                    if (!nodes.Contains(NodeB))
                    {
                        nodes.Add(NodeB);
                        NodeB.InComponents.Add(component);
                    }
                    else
                    {
                        int pos = nodes.IndexOf(NodeB);
                        nodes[pos].InComponents.Add(component);
                    }
                }
                Trace.WriteLine("Analysis finished with " + nodes.Count + " nodes", "Rung");
                #endregion Node Analisys

                writer.WriteStartElement("Rung");
                writer.WriteStartAttribute("Count");
                writer.WriteValue(rung.Components.Count);
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("Comment");
                writer.WriteValue(rung.Comment);
                writer.WriteEndAttribute();

                #region Component Write
                Stack<int> Mode = new Stack<int>();
                Mode.Push(-1);

                Trace.WriteLine("Starting component write", "Rung");
                Trace.Indent();
                foreach (ComponentBase component in rung.Components)
                {
                    NodeConnections NodeA = nodes.GetNodeConnections(component.LeftLide);
                    NodeConnections NodeB = nodes.GetNodeConnections(component.RightLide);

                    //Decide when add a parallel sub-circuit
                    if (NodeA.OutComponents.Count > 1)
                    {
                        if (Mode.Peek() == -1)
                        {
                            Mode.Push(NodeA.OutComponents.Count - 1);
                            writer.WriteStartElement("Parallel");
                            Trace.WriteLine("Opened Parallel Sub-Circuit", "Rung");
                            Trace.Indent();
                        }
                        else Mode.Push(Mode.Pop() - 1);
                    }

                    // Decide when add a serial sub-circuit
                    if (NodeB.InComponents.Count == 1 && Mode.Peek() != -1)
                    {
                        Mode.Push(-1);
                        writer.WriteStartElement("Serial");
                        Trace.WriteLine("Opened Serial Sub-Circuit", "Rung");
                        Trace.Indent();
                    }

                    WriteComponent(writer, component);
                    Trace.WriteLine(component.ToString() + " Added", "Rung");

                    //Close sub-circuit
                    if ((NodeB.InComponents.Count > 1 || NodeA.InComponents.Count > 1) && Mode.Count > 1 && Mode.Peek() <= 0)
                    {
                        Mode.Pop();
                        writer.WriteEndElement();
                        Trace.Unindent();
                        Trace.WriteLine("Closed Sub-Circuit", "Rung");
                    }
                }
                Trace.Unindent();
                #endregion Component Write

                writer.WriteEndElement();
                Trace.Unindent();
                Trace.WriteLine("Rung Ended", "DiagramWriter");
            }
            #endregion Rung Loop

            writer.WriteEndElement();
        }

        /// <summary>
        /// Write a simgle component in file
        /// </summary>
        /// <param name="writer">File generator</param>
        /// <param name="component">Component to be writen</param>
        private static void WriteComponent(XmlWriter writer, ComponentBase component)
        {
            string componentTypeName = component.GetType().ToString().Replace("Core.Components.", string.Empty);

            writer.WriteStartElement(componentTypeName);

            if (component is NameableComponent)
            {
                writer.WriteStartAttribute("Name");
                writer.WriteValue((component as NameableComponent).Name);
                writer.WriteEndAttribute();
            }

            switch (componentTypeName)
            {
                #region Basic
                case "Coil":
                    writer.WriteStartAttribute("Mode");
                    writer.WriteValue((component as Coil).Mode.ToString());
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("Type");
                    writer.WriteValue((component as Coil).Type.ToString());
                    writer.WriteEndAttribute();
                    break;

                case "Contact":
                    writer.WriteStartAttribute("IsClosed");
                    writer.WriteValue((component as Contact).IsClosed);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("IsInverted");
                    writer.WriteValue((component as Contact).IsInverted);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("Type");
                    writer.WriteValue((component as Contact).Type.ToString());
                    writer.WriteEndAttribute();
                    break;
                #endregion Basic

                #region Compare Components
                case "EQU":
                case "GEQ":
                case "GRT":
                case "LEG":
                case "LES":
                case "NEQ":
                    writer.WriteStartAttribute("VarA");
                    writer.WriteValue((component as CompareComponent).VarA);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("VarB");
                    writer.WriteValue((component as CompareComponent).VarB);
                    writer.WriteEndAttribute();
                    break;
                #endregion Compare Components

                #region Counter Components
                case "CTC":
                case "CTD":
                case "CTU":
                    writer.WriteStartAttribute("Limit");
                    writer.WriteValue((component as CounterComponent).Limit);
                    writer.WriteEndAttribute();
                    break;
                #endregion Counter Components

                #region Math Components
                case "ADD":
                case "DIV":
                case "MUL":
                case "SUB":
                case "MOV":
                    writer.WriteStartAttribute("Destination");
                    writer.WriteValue((component as MathComponent).Destination);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("VarA");
                    writer.WriteValue((component as MathComponent).VarA);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("VarB");
                    writer.WriteValue((component as MathComponent).VarB);
                    writer.WriteEndAttribute();
                    break;
                #endregion Math Components

                #region Analog Components
                case "ADC":
                    writer.WriteStartAttribute("Destination");
                    writer.WriteValue((component as ADC).Destination);
                    writer.WriteEndAttribute();
                    break;

                case "PWM":
                    writer.WriteStartAttribute("DudyCycle");
                    writer.WriteValue((component as PWM).DudyCycle);
                    writer.WriteEndAttribute();
                    break;
                #endregion Analog Components

                #region Components that do not require any extra processing
                case "OSF":
                case "OSR":
                case "SC":
                case "RES":
                    //Nothing to do were
                    break;
                #endregion Components that do not require any extra processing

                default:
                    throw new ArgumentException("Unknow Component", "component");
            }

            writer.WriteStartAttribute("Comment");
            writer.WriteValue(component.Comment);
            writer.WriteEndAttribute();

            writer.WriteEndElement();
        } 
    }
}
