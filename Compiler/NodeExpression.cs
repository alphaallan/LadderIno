using Utilities.Encoders;
using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;

namespace Compiler
{
    /// <summary>
    /// Store a node connection and its activation expression
    /// </summary>
    class NodeExpression : NodeConnections, IEquatable<NodeExpression>
    {
        /// <summary>
        /// Node logical expression
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Flag to say if logical expression is a temp value
        /// </summary>
        public bool IsTempValue { get; set; }

        public NodeExpression(Node node) : base(node) 
        {
            Expression = string.Empty;
            IsTempValue = false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(NodeExpression other)
        {
            if (other == null) return false;
            return (this.Node == other.Node);
        }

        public static NodeExpression operator +(NodeExpression obj, string subExp)
        {
            if (!string.IsNullOrEmpty(subExp))
            {
                if (!string.IsNullOrEmpty(obj.Expression)) obj.Expression += " | ";
                obj.Expression += "(" + subExp + ")";
                obj.IsTempValue = false;
            }

            return obj;
        }
    }

    /// <summary>
    /// Extention Metods to a List<NodeConnections>
    /// </summary>
    static class NodeExpressionListExtentions
    {
        /// <summary>
        /// Return the index of a Node in the NodeExpression list
        /// </summary>
        /// <param name="list">List of node NodeExpression</param>
        /// <param name="node">Node to be found</param>
        /// <returns></returns>
        public static int FindNodeConnections(this List<NodeExpression> list, Node node)
        {
            return list.IndexOf(new NodeExpression(node));
        }

        /// <summary>
        /// Return the NodeExpression of a Node in the NodeExpression list
        /// </summary>
        /// <param name="list">List of node NodeExpression</param>
        /// <param name="node">Node to be found</param>
        /// <returns></returns>
        public static NodeExpression GetNodeConnections(this List<NodeExpression> list, Node node)
        {
            return list[list.IndexOf(new NodeExpression(node))];
        }

        /// <summary>
        /// Return closer parallel connection in list 
        /// </summary>
        /// <param name="list">List of node NodeExpression</param>
        /// <param name="node">Start node</param>
        /// <returns></returns>
        public static NodeExpression GetLastParallel(this List<NodeExpression> list, NodeExpression node)
        {
            int index = list.IndexOf(node);

            while (list[index].InComponents.Count == 1)
            {
                string exp = list[index].Expression;
                int markerPos = exp.IndexOf(DiagramCompiler.NODE_NUMBER_MARKER);
                if (markerPos == -1) break;

                index = exp.GetInt(markerPos + 1);
            }

            return list[index];
        }

        /// <summary>
        /// Build the node Connections List from a Rung
        /// </summary>
        /// <param name="list">List of node NodeExpression</param>
        /// <param name="rung">Rung to be analysed</param>
        /// <returns></returns>
        public static List<NodeExpression> RunAnalysis(this List<NodeExpression> list, Rung rung)
        {
            foreach (ComponentBase component in rung.Components)
            {
                NodeExpression NodeA = new NodeExpression(component.LeftLide);
                NodeExpression NodeB = new NodeExpression(component.RightLide);

                if (!list.Contains(NodeA))
                {
                    list.Add(NodeA);
                    NodeA.OutComponents.Add(component);
                }
                else
                {
                    int pos = list.IndexOf(NodeA);
                    list[pos].OutComponents.Add(component);
                }

                if (!list.Contains(NodeB))
                {
                    list.Add(NodeB);
                    NodeB.InComponents.Add(component);
                }
                else
                {
                    int pos = list.IndexOf(NodeB);
                    list[pos].InComponents.Add(component);
                }
            }

            return list;
        }
    }
}
