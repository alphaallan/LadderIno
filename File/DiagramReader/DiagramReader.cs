using Core.Components;
using System;
using System.Xml;
using System.Diagnostics;

namespace LDFile
{
    public static partial class DiagramReader
    {
        /// <summary>
        /// Load a LD diagram from file
        /// </summary>
        /// <param name="filePath">File to load From</param>
        /// <returns>Loaded Diagram</returns>
        public static Diagram LoadDiagram(string filePath)
        {
            Trace.WriteLine("Load process started", "LD File");
            Trace.Indent();

            Diagram diagram = new Diagram();

            #region File Load
            XmlDocument file = new XmlDocument();
            file.Load(filePath);
            Trace.WriteLine("Document loaded", "LD File");

            XmlNodeList rungs = file.SelectNodes("/Diagram/Rungs/Rung");
            Trace.WriteLine("Rungs Loaded", "LD File");

            XmlNodeList variables = file.SelectSingleNode("/Diagram/DataTable").ChildNodes;
            Trace.WriteLine("Variables Loaded", "LD File");

            XmlNodeList pins = file.SelectSingleNode("/Diagram/Pinout").ChildNodes;
            Trace.WriteLine("Pinout Loaded", "LD File");
            #endregion File Load

            #region Diagram Load
            try
            {
                ReadRungs(rungs, diagram);
                ReadDataTable(variables, diagram);
                ReadPinout(pins, diagram);
            }
            catch (Exception ex)
            {
                throw new System.IO.FileLoadException("Fail to load file", filePath, ex);
            }
            #endregion Diagram Load

            Trace.Unindent();
            Trace.WriteLine("Load process ended successful");

            return diagram;
        }
    }
}
