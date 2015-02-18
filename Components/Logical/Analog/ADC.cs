using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: ADC Read
    /// Description: Analog input read command
    /// Function: Execute one read in a selected input
    /// </summary>
    public class ADC : NameableComponent
    {
        #region Properties
        public int ReadValue
        {
            get { return _ReadValue; }
            set
            {
                _ReadValue = (value > 1023) ? 1023 : (value < 0) ? 0 : value;
                RaisePropertyChanged("ReadValue");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (LeftLide.LogicLevel);
        }
        #endregion Functions

        #region Constructors
        public ADC(string name, Node Left)
            : base(name,Left,null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public ADC(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public ADC()
            : base(new Node(), null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }
        #endregion Constructors

        #region Internal Data
        int _ReadValue;
        #endregion Internal Data
    }
}
