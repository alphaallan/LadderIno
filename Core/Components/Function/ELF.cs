using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Components
{
    /// <summary>
    /// End-Line Function
    /// </summary>
    public class ELF : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Function Name
        /// Used by compiler as identifier
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (DataTable != null)
                {
                    try
                    {
                        DataTable.Rename(_Name, value);
                    }
                    catch (ArgumentException ex)
                    {
                        if (ex.ParamName == "oldName") DataTable.Add(value, typeof(string), Data.LDVarClass.OutFunction);
                        else throw ex;
                    }
                }

                _Name = value;

                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Function inner code
        /// </summary>
        public string Code
        {
            get 
            { 
                _Code = (string)((DataTable != null) ? DataTable.GetValue(Name) : _Code);
                return _Code; 
            }
            set
            {
                _Code = value;
                if (DataTable != null) DataTable.SetValue(Name, _Code);
                RaisePropertyChanged("Code");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (LeftLide.LogicLevel);
        }

        protected override void DataTableRelease()
        {
            base.DataTableRelease();

            try
            {
                if (DataTable != null) DataTable.Remove(Name);
            }
            catch (ArgumentException) { }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();

            if (DataTable != null)
            {
                DataTable.Add(Name, typeof(string), Data.LDVarClass.OutFunction, _Code);
            }
        }
        #endregion Functions

        #region Constructors
        public ELF() 
            : base()
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Internal Data
        string _Code;
        string _Name;
        #endregion Internal Data
    }
}
