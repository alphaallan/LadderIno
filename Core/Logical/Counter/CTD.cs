using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
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
            RetrieveData();
            if (!LastInput && LeftLide.LogicLevel) Data.LDIVariableTable.SetValue(FullName, --CurrentValue);
            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue >= LimitValue);
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

        public CTD(short startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
        }

        public CTD(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
        }

        public CTD(string name, short startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
        }
        #endregion Constructors
    }
}
