using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Greater than
    /// Description: Greater than compare block
    /// Function: True if value A is greater than value B
    /// </summary>
    public class GRT : CompareComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (ValueA > ValueB);
        }
        #endregion Functions

        #region Constructors
        public GRT()
        {
            
        }

        public GRT(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
