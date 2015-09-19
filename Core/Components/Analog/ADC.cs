using System;

namespace Core.Components
{
    /// <summary>
    /// Component: ADC Read
    /// Description: Analog input read command
    /// Function: Execute one read in a selected input
    /// </summary>
    public class ADC : NameableComponent
    {
        #region Properties
        /// <summary>
        /// Destination Variable
        /// </summary>
        public string Destination
        {
            get { return _Destination; }
            set
            {
                if (!string.IsNullOrEmpty(_Destination))
                {
                    try
                    {
                        if (DataTable != null) DataTable.Rename(_Destination, value);
                        _Destination = value;
                    }
                    catch (ArgumentException ex)
                    {
                        if (ex.ParamName == "oldName")
                        {
                            DataTable.Add(value, typeof(short));
                            _Destination = value;
                        }
                        else throw ex;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value) && DataTable != null) DataTable.Add(value, typeof(short));
                    _Destination = value;
                }

                RaisePropertyChanged("Destination");
            }
        }

        /// <summary>
        /// Last value Reded by the ADC
        /// </summary>
        public short ReadValue
        {
            get { return _ReadValue; }
            set
            {
                _ReadValue = (short)((value > 1023) ? 1023 : (value < 0) ? 0 : value);
                if (DataTable != null) DataTable.SetValue(FullName + "_READ", _ReadValue);
                RaisePropertyChanged("ReadValue");
            }
        }

        /// <summary>
        /// Input value in ADC
        /// </summary>
        public short InputValue
        {
            get { return _InputValue; }
            set
            {
                _InputValue = (short)((value > 1023) ? 1023 : (value < 0) ? 0 : value);
                if (DataTable != null) DataTable.SetValue(FullName + "_INPUT", _InputValue);
                RaisePropertyChanged("InputValue");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel)
            {
                _InputValue = (short)((DataTable != null) ? DataTable.GetValue(FullName + "_INPUT") : _InputValue);
                ReadValue = InputValue;
            }
            InternalState = (LeftLide.LogicLevel);
        }

        protected override void NameChangedHandler(string oldName, string newName)
        {
            if (DataTable != null)
            {
                base.NameChangedHandler(oldName, newName);

                try
                {
                    DataTable.Rename(oldName + "_READ", newName + "_READ");
                }
                catch (ArgumentException ex)
                {
                    if (ex.ParamName == "oldName") DataTable.Add(newName + "_READ", typeof(short), Data.LDVarClass.Simulator);
                    else throw ex;
                }

                try
                {
                    DataTable.Rename(oldName + "_INPUT", newName + "_INPUT");
                }
                catch (ArgumentException ex)
                {
                    if (ex.ParamName == "oldName") DataTable.Add(newName + "_INPUT", typeof(Int16), Data.LDVarClass.Simulator);
                    else throw ex;
                }
            }
        }

        protected override void DataTableRelease()
        {
            base.DataTableRelease();

            try
            {
                if (DataTable != null) DataTable.Remove(FullName + "_READ");
            }
            catch (ArgumentException) { }

            try
            {
                if (DataTable != null) DataTable.Remove(FullName + "_INPUT");
            }
            catch (ArgumentException) { }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();

            if (DataTable != null)
            {
                DataTable.Add(FullName + "_READ", typeof(short), Data.LDVarClass.Simulator);
                DataTable.Add(FullName + "_INPUT", typeof(Int16), Data.LDVarClass.Simulator);
            }
        }
        #endregion Functions

        #region Constructors
        public ADC(string name, Node Left)
            : base(typeof(string), name,Left,null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
            VarClass = Data.LDVarClass.Analog;
            DefaultValue = "ADC INPUT";
        }

        public ADC(Node Left)
            : base(typeof(string), Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
            VarClass = Data.LDVarClass.Analog;
            DefaultValue = "ADC INPUT";
        }

        public ADC()
            : base(typeof(string), new Node(), null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
            VarClass = Data.LDVarClass.Analog;
            DefaultValue = "ADC INPUT";
        }
        #endregion Constructors

        #region Internal Data
        string _Destination;
        short _ReadValue;
        short _InputValue;
        #endregion Internal Data
    }
}
