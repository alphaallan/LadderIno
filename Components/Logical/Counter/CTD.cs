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
    public class CTD : NameableComponentBase
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
            if (!LastInput && LeftLide.LogicLevel) CurrentValue--;
            LastInput = LeftLide.LogicLevel;
            InternalState = (CurrentValue >= Limit);
        }
        #endregion Functions

        #region Constructors
        public CTD()
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CTD(Node Left, Node Right)
            : base(Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CTD(int startValue, Node Left, Node Right)
            : this(Left, Right)
        {
            CurrentValue = startValue;
        }

        public CTD(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CTD(string name, int startValue, Node Left, Node Right)
            : base(name, Left, Right)
        {
            CurrentValue = startValue;
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }
        #endregion Constructors

        #region Internal Data
        bool LastInput;
        int _Limit;
        int _CurrentValue;
        #endregion Internal Data
    }
}
