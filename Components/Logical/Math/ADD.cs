using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: ADD
    /// Description: Integer 16 bits sum function block
    /// Function: put on destination variable the sum of value A and value B
    /// </summary>
    public class ADD : MathComponentBase
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel) Destination = (short)(ValueA + ValueB);
            InternalState = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public ADD()
        {
            
        }

        public ADD(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
