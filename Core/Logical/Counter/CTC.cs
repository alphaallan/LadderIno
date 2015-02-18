using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical.Counter
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
            RetrieveData();
            if (!LastInput && LeftLide.LogicLevel) Data.LDIVariableTable.SetValue(FullName, (++CurrentValue > LimitValue) ? (short)0 : CurrentValue);

            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue == LimitValue);
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

        public CTC(short startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
            Class = ComponentClass.Output;
        }

        public CTC(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            Class = ComponentClass.Output;
        }

        public CTC(string name, short startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors
    }
}
