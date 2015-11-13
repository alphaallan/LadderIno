using Utilities.Encoders;
using Core.Components;
using System.Diagnostics;
using System.Xml;
using System;

namespace LDFile
{
    public static partial class DiagramReader
    {
        internal static void ReadDataTable(XmlNodeList variables, Diagram diagram)
        {
            Trace.WriteLine("Data Load Started");
            Trace.Indent();
            foreach (XmlNode xVar in variables)
            {
                switch (xVar.LocalName)
                {
                    case "Boolean":
                        diagram.DataTable.SetValue(xVar.Attributes["Name"].Value, xVar.Attributes["Value"].Value.ToBool());
                        break;

                    case "Int16":
                        diagram.DataTable.SetValue(xVar.Attributes["Name"].Value, xVar.Attributes["Value"].Value.ToShort());
                        break;

                    case "Byte":
                        diagram.DataTable.SetValue(xVar.Attributes["Name"].Value, xVar.Attributes["Value"].Value.ToByte());
                        break;

                    case "String":
                        diagram.DataTable.SetValue(xVar.Attributes["Name"].Value, xVar.InnerText);
                        break;

                    default:
                        throw new FormatException("Corrupted File. Unrecognized variable type");
                }
            }
            Trace.Unindent();
            Trace.WriteLine("Data Load Ended");
        }
    }
}
