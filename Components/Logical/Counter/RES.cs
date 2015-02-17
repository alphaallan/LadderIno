using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Reset Counter
    /// Description: Resets a counter or timer
    /// Function: Set zero to the control variable of an counter or timer 
    /// </summary>
    public class RES : NameableComponentBase
    {
        #region Properties
        /// <summary>
        /// Internal copy of the counter variable
        /// real set made by external control
        /// </summary>
        public int Counter
        {
            get { return _Counter; }
            set
            {
                _Counter = value;
                RaisePropertyChanged("Counter");
            }
        }

        /// <summary>
        /// Defines if the reset is for a timer or a counter
        /// </summary>
        public ResetType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                NamePerfix = (_Type == ResetType.Counter) ? ComponentPrefix.Conter : ComponentPrefix.Timer ;
                RaisePropertyChanged("Type");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel) Counter = 0;
            InternalState = (LeftLide.LogicLevel);
        }
        #endregion Functions

        #region Constructors
        public RES()
        {
            Class = ComponentClass.Output;
            Type = ResetType.Counter;
        }

        public RES(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Output;
            Type = ResetType.Counter;
        }

        public RES(string name, Node Left)
            : base(name, Left, null)
        {
            Class = ComponentClass.Output;
            Type = ResetType.Counter;
        }
        #endregion Constructors

        #region Internal Data
        int _Counter;
        ResetType _Type;
        #endregion Internal Data

        #region Enum
        public enum ResetType
        {
            Counter,
            Timer
        }
        #endregion Enum
    }
}
