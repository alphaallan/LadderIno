using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Count Donw
    /// Description: Raising edge counter
    /// Function: Counts minus one for each raising edge detected, while counter is bigger or equals its limit, it will give high output
    /// </summary>
    public class CTD : CounterComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (!LastInput && LeftLide.LogicLevel) CurrentValue--;
            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue >= Limit);
        }
        #endregion Functions

        #region Constructors
        public CTD()
        {
        }

        public CTD(Node Left, Node Right)
            : base(Left, Right)
        {
        }

        public CTD(int startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
        }

        public CTD(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
        }

        public CTD(string name, int startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
        }
        #endregion Constructors
    }
}
