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
                        if (DataTable != null)
                        {
                            try
                            {
                                DataTable.Remove(_DudyCycle);
                            }
                            catch (ArgumentException) { }
                        }
                        
                    }
                }
                else
                {
                    if (byte.TryParse(_DudyCycle, out DudyCycleValue))
                    {
                        if (DataTable != null) DataTable.Add(value, typeof(byte));
                    }
                    else
                    {
                        if (DataTable != null)
                        {
                            try
                            {
                                DataTable.Rename(_DudyCycle, value);
                            }
                            catch (ArgumentException ex)
                            {
                                if (ex.ParamName == "oldName") DataTable.Add(value, typeof(byte));
                                else throw ex;
                            }
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
                DudyCycleValue = (byte)((DataTable != null) ? DataTable.GetValue(_DudyCycle) : 0);
            }

            InternalState = (LeftLide.LogicLevel);
        }

        protected override void DataTableRelease()
        {
            if (DataTable != null)
            {
                DataTable.Remove(_DudyCycle);
            }
        }

        protected override void DataTableAlloc()
        {
            if (!byte.TryParse(_DudyCycle, out DudyCycleValue) && !string.IsNullOrEmpty(_DudyCycle))
            {
                if (DataTable != null) DataTable.Add(_DudyCycle, typeof(byte));                
            }
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

        #region Internal Data
        string _DudyCycle;

        byte DudyCycleValue;
        #endregion Internal Data
    }
}
