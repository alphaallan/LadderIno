using Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDFile
{
    /// <summary>
    /// Store a node and the components conented to it
    /// </summary>
    class NodeConnections : IEquatable<NodeConnections>
    {
        /// <summary>
        /// Node Reference
        /// </summary>
        public Node Node { get; set; }

        /// <summary>
        /// Components coming to the node
        /// </summary>
        public List<ComponentBase> InComponents { get; set; }

        /// <summary>
        /// Components coming from the node 
        /// </summary>
        public List<ComponentBase> OutComponents { get; set; }

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
    static class NodeConnectionsListExtentions
    {
        public static int FindNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list.IndexOf(new NodeConnections(node));
        }

        public static NodeConnections GetNodeConnections(this List<NodeConnections> list, Node node)
        {
            return list[list.IndexOf(new NodeConnections(node))];
        }
    }
}
