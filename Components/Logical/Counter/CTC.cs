using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical.Counter
{
    /// <summary>
    /// Component: Circular Couter
    /// Description: Raising edge counter
    /// Function: Counts plus one for each raising edge detected, once counter is bigger or equals its limit, it will give high output
    /// </summary>
    public class CTC : CounterComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (!LastInput && LeftLide.LogicLevel) CurrentValue++;
            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue == Limit);
            if (CurrentValue > Limit) CurrentValue = 0;
        }
        #endregion Functions

        #region Constructors
        public CTC()
        {
            Class = ComponentClass.Output;
        }

        public CTC(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Output;
        }

        public CTC(int startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
            Class = ComponentClass.Output;
        }

        public CTC(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            Class = ComponentClass.Output;
        }

        public CTC(string name, int startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors
    }
}
