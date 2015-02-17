using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: PWM output
    /// Description: Analog output based on Pulse Width Modulation
    /// Function: Set PWM Duty Cycle in one of the modules avaliable
    /// </summary>
    public class PWM : NameableComponentBase
    {
        #region Properties
        public byte DudyCycle 
        {
            get { return _DudyCycle; }
            set
            {
                _DudyCycle = value;
                RaisePropertyChanged("DudyCycle");
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
        public PWM(string name, Node Left)
            : base(name,Left,null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public PWM(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public PWM()
            : base(new Node(), null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }
        #endregion Constructors

        #region Internal Data
        byte _DudyCycle;
        #endregion Internal Data
    }
}
