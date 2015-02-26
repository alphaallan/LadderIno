using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Circuit node
    /// Used as a point of connection between two or more components,
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Node’s root component.
        /// Used for simulation purposes in order to avoid logical level involuntary resets
        /// </summary>
        public ComponentBase Root { get; set; }

        /// <summary>
        /// Node’s current logical level 
        /// </summary>
        public bool LogicLevel { get; set; }

        /// <summary>
        /// Default builder
        /// </summary>
        public Node()
        {

        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="root">Owner component</param>
        public Node(ComponentBase root)
        {
            Root = root;
        }
    }
}
