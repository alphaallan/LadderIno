using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
{
    /// <summary>
    /// Component: PWM output
    /// Description: Analog output based on Pulse Width Modulation
    /// Function: Set PWM Duty Cycle in one of the modules avaliable
    /// </summary>
    public class PWM : ComponentBase
    {
        #region Properties
        public string DudyCycle 
        {
            get { return _DudyCycle; }
            set
            {
                if (byte.TryParse(value, out DudyCycleValue) || string.IsNullOrEmpty(value))
                {
                    if (!byte.TryParse(_DudyCycle, out DudyCycleValue))
                    {
                        try
                        {
                            Data.LDIVariableTable.Remove(_DudyCycle);
                        }
                        catch (ArgumentException) {  }
                    }
                }
                else
                {
                    if (byte.TryParse(_DudyCycle, out DudyCycleValue))
                    {
                        Data.LDIVariableTable.Add(value, typeof(byte));
                    }
                    else
                    {
                        try
                        {
                            Data.LDIVariableTable.Rename(_DudyCycle, value);
                        }
                        catch (ArgumentException ex)
                        {
                            if (ex.ParamName == "oldName") Data.LDIVariableTable.Add(value, typeof(byte));
                            else throw ex;
                        }
                    }
                }
                _DudyCycle = value;
                RaisePropertyChanged("DudyCycle");
            } 
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            if (!byte.TryParse(_DudyCycle, out DudyCycleValue) && !string.IsNullOrEmpty(_DudyCycle))
            {
                DudyCycleValue = (byte)Data.LDIVariableTable.GetValue(_DudyCycle);
            }

            InternalState = (LeftLide.LogicLevel);
        }
        #endregion Functions

        #region Constructors
        public PWM(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Output;
        }

        public PWM()
            : base(new Node(), null)
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Destructor
        ~PWM()
        {
            try
            {
                Data.LDIVariableTable.Remove(_DudyCycle);
            }
            catch (ArgumentException) { }
        }
        #endregion Destructor

        #region Internal Data
        string _DudyCycle;

        byte DudyCycleValue;
        #endregion Internal Data
    }
}
