using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Count Up
    /// Description: Raising edge counter
    /// Function: Counts plus one for each raising edge detected, once counter is bigger or equals its limit, it will give high output
    /// </summary>
    public class CTU : CounterComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (!LastInput && LeftLide.LogicLevel) CurrentValue++;
            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue >= Limit);
        }
        #endregion Functions

        #region Constructors
        public CTU()
        {
        }

        public CTU(Node Left, Node Right)
            : base(Left, Right)
        {
        }

        public CTU(int startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
        }

        public CTU(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
        }

        public CTU(string name, int startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
        }
        #endregion Constructors
    }
}
