using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Less than or Equal
    /// Description: Less than or equal compare block
    /// Function: True if value A is Less or equal to value B
    /// </summary>
    public class LEG : CompareComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (ValueA <= ValueB);
        }
        #endregion Functions

        #region Constructors
        public LEG()
        {
            
        }

        public LEG(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
