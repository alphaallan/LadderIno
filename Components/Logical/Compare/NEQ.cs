using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Not Equals
    /// Description: Not Equals compare block
    /// Function: True if value A is different of value B
    /// </summary>
    public class NEQ : CompareCompoentBase
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (ValueA != ValueB);
        }
        #endregion Functions

        #region Constructors
        public NEQ()
        {
            
        }

        public NEQ(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
