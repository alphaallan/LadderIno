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
    class CTU : NameableComponentBase
    {
        #region Properties
        /// <summary>
        /// Counter limit value
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
            : this(Left, Right)
        {
            CurrentValue = startValue;
        }

        public CTU(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {

        }

        public CTU(string name, int startValue, Node Left, Node Right)
            : base(name, Left, Right)
        {
            CurrentValue = startValue;
        }
        #endregion Constructors

        #region Internal Data
        bool LastInput;
        int _Limit;
        int _CurrentValue;
        #endregion Internal Data
    }
}
