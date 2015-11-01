using Utilities.Encoders;
using Core.Data;
using Core.Components;
using System;
using System.Linq;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;

namespace LDFile
{
    public static partial class DiagramReader
    {
        /// <summary>
        /// Read rungs from node list
        /// </summary>
        /// <param name="diagram"></param>
        /// <param name="rungs"></param>
        internal static void ReadRungs(XmlNodeList rungs, Diagram diagram)
        {
            foreach (XmlNode xRung in rungs)
            {
                Trace.WriteLine("Rung Started");
                Trace.Indent();

                Rung rung = new Rung();
                rung.Comment = xRung.Attributes["Comment"].Value;
                List<ComponentBase> components = new List<ComponentBase>();
                List<List<string>> dRung = new List<List<string>>();//Decomposed stringRung

                #region Decompose Loop
                Trace.WriteLine("Starting decomposition", "Rung");
                Trace.Indent();

                string sRung = xRung.InnerXml; //Rung as string
                Stack<bool> SerialMode = new Stack<bool>();
                SerialMode.Push(true);

                dRung.Add(new List<string>());

                //Decompose loop
                for (int pos = 0, CurrLevel = 0; pos < sRung.Length; pos++)
                {
                    if (sRung[pos] == '<')
                    {
                        if (sRung.Substring(pos + 1, 6) == "Serial")
                        {
                            dRung[CurrLevel].Add("SS" + ((CurrLevel + 2 > dRung.Count) ? 0 : dRung[CurrLevel + 1].Count));
                            pos += 7;
                            SerialMode.Push(true);

                            CurrLevel++;
                            if (CurrLevel + 1 > dRung.Count) dRung.Add(new List<string>());
                        }
                        else if (sRung.Substring(pos + 1, 8) == "Parallel")
                        {
                            dRung[CurrLevel].Add("SP" + ((CurrLevel + 2 > dRung.Count) ? 0 : dRung[CurrLevel + 1].Count));
                            pos += 9;
                            SerialMode.Push(false);

                            CurrLevel++;
                            if (CurrLevel + 1 > dRung.Count) dRung.Add(new List<string>());
                        }
                        else if (sRung[pos + 1] == '/')
                        {
                            int lenght = 0;
                            while (sRung[pos + lenght] != '>') lenght++;
                            dRung[CurrLevel].Add((sRung.Substring(pos, lenght + 1).Contains('S')) ? "ES" : "EP");
                            pos += lenght;
                            SerialMode.Pop();

                            CurrLevel--;
                        }
                        else
                        {
                            int lenght = 0;
                            while (sRung[pos + lenght] != '/' && sRung[pos + lenght + 1] != '>') lenght++;

                            components.Add(CreateComponent(sRung.Substring(pos, ++lenght + 1)));
                            dRung[CurrLevel].Add(((components.Count == 1) ? "I" : "N") + ((SerialMode.Peek()) ? "S" : "P") + (components.Count - 1).ToString());
                            pos += lenght;
                        }
                    }
                }

                dRung[0].Add("ES");

                Trace.WriteLine("Decomposition ended, rung deepness " + dRung.Count.ToString() + " levels", "Rung");
                Trace.Unindent();
                #endregion Decompose Loop

                if (xRung.Attributes["Count"].Value.ToInt() != components.Count) throw new Exception("Corrupted File. Wrong number of components in a rung");

                #region Rebuild Loop
                Trace.WriteLine("Starting Rung Rebuild", "Rebuild");
                Trace.Indent();

                int lastComponent = 0;
                rung.Add(components[0]);

                for (int level = 0; level < dRung.Count; level++)
                {
                    for (int pos = 0; pos < dRung[level].Count; pos++)
                    {
                        switch (dRung[level][pos][0])
                        {
                            #region Hit Sub-Circuit
                            case 'S':
                                int m = pos;
                                while (dRung[level][m][0] != 'E') m++;

                                if (m > pos + 1)
                                {
                                    int slA = level, //Selected level A
                                    spA = pos;   //Selected Position A

                                    while (dRung[slA][spA][0] == 'S')
                                    {
                                        spA = dRung[slA][spA].Substring(2).ToInt();
                                        slA++;
                                    }

                                    int slB = level,  //Selected level B
                                        spB = pos + 1;//Selected Position B

                                    while (dRung[slB][spB][0] == 'S')
                                    {
                                        spB = dRung[slB][spB].Substring(2).ToInt();
                                        slB++;
                                    }

                                    Trace.WriteLine("Component pair -> " + dRung[slA][spA].Substring(2) + " | " + dRung[slB][spB].Substring(2), "Rebuild");

                                    //if (dRung[slA][spA][0] == 'N')
                                    //{
                                    //    if (dRung[level][m][1] == 'S') rung.InsertAfter(components[dRung[slA][spA].Substring(2).ToInt()], components[lastComponent]);
                                    //    else rung.InsertUnder(components[dRung[slA][spA].Substring(2).ToInt()], components[lastComponent]);
                                    //    dRung[slA][spA] = dRung[slA][spA].Replace("N", "I");
                                    //    Trace.WriteLine("Component A inserted ", "Rebuild");
                                    //}

                                    lastComponent = dRung[slA][spA].Substring(2).ToInt();

                                    if (dRung[slB][spB][0] == 'N')
                                    {
                                        if (dRung[level][m][1] == 'S') rung.InsertAfter(components[dRung[slB][spB].Substring(2).ToInt()], components[lastComponent]);
                                        else rung.InsertUnder(components[dRung[slB][spB].Substring(2).ToInt()], components[lastComponent]);
                                        dRung[slB][spB] = dRung[slB][spB].Replace("N", "I");
                                        Trace.WriteLine("Component " + dRung[slB][spB].Substring(2) + " inserted ", "Rebuild");
                                    }

                                    lastComponent = dRung[slB][spB].Substring(2).ToInt();
                                }
                                else
                                {
                                    int sl = level, //Selected level
                                    sp = pos;   //Selected Position

                                    while (dRung[sl][sp][0] == 'S')
                                    {
                                        sp = dRung[sl][sp].Substring(2).ToInt();
                                        sl++;
                                    }

                                    if (dRung[sl][sp][0] == 'N')
                                    {
                                        if (dRung[level][m][1] == 'S') rung.InsertAfter(components[dRung[sl][sp].Substring(2).ToInt()], components[lastComponent]);
                                        else rung.InsertUnder(components[dRung[sl][sp].Substring(2).ToInt()], components[lastComponent]);
                                        dRung[sl][sp] = dRung[sl][sp].Replace("N", "I");
                                        Trace.WriteLine("Component " + dRung[sl][sp].Substring(2) + " inserted ", "Rebuild");
                                    }
                                }
                                break;
                            #endregion Hit Sub-Circuit

                            #region Hit End of Sub-Circuit
                            case 'E': //End Sub-Circuit
                                Trace.WriteLine("End sub-circuit", "Rebuild");
                                break;
                            #endregion Hit End of Sub-Circuit

                            #region Hit Not Inserted Component
                            case 'N': //New component
                                if (dRung[level][pos][1] == 'S') rung.InsertAfter(components[dRung[level][pos].Substring(2).ToInt()], components[lastComponent]);
                                else rung.InsertUnder(components[dRung[level][pos].Substring(2).ToInt()], components[lastComponent]);

                                dRung[level][pos] = dRung[level][pos].Replace("N", "I");

                                lastComponent = dRung[level][pos].Substring(2).ToInt();

                                Trace.WriteLine("Component " + dRung[level][pos].Substring(2) + " inserted", "Rebuild");
                                break;
                            #endregion Hit Not Inserted Component

                            #region Hit Inserted Component
                            case 'I': //Inserted Component
                                lastComponent = dRung[level][pos].Substring(2).ToInt();
                                Trace.WriteLine("Last component index -> " + lastComponent, "Rebuild");
                                break;
                            #endregion Hit Inserted Component
                        }
                    }
                }

                Trace.Unindent();
                Trace.WriteLine("Rung Rebuild Ended");
                #endregion Rebuild Loop

                diagram.Add(rung);
                Trace.Unindent();
                Trace.WriteLine("Rung Ended");
            }
        }

        /// <summary>
        /// Create a single component from XML string
        /// </summary>
        /// <param name="xml">string containing component XML data</param>
        /// <returns>Created Component</returns>
        private static ComponentBase CreateComponent(string xml)
        {
            XmlDocument temp = new XmlDocument();
            temp.LoadXml(xml);
            return CreateComponent(temp.DocumentElement);
        }

        /// <summary>
        /// Create a single component from XML data
        /// </summary>
        /// <param name="node">XML node containing component data</param>
        /// <returns>Created Component</returns>
        private static ComponentBase CreateComponent(XmlNode node)
        {
            ComponentBase component;

            switch (node.LocalName)
            {
                #region Basic
                case "Coil":
                    Coil coil = new Coil();
                    coil.Mode = node.Attributes["Mode"].Value.ToEnum<Coil.CoilMode>();
                    coil.Type = node.Attributes["Type"].Value.ToEnum<Coil.CoilType>();
                    component = coil;
                    break;

                case "Contact":
                    Contact contact = new Contact();
                    contact.IsClosed = node.Attributes["IsClosed"].Value.ToBool();
                    contact.IsInverted = node.Attributes["IsInverted"].Value.ToBool();
                    contact.Type = node.Attributes["Type"].Value.ToEnum<Contact.ContactType>();
                    component = contact;
                    break;

                case "SC": component = new SC(); break;
                case "OSF": component = new OSF(); break;
                case "OSR": component = new OSR(); break;
                #endregion Basic

                #region Compare Components
                case "EQU": component = new EQU(); break;
                case "GEQ": component = new GEQ(); break;
                case "GRT": component = new GRT(); break;
                case "LEG": component = new LEG(); break;
                case "LES": component = new LES(); break;
                case "NEQ": component = new NEQ(); break;
                #endregion Compare Components

                #region Counter Components
                case "CTC": component = new CTC(); break;
                case "CTD": component = new CTD(); break;
                case "CTU": component = new CTU(); break;
                case "RES": component = new RES(); break;
                #endregion Counter Components

                #region Math Components
                case "ADD": component = new ADD(); break;
                case "DIV": component = new DIV(); break;
                case "MUL": component = new MUL(); break;
                case "SUB": component = new SUB(); break;
                case "MOV": component = new MOV(); break;
                #endregion Math Components

                #region Analog Components
                case "ADC":
                    ADC adc = new ADC();
                    adc.Destination = node.Attributes["Destination"].Value;
                    component = adc;
                    break;
                case "PWM":
                    PWM pwm = new PWM();
                    pwm.DudyCycle = node.Attributes["DudyCycle"].Value;
                    component = pwm;
                    break;
                #endregion Analog Components

                default:
                    throw new ArgumentException("Unknow Component", "node");
            }

            component.Comment = node.Attributes["Comment"].Value;

            #region Extra Processing based on Components Base Class
            if (component is NameableComponent) (component as NameableComponent).Name = node.Attributes["Name"].Value;

            if (component is CompareComponent)
            {
                (component as CompareComponent).VarA = node.Attributes["VarA"].Value;
                (component as CompareComponent).VarB = node.Attributes["VarB"].Value;
            }
            else if (component is CounterComponent)
            {
                (component as CounterComponent).Limit = node.Attributes["Limit"].Value;
            }
            else if (component is MathComponent)
            {
                (component as MathComponent).Destination = node.Attributes["Destination"].Value;
                (component as MathComponent).VarA = node.Attributes["VarA"].Value;
                (component as MathComponent).VarB = node.Attributes["VarB"].Value;
            }
            #endregion Extra Processing based on Components Base Class

            return component;
        }
    }
}
