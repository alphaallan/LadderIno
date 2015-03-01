using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public short ReadValue
        {
            get { return _ReadValue; }
            set
            {
                _ReadValue = (short)((value > 1023) ? 1023 : (value < 0) ? 0 : value);
                RaisePropertyChanged("ReadValue");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            ReadValue = (short)((DataTable != null) ? DataTable.GetValue(FullName + "_INPUT") : 0);
            if (DataTable != null) DataTable.SetValue(FullName, ReadValue);
            InternalState = (LeftLide.LogicLevel);
        }

        protected override void NameChangedHandler(string oldName, string newName)
        {
            if (DataTable != null)
            {
                base.NameChangedHandler(oldName, newName);
                try
                {
                    DataTable.Rename(oldName + "_INPUT", newName + "_INPUT");
                }
                catch (ArgumentException ex)
                {
                    if (ex.ParamName == "oldName") DataTable.Add(newName + "_INPUT", typeof(Int16));
                    else throw ex;
                }
            }
        }

        protected override void DataTableRelease()
        {
            base.DataTableRelease();

            try
            {
                if (DataTable != null) DataTable.Remove(FullName + "_INPUT");
            }
            catch (ArgumentException) { }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();

            if (DataTable != null) DataTable.Add(FullName + "_INPUT", typeof(Int16));
        }
        #endregion Functions

        #region Constructors
        public ADC(string name, Node Left)
            : base(typeof(short), name,Left,null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public ADC(Node Left)
            : base(typeof(short), Left, null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }

        public ADC()
            : base(typeof(short), new Node(), null)
        {
            Class = ComponentClass.Output;
            NamePerfix = ComponentPrefix.AnalogInput;
        }
        #endregion Constructors

        #region Internal Data
        short _ReadValue;
        #endregion Internal Data
    }
}
