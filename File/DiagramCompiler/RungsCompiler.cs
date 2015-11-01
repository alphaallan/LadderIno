using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace LDFile
{
    public static partial class DiagramCompiler
    {
        internal static void CompileRungs(IEnumerable<Rung> rungs, CompilerBuffer codeBuffer)
        {
            XmlNodeList xRungs = SerializeRungs(rungs);

            foreach (XmlNode rung in xRungs)
            {

            }

            codeBuffer.BoolTempCount = 1;
            codeBuffer.OSRCount = 2;

            codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X1;", "Y1 = (((" + OSR_FN + "(0, " + RATD + "[0]) && !Y2) || Y1) && !X2);" });
            codeBuffer.Rungs.Add(new List<string> { RATD + "[0] = X2;", "Y2 = (((" + OSR_FN + "(1, " + RATD + "[0]) && !Y1) || Y2) && !X1);" });
        }

        private static XmlNodeList SerializeRungs(IEnumerable<Rung> rungs)
        {
            XmlDocument doc = new XmlDocument();
            StringWriter textWriter = new StringWriter();

            XmlWriter writer = XmlWriter.Create(textWriter);

            writer.WriteStartDocument();
            DiagramWriter.WriteRungs(rungs, writer);
            writer.WriteEndDocument();
            writer.Close();

            doc.LoadXml(textWriter.ToString());
            return doc.LastChild.ChildNodes;
        }
    }
}
