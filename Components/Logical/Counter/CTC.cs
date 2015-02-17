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
    public class CTC : NameableComponentBase
    {
        #region Properties
        /// <summary>
        /// Counter maximum value
        /// </summary>
        public int Limit
        {
            get { return _Limit; }
            set
            {
                _Limit = value;
                RaisePropertyChanged("Limit");
            }
        }

        /// <summary>
        /// Counter current value
        /// </summary>
        public int CurrentValue
        {
            get { return _CurrentValue; }
            set
            {
                _CurrentValue = value;
                RaisePropertyChanged("CurrentValue");
            }
        }
        #endregion Properties

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
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Output;
        }

        public CTC(Node Left)
            : base(Left, null)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Output;
        }

        public CTC(int startValue, Node Left)
            : this(Left)
        {
            CurrentValue = startValue;
        }

        public CTC(string name, Node Left)
            : base(name, Left, null)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Output;
        }

        public CTC(string name, int startValue, Node Left)
            : base(name, Left, null)
        {
            CurrentValue = startValue;
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Internal Data
        bool LastInput;
        int _Limit;
        int _CurrentValue;
        #endregion Internal Data
    }
}
