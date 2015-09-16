using System;

namespace Core.Components
{
    /// <summary>
    /// Component: PWM output
    /// Description: Analog output based on Pulse Width Modulation
    /// Function: Set PWM Duty Cycle in one of the modules avaliable
    /// </summary>
    public class PWM : NameableComponent
    {
        #region Properties
        /// <summary>
        /// Dudy Cycle variable
        /// </summary>
        public string DudyCycle 
        {
            get { return _DudyCycle; }
            set
            {
                if (byte.TryParse(value, out _DudyCycleValue) || string.IsNullOrEmpty(value))
                {
                    if (!byte.TryParse(_DudyCycle, out _DudyCycleValue))
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
                    if (byte.TryParse(_DudyCycle, out _DudyCycleValue))
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

        /// <summary>
        /// DudyCycle current Value
        /// </summary>
        public byte DudyCycleValue
        {
            get { return _DudyCycleValue; }
            set 
            {
                if (byte.TryParse(_DudyCycle, out temp) || string.IsNullOrEmpty(_DudyCycle))
                {
                    _DudyCycle = value.ToString();
                    RaisePropertyChanged("DudyCycle");
                }
                else if (DataTable != null) DataTable.SetValue(_DudyCycle, value);

                _DudyCycleValue = value;
                RaisePropertyChanged("DudyCycleValue");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel)
            {
                if (!byte.TryParse(_DudyCycle, out temp) && !string.IsNullOrEmpty(_DudyCycle))
                {
                    _DudyCycleValue = (byte)((DataTable != null) ? DataTable.GetValue(_DudyCycle) : _DudyCycleValue);
                }
                else _DudyCycleValue = temp;
            }
            
            InternalState = (LeftLide.LogicLevel);
        }

        protected override void DataTableRelease()
        {
            if (DataTable != null)
            {
                base.DataTableRelease();

                try
                {
                    if (DataTable != null) DataTable.Remove(_DudyCycle);
                }
                catch (ArgumentException) { }
            }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();

            if (!byte.TryParse(_DudyCycle, out temp) && !string.IsNullOrEmpty(_DudyCycle))
            {
                if (DataTable != null) DataTable.Add(_DudyCycle, typeof(byte));                
            }
        }
        #endregion Functions

        #region Constructors
        public PWM(string name, Node Left)
            : base(typeof(string), name, Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.PWM;
            VarClass = Data.LDVarClass.PWM;
            DefaultValue = "PWM OUTPUT";
        }

        public PWM(Node Left)
            : base(typeof(string), Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.PWM;
            VarClass = Data.LDVarClass.PWM;
            DefaultValue = "PWM OUTPUT";
        }

        public PWM()
            : base(typeof(string), new Node(), null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.PWM;
            VarClass = Data.LDVarClass.PWM;
            DefaultValue = "PWM OUTPUT";
        }
        #endregion Constructors

        #region Internal Data
        string _DudyCycle;

        byte _DudyCycleValue;
        byte temp;
        #endregion Internal Data
    }
}
