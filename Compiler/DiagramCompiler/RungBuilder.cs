using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Build rung instructions from semi-compiled parts
        /// </summary>
        /// <param name="tempStatements">Temp variables statements</param>
        /// <param name="nodes">NodeExpression collection</param>
        /// <param name="outputs">NodeOutputs collection</param>
        /// <returns></returns>
        internal static List<string> BuildRung(List<string> tempStatements, List<NodeExpression> nodes, List<NodeOutputs> outputs)
        {
            List<string> buffer = new List<string>();

            foreach (string item in tempStatements) buffer.Add(item.BuildExpression(nodes)); 

            foreach(NodeOutputs item in outputs)
            {
                buffer.Add("if " + nodes.GetNodeConnections(item.Node).Expression.BuildExpression(nodes));
                buffer.Add("{");
                foreach (string statement in item.IfStatements) buffer.Add(INDENT + statement);
                buffer.Add("}");
                buffer.Add("else");
                buffer.Add("{");
                foreach (string statement in item.ElseStatements) buffer.Add(INDENT + statement);
                buffer.Add("}");
            }


            return buffer;
        }

        /// <summary>
        /// Build the activation expression from a semi-compiled expression
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        internal static string BuildExpression(this string input, List<NodeExpression> nodes)
        {
            string buffer = input;
            int markerPos = buffer.IndexOf(NODE_NUMBER_MARKER);

            while (markerPos != -1)
            {
                int numberLeght = 1;
                while ((markerPos + numberLeght <= buffer.Length) && char.IsDigit(buffer[markerPos + numberLeght])) numberLeght++;
                string subExp = buffer.Substring(markerPos, numberLeght);
                int nodeIndex = Int16.Parse(subExp.Substring(1));

                buffer = buffer.Replace(subExp, nodes[nodeIndex].Expression);

                markerPos = buffer.IndexOf(NODE_NUMBER_MARKER);
            }

            return buffer;
        }
    }
}
