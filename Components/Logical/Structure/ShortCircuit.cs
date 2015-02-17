using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Auxiliar structure component
    /// </summary>
    public class ShortCircuit : ComponentBase
    {

        protected override void RunLogicalTest()
        {
            InternalState = LeftLide.LogicLevel;
        }

        public ShortCircuit(Node Left, Node Right) 
            : base(Left, Right)
        {
            Class = ComponentClass.Input;
        }

        public ShortCircuit(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Input;
        }
    }
}
