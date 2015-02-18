using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Base class for counters
    /// </summary>
    public abstract class CounterComponent : NameableComponent
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

        #region Constructors
        public CounterComponent()
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(Node Left, Node Right)
            : base(Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(int startValue, Node Left, Node Right)
            : this(Left, Right)
        {
            CurrentValue = startValue;
        }

        public CounterComponent(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(string name, int startValue, Node Left, Node Right)
            : base(name, Left, Right)
        {
            CurrentValue = startValue;
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }
        #endregion Constructors

        #region Internal Data
        protected bool LastInput;
        int _Limit;
        int _CurrentValue;
        #endregion Internal Data
    }
}
