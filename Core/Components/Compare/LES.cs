using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Component: Less than
    /// Description: Less than compare block
    /// Function: True if value A is less than value B
    /// </summary>
    public class LES : CompareComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            RetrieveData();
            InternalState = (ValueA < ValueB);
        }
        #endregion Functions

        #region Constructors
        public LES()
        {
            
        }

        public LES(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
