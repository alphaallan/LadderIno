using Core.Components;
using System;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace LDFile
{
    public static partial class DiagramWriter
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

            Trace.WriteLine("Save process started", "LD File");
            Trace.Indent();

            FileStream stream = new FileStream(filePath, FileMode.Create);
            Trace.WriteLine("File stream started", "LD File");
            
            SerializeDiagram(diagram).Save(stream);
            
            stream.Close();
            Trace.WriteLine("File stream closed", "LD File");

            Trace.Unindent();
            Trace.WriteLine("Save process ended successful", "DiagramWriter");
        }

        /// <summary>
        /// Serialize a LD Diagram
        /// </summary>
        /// <param name="diagram">Diagram to be serialized</param>
        /// <returns>Serialized Diagram</returns>
        public static XmlDocument SerializeDiagram(Diagram diagram)
        {
            if (diagram == null) throw new ArgumentNullException("diagram", "Null Diagram");

            #region Stratup
            Trace.WriteLine("Serialize process started", "LD File");
            Trace.Indent();

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.NewLineOnAttributes = false;
            xmlWriterSettings.Indent = true;

            StringWriter textWriter = new StringWriter();

            XmlWriter writer = XmlWriter.Create(textWriter, xmlWriterSettings);

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

            WriteRungs(writer, diagram.Rungs);
            WriteDataTable(writer, diagram.DataTable);
            WritePinTable(writer, diagram.Pins);

            writer.WriteEndElement(); //Diagram end
            #endregion Main Process

            #region Finish
            writer.WriteEndDocument();
            writer.Close();

            Trace.Unindent();
            Trace.WriteLine("Serialize process ended successful", "DiagramWriter");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(textWriter.ToString());
            return doc;
            #endregion Finish
        }
    }
}
