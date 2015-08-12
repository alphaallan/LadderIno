
namespace Core.Components
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
            if (!LastInput && LeftLide.LogicLevel)
            {
                RetrieveData();
                CurrentValue++;
            }
            LastInput = LeftLide.LogicLevel;
            InternalState = (LeftLide.LogicLevel && (CurrentValue >= LimitValue));
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

        public CTU(short startValue, Node Left, Node Right)
            : base(startValue, Left, Right)
        {
        }

        public CTU(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
        }

        public CTU(string name, short startValue, Node Left, Node Right)
            : base(name, startValue, Left, Right)
        {
        }
        #endregion Constructors
    }
}
