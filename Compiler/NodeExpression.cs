using Core.Components;
using Core.Data;
using System;
using System.Collections.Generic;

namespace Compiler
{
    class NodeExpression : NodeConnections, IEquatable<NodeExpression>, IEquatable<NodeConnections>
    {
        public string Expression { get; set; }

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
            obj.Expression += " | (" + subExp + ")";
            obj.IsTempValue = false;

            return obj;
        }
    }


    static class NodeExpressionListExtentions
    {
        public static int FindNodeConnections(this List<NodeExpression> list, Node node)
        {
            return list.IndexOf(new NodeExpression(node));
        }

        public static NodeExpression GetNodeConnections(this List<NodeExpression> list, Node node)
        {
            return list[list.IndexOf(new NodeExpression(node))];
        }

        public static List<NodeExpression> RunAnalisys(this List<NodeExpression> list, Rung rung)
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
