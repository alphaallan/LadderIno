using Core.Data;
using System.Diagnostics;
using System.Xml;

namespace LDFile
{
    public static partial class LDFile
    {
        private static void WriteDataTable(XmlWriter writer, LadderDataTable dataTable)
        {
            Trace.WriteLine("Data Table Started", "DiagramWriter");
            Trace.Indent();
            writer.WriteStartElement("DataTable");
            writer.WriteStartAttribute("Count");
            writer.WriteValue(dataTable.Count);
            writer.WriteEndAttribute();

            #region Variable Loop
            //Write every variable in table to file. 
            //Tag name is the variable type without the "System." prefix
            foreach (var variable in dataTable.ListAllData())
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
        }
    }
}
