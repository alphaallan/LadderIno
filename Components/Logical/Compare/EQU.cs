using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Equals
    /// Description: Equals compare block
    /// Function: True if value A is equals to value B
    /// </summary>
    public class EQU : CompareCompoentBase
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (ValueA == ValueB);
        }
        #endregion Functions

        #region Constructors
        public EQU()
        {
            
        }

        public EQU(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
