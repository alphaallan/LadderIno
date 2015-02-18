using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Greater than or Equal
    /// Description: Greater than or equal compare block
    /// Function: True if value A is Greater or equal to value B
    /// </summary>
    public class GEQ : CompareComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (ValueA >= ValueB);
        }
        #endregion Functions

        #region Constructors
        public GEQ()
        {
            
        }

        public GEQ(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
