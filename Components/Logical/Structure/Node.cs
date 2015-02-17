using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
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

        public Node()
        {

        }

        public Node(ComponentBase root)
        {
            Root = root;
        }
    }
}
