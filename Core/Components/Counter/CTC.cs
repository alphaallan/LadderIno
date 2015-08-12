
namespace Core.Components
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
            if (!LastInput && LeftLide.LogicLevel)
            {
                RetrieveData();
                CurrentValue = (short)((CurrentValue >= LimitValue) ? 0 : CurrentValue + 1);
            }

            LastInput = LeftLide.LogicLevel;
            InternalState = (LeftLide.LogicLevel && (CurrentValue == LimitValue));
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
