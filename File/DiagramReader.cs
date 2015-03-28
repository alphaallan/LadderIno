using Utilities.Encoders;
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
    public static class DiagramReader
    {
        public static Diagram ReadDiagram(string filePath)
        {

            Trace.WriteLine("Load process started", "LD File");
            Trace.Indent();

            Diagram diagram = new Diagram();

            XmlDocument file = new XmlDocument();
            file.Load(filePath);
            Trace.WriteLine("Document loaded", "LD File");

            XmlNodeList rungs = file.SelectNodes("/Diagram/Rungs/Rung");
            Trace.WriteLine("Rungs Loaded", "LD File");
            XmlNodeList variables = file.SelectSingleNode("/Diagram/DataTable").ChildNodes;
            Trace.WriteLine("Variables Loaded", "LD File");

            foreach (XmlNode xRung in rungs)
            {
                Trace.WriteLine("Rung Started");
                Trace.Indent();

                Rung rung = new Rung();
                rung.Comment = xRung.Attributes["Comment"].Value;
                int componentCount = xRung.Attributes["Count"].Value.ToInt();
                List<ComponentBase> components = new List<ComponentBase>();
                List<List<string>> dRung = new List<List<string>>();//Decomposed stringRung

                #region Decompose Loop
                Trace.WriteLine("Starting decomposition", "Rung");
                Trace.Indent();

                string sRung = xRung.InnerXml; //Rung as string
                
                dRung.Add(new List<string>());

                //Decompose loop
                for (int pos = 0, CurrLevel = 0; pos < sRung.Length; pos++)
                {
                    if (sRung[pos] == '<') 
                    {
                        if ((sRung.Substring(pos + 1, 6) == "Serial") || (sRung.Substring(pos + 1, 8) == "Parallel"))
                        {
                            int lenght = 0;
                            while (sRung[pos + lenght] != '>') lenght++;
                            dRung[CurrLevel].Add(sRung.Substring(pos, lenght + 1) + ((CurrLevel + 2 > dRung.Count) ? "0" : dRung[CurrLevel + 1].Count.ToString()));
                            pos += lenght;

                            CurrLevel++;
                            if (CurrLevel + 1 > dRung.Count) dRung.Add(new List<string>());
                        }
                        else if (sRung[pos + 1] == '/')
                        {
                            int lenght = 0;
                            while (sRung[pos + lenght] != '>') lenght++;
                            dRung[CurrLevel].Add(sRung.Substring(pos, lenght + 1));
                            pos += lenght;

                            CurrLevel--;
                        }
                        else
                        {
                            int lenght = 0;
                            while (sRung[pos + lenght] != '/' && sRung[pos + lenght + 1] != '>') lenght++;

                            components.Add(CreateComponent(sRung.Substring(pos, ++lenght + 1)));
                            dRung[CurrLevel].Add((components.Count - 1).ToString() + ((components.Count == 1) ? "*" : string.Empty));
                            pos += lenght;
                        }
                    }
                }
                Trace.WriteLine("Decomposition ended, rung deepness " + dRung.Count.ToString() + " levels", "Rung");
                Trace.Unindent();
                #endregion Decompose Loop

                #region Rebuild Loop
                Trace.WriteLine("Starting rung rebuild", "Rung");
                Trace.Indent();

                int lastComponent = 0;
                Stack<int> Mode = new Stack<int>();
                Mode.Push(-1);

                rung.Add(components[0]);

                for (int level = 0; level < dRung.Count; level++)
                {
                    for (int pos = 0; pos < dRung[level].Count; pos++ )
                    {
                        if (dRung[level][pos].Contains("<Serial>"))
                        {
                            int slA = level, spA = pos;
                            while (dRung[slA][spA].Contains("<Parallel>") || dRung[slA][spA].Contains("<Serial>"))
                            {
                                spA = dRung[slA][spA].Replace("<Parallel>", string.Empty).Replace("<Serial>", string.Empty).ToInt();
                                slA++;
                            }

                            int slB = level, spB = pos + 1;
                            while (dRung[slB][spB].Contains("<Parallel>") || dRung[slB][spB].Contains("<Serial>"))
                            {
                                spB = dRung[slB][spB].Replace("<Parallel>", string.Empty).Replace("<Serial>", string.Empty).ToInt();
                                slB++;
                            }
                            dRung[slB][spB].ToString();

                            if (!dRung[slA][spA].Contains('*'))
                            {
                                if (Mode.Peek() == -1)
                                {
                                    rung.InsertAfter(components[dRung[slA][spA].ToInt()], components[lastComponent]);
                                }
                                else
                                {
                                    rung.InsertUnder(components[dRung[slA][spA].ToInt()], components[lastComponent]);
                                }

                                lastComponent = dRung[slA][spA].ToInt();
                                dRung[level][pos] += "*";
                            }
                            else lastComponent = dRung[slA][spA].Replace("*", string.Empty).ToInt();

                            if (!dRung[slA][spA].Contains('*')) dRung[slA][spA].ToString();
                            else lastComponent = dRung[slA][spA].Replace("*", string.Empty).ToInt();

                        }
                        else if (dRung[level][pos].Contains("<Parallel>"))
                        {
                            int sl = level, sp = pos;
                            while (dRung[sl][sp].Contains("<Parallel>") || dRung[sl][sp].Contains("<Serial>"))
                            {
                                sp = dRung[sl][sp].Replace("<Parallel>", string.Empty).Replace("<Serial>", string.Empty).ToInt();
                                sl++;
                            }
                            dRung[sl][sp].ToString();

                            int slB = level, spB = pos + 1;
                            while (dRung[slB][spB].Contains("<Parallel>") || dRung[slB][spB].Contains("<Serial>"))
                            {
                                spB = dRung[slB][spB].Replace("<Parallel>", string.Empty).Replace("<Serial>", string.Empty).ToInt();
                                slB++;
                            }
                            dRung[slB][spB].ToString();
                        }
                        else if (dRung[level][pos].Contains(@"</Serial>"))
                        {

                        }
                        else if (dRung[level][pos].Contains(@"</Parallel>"))
                        {

                        }
                        else
                        {
                            if (!dRung[level][pos].Contains('*'))
                            {
                                if (Mode.Peek() == -1)
                                {
                                    rung.InsertAfter(components[dRung[level][pos].ToInt()], components[lastComponent]);
                                }
                                else
                                {
                                    rung.InsertUnder(components[dRung[level][pos].ToInt()], components[lastComponent]);
                                }

                                lastComponent = dRung[level][pos].ToInt();
                                dRung[level][pos] += "*";
                            }
                            else lastComponent = dRung[level][pos].Replace("*", string.Empty).ToInt();
                        }
                    }
                }

                Trace.Unindent();
                #endregion Rebuild Loop

                diagram.Add(rung);
                Trace.Unindent();
            }

            Trace.Unindent();
            Trace.WriteLine("Load process ended successful");

            return diagram;
        }

        private static ComponentBase CreateComponent(string xml)
        {
            XmlDocument temp = new XmlDocument();
            temp.LoadXml(xml);
            return CreateComponent(temp.DocumentElement);
        }

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

                case "ShortCircuit": component = new ShortCircuit(); break;
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
                    adc.InputValue = node.Attributes["InputValue"].Value.ToShort();
                    component = adc; 
                    break;
                case "PWM":
                    PWM pwm = new PWM();
                    pwm.DudyCycle = node.Attributes["DudyCycle"].Value;
                    component = pwm; 
                    break;
                #endregion Analog Components

                default:
                    throw new ArgumentException("Unknow Component", "xml");
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
                (component as MathComponent).VarA = node.Attributes["VarA"].Value;
                (component as MathComponent).VarB = node.Attributes["VarB"].Value;
            }
            #endregion Extra Processing based on Components Base Class

            return component;
        }
    }
}
