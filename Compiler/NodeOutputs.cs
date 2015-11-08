using Core.Components;
using System;
using System.Collections.Generic;

namespace Compiler
{
    /// <summary>
    /// Store a output statments bundle
    /// </summary>
    class NodeOutputs : IEquatable<NodeOutputs>
    {
        /// <summary>
        /// Bundle input node
        /// </summary>
        public Node Node { get; private set; }

        /// <summary>
        /// Bundle if statements
        /// </summary>
        public List<string> IfStatements { get; private set; }

        /// <summary>
        /// Bundle else statements
        /// </summary>
        public List<string> ElseStatements { get; private set; }

        public NodeOutputs(Node node)
        {
            Node = node;
            IfStatements = new List<string>();
            ElseStatements = new List<string>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is NodeOutputs)) return false;
            return (this.Node == (obj as NodeOutputs).Node);
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }

        public bool Equals(NodeOutputs other)
        {
            if (other == null) return false;
            return (this.Node == other.Node);
        }
    }
}
