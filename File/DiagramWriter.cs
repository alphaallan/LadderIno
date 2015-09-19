﻿using Core.Data;
using Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;

namespace LDFile
{
    public static partial class LDFile
    {
        /// <summary>
        /// Write a LD Diagram to File
        /// </summary>
        /// <param name="diagram">Diagram to be written</param>
        /// <param name="filePath">File path</param>
        public static void SaveDiagram(Diagram diagram, string filePath)
        {
            if (diagram == null) throw new ArgumentNullException("diagram", "Null Diagram");
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath", "Empty File Path");

            #region Stratup
            Trace.WriteLine("Save process started", "LD File");
            Trace.Indent();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.Indent = true;

            XmlWriter writer = XmlWriter.Create(filePath, xmlWriterSettings);

            writer.WriteStartDocument();
            Trace.WriteLine("Writer Started", "LD File");
            #endregion Stratup

            #region Comment Header
            writer.WriteComment("This files has created by DiagramWriter V. 0.2a");
            writer.WriteComment("Developed by: Allan Leon");
            Trace.WriteLine("Comment Header written", "LD File");
            #endregion Comment Header

            #region Main Process
            writer.WriteStartElement("Diagram");
            Trace.WriteLine("Diagram started", "DiagramWriter");
            Trace.Indent();

            writer.WriteStartAttribute("Date");
            writer.WriteValue(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            writer.WriteEndAttribute();

            #region Rungs
            writer.WriteStartElement("Rungs");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(diagram.Rungs.Count);
            writer.WriteEndAttribute();

            #region Rung Loop
            foreach (Rung rung in diagram.Rungs)
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
            #endregion Rungs

            #region DataTable
            Trace.WriteLine("Data Table Started", "DiagramWriter");
            Trace.Indent();
            writer.WriteStartElement("DataTable");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(diagram.DataTable.Count);
            writer.WriteEndAttribute();

            #region Variable Loop
            //Write every variable in table to file. 
            //Tag name is the variable type without the "System." prefix
            foreach (var variable in diagram.DataTable.ListAllData())
            {
                string type = variable.Item2.ToString().Replace("System.", string.Empty);

                writer.WriteStartElement(type);

                writer.WriteStartAttribute("Name");
                writer.WriteValue(variable.Item1);
                writer.WriteEndAttribute();

                //So far not needed
                //writer.WriteStartAttribute("Class");
                //writer.WriteValue(variable.Item3.ToString());
                //writer.WriteEndAttribute();

                writer.WriteStartAttribute("Value");
                writer.WriteValue(variable.Item4);
                writer.WriteEndAttribute();

                writer.WriteEndElement();

                Trace.WriteLine("Written: " + type + ", Name=" + variable.Item1 + ", Value=" + variable.Item4, "DataTable");
            }
            #endregion Variable Loop

            writer.WriteEndElement();//Data table end
            Trace.Unindent();
            Trace.WriteLine("Data Table Ended", "DiagramWriter");
            #endregion DataTable

            #region Pin Table
            Trace.WriteLine("Pinout Started", "DiagramWriter");
            Trace.Indent();
            writer.WriteStartElement("Pinout");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(diagram.Pins.Count);
            writer.WriteEndAttribute();

            #region Pin Loop
            //Write every pin in table to file. 
            //Tag name is the Pin type
            foreach (var ldPin in diagram.Pins.OrderBy(x => x.Variable))
            {
                writer.WriteStartElement(ldPin.Type.ToString());

                writer.WriteStartAttribute("Variable");
                writer.WriteValue(ldPin.Variable);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("Pin");
                writer.WriteValue(ldPin.Pin);
                writer.WriteEndAttribute();

                writer.WriteEndElement();

                Trace.WriteLine("Written: " + ldPin.Type + ", Variable=" + ldPin.Variable + ", Pin=" + ldPin.Pin, "Pinout Table");
            }
            #endregion Pin Loop

            writer.WriteEndElement();//Pinout table end
            Trace.Unindent();
            Trace.WriteLine("Pinout Ended", "DiagramWriter");
            #endregion Pin Table

            writer.WriteEndElement(); //Diagram end
            #endregion Main Process

            #region Finish
            writer.WriteEndDocument();
            writer.Close();

            Trace.Unindent();
            Trace.WriteLine("Save process ended successful", "DiagramWriter");
            #endregion Finish
        }

        /// <summary>
        /// Write a simgle component in file
        /// </summary>
        /// <param name="writer">File generator</param>
        /// <param name="component">Component to be writen</param>
        private static void WriteComponent(XmlWriter writer, ComponentBase component)
        {
            string componentTypeName = component.GetType().ToString().Replace("Core.Components.",string.Empty);

            writer.WriteStartElement(componentTypeName);

            if(component is NameableComponent)
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
