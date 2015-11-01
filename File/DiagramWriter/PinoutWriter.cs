using Core.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace LDFile
{
    public static partial class DiagramWriter
    {
        internal static void WritePinTable(IEnumerable<LDPin> pins, XmlWriter writer)
        {
            Trace.WriteLine("Pinout Started", "DiagramWriter");
            Trace.Indent();
            writer.WriteStartElement("Pinout");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(pins.Count());
            writer.WriteEndAttribute();

            #region Pin Loop
            //Write every pin in table to file. 
            //Tag name is the Pin type
            foreach (var ldPin in pins.OrderBy(x => x.Variable))
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
        }
    }
}
