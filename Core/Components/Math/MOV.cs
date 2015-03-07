using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Component: MOV
    /// Description: Move value function block
    /// Function: moves a value to the destination variable
    /// Obs: Value B is not used by this component
    /// </summary>
    public class MOV : MathComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel)
            {
                RetrieveData();
                if (DataTable != null) DataTable.SetValue(Destination, ValueA);
            }
            InternalState = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public MOV()
        {
            
        }

        public MOV(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
