using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Component: One Shot Falling
    /// Description: Border detector from High to Low
    /// Function: Detects one falling border on input
    /// </summary>
    public class OSF : ComponentBase
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = LastInput && !LeftLide.LogicLevel;
            LastInput = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public OSF()
        {
            Class = ComponentClass.Input;
        }

        public OSF(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Input;
        }
        #endregion Constructors

        #region Internal Data
        bool LastInput;
        #endregion Internal Data
    }
}
