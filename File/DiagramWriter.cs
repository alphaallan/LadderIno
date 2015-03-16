using Core.Data;
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
    public static class DiagramWriter
    {
        public static void WriteDiagram(Diagram diagram, string filePath)
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
            writer.WriteComment("This files has created by DiagramWriter V. 0.1a");
            writer.WriteComment("Developed by: Allan Leon");
            Trace.WriteLine("Comment Header written", "LD File");
            #endregion Comment Header

            #region Main Process
            writer.WriteStartElement("Diagram");
            Trace.WriteLine("Diagram started");
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
                Trace.WriteLine("Rung Started");
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

                writer.WriteEndElement();
                Trace.WriteLine("Rung Ended");
                Trace.Unindent();
            }
            #endregion Rung Loop

            writer.WriteEndElement();
            #endregion Rungs

            #region DataTable
            Trace.WriteLine("Data Table Started");
            Trace.Indent();
            writer.WriteStartElement("DataTable");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(diagram.DataTable.Count);
            writer.WriteEndAttribute();

            #region Variable Loop
            //Write every variable in table to file. 
            //Tag name is the variable type without the "System." prefix
            for (int c = 0; c < diagram.DataTable.Count; c++)
            {
                object value = diagram.DataTable.GetValue(c);
                string name = diagram.DataTable.GetName(c);
                string type = value.GetType().ToString().Replace("System.",string.Empty);

                writer.WriteStartElement(type);

                writer.WriteStartAttribute("Name");
                writer.WriteValue(name);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("Value");
                writer.WriteValue(value);
                writer.WriteEndAttribute();

                writer.WriteEndElement();

                Trace.WriteLine("Written: " + type + ", Name=" + name + ", Value=" + value, "DataTable");
            }
            #endregion Variable Loop

            writer.WriteEndElement();//Data table end
            #endregion DataTable

            writer.WriteEndElement(); //Diagram end
            Trace.Unindent();
            Trace.WriteLine("Data Table Ended");
            #endregion Main Process

            #region Finish
            writer.WriteEndDocument();
            writer.Close();

            Trace.Unindent();
            Trace.WriteLine("Save process ended successful");
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

                #region Compare Components
                case "EQU":
                case "GEQ":
                case "GRT":
                case "LEG":
                case "LES":
                case "NEG":
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
                    writer.WriteStartAttribute("InputValue");
                    writer.WriteValue((component as ADC).InputValue);
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
                case "ShortCircuit":
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

    /// <summary>
    /// Store a node and the components conented to it
    /// </summary>
    class NodeConnections : IEquatable<NodeConnections>
    {
        /// <summary>
        /// Node Reference
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Components coming to the node
        /// </summary>
        public List<ComponentBase> InComponents { get; set; }

        /// <summary>
        /// Components coming from the node 
        /// </summary>
        public List<ComponentBase> OutComponents { get; set; }

        public NodeConnections(Node node)
        {
            Node = node;
            InComponents = new List<ComponentBase>();
            OutComponents = new List<ComponentBase>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is NodeConnections)) return false;
            return (this.Node == (obj as NodeConnections).Node);
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }

        public bool Equals(NodeConnections other)
        {
            if (other == null) return false;
            return (this.Node == other.Node);
        }
    }

    /// <summary>
    /// Extention Metods to a List<NodeConnections>
    /// </summary>
    static class NodeConnectionsListExtentions
    {
        public static int FindNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list.IndexOf(new NodeConnections(node));
        }

        public static NodeConnections GetNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list[list.IndexOf(new NodeConnections(node))];
        }
    }
}
