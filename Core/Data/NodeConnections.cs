using Core.Components;
using System;
using System.Collections.Generic;

namespace Core.Data
{
    /// <summary>
    /// Store a node and the components connected to it
    /// </summary>
    public class NodeConnections : IEquatable<NodeConnections>
    {
        /// <summary>
        /// Node Reference
        /// </summary>
        public Node Node { get; private set; }

        /// <summary>
        /// Components coming to the node
        /// </summary>
        public List<ComponentBase> InComponents { get; private set; }

        /// <summary>
        /// Components coming from the node 
        /// </summary>
        public List<ComponentBase> OutComponents { get; private set; }

        public NodeConnections(Node node)
        {
            Node = node;
            InComponents = new List<ComponentBase>();
            OutComponents = new List<ComponentBase>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is NodeConnections)) return false;
            return (this.Node == (obj as NodeConnections).Node);
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }

        public bool Equals(NodeConnections other)
        {
            if (other == null) return false;
            return (this.Node == other.Node);
        }
    }

    /// <summary>
    /// Extention Metods to a List<NodeConnections>
    /// </summary>
    public static class NodeConnectionsListExtentions
    {
        /// <summary>
        /// Return the index of a Node in the NodeConnections list
        /// </summary>
        /// <param name="list">List of node Connections</param>
        /// <param name="node">Node to be found</param>
        /// <returns></returns>
        public static int FindNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list.IndexOf(new NodeConnections(node));
        }

        /// <summary>
        /// Return the NodeConnections of a Node in the NodeConnections list
        /// </summary>
        /// <param name="list">List of node Connections</param>
        /// <param name="node">Node to be found</param>
        /// <returns></returns>
        public static NodeConnections GetNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list[list.IndexOf(new NodeConnections(node))];
        }

        /// <summary>
        /// Build the node Connections List from a Rung
        /// </summary>
        /// <param name="list">List of node Connections</param>
        /// <param name="rung">Rung to be analysed</param>
        /// <returns></returns>
        public static List<NodeConnections> RunAnalysis(this List<NodeConnections> list, Rung rung)
        {
            foreach (ComponentBase component in rung.Components)
            {
                NodeConnections NodeA = new NodeConnections(component.LeftLide);
                NodeConnections NodeB = new NodeConnections(component.RightLide);

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
