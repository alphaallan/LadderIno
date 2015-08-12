using System;

namespace Core.Components
{
    /// <summary>
    /// Base classe for compare operations
    /// </summary>
    public abstract class CompareComponent : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Variable A
        /// </summary>
        public string VarA
        {
            get { return _VarA; }
            set
            {
                ParameterChangedHandler(VarA, value);
                _VarA = (string.IsNullOrEmpty(value)) ? "0" : value;
                RaisePropertyChanged("VarA");
            }
        }

        /// <summary>
        /// Variable A current value
        /// </summary>
        public short ValueA
        {
            get { return _ValueA; }
            set
            {
                if (short.TryParse(_VarA, out _ValueA) || string.IsNullOrEmpty(_VarA))
                {
                    _VarA = value.ToString();
                    RaisePropertyChanged("VarA");
                }
                else if (DataTable != null) DataTable.SetValue(_VarA, value);

                _ValueA = value;
                RaisePropertyChanged("ValueA");
            }
        }

        /// <summary>
        /// Variable B (can be a constant)
        /// </summary>
        public string VarB
        {
            get { return _VarB; }
            set
            {
                ParameterChangedHandler(VarB, value);
                _VarB = (string.IsNullOrEmpty(value)) ? "0" : value;
                RaisePropertyChanged("VarB");
            }
        }

        /// <summary>
        /// Variable B current value
        /// </summary>
        public short ValueB
        {
            get { return _ValueB; }
            set
            {
                if (short.TryParse(_VarB, out _ValueB) || string.IsNullOrEmpty(_VarA))
                {
                    _VarB = value.ToString();
                    RaisePropertyChanged("VarB");
                }
                else if (DataTable != null) DataTable.SetValue(_VarB, value);

                _ValueB = value;
                RaisePropertyChanged("ValueB");
            }
        }
        #endregion Properties

        #region Functions
        protected void RetrieveData()
        {
            if (!short.TryParse(_VarA, out _ValueA) && !string.IsNullOrEmpty(_VarA))
            {
                _ValueA = (short)((DataTable != null) ? DataTable.GetValue(_VarA) : 0);
            }

            if (!short.TryParse(_VarB, out _ValueB) && !string.IsNullOrEmpty(_VarB))
            {
                _ValueB = (short)((DataTable != null) ? DataTable.GetValue(_VarB) : 0);
            }
        }

        protected virtual void ParameterChangedHandler(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) oldName = "0";
            if (string.IsNullOrEmpty(newName)) newName = "0";

            if (short.TryParse(oldName, out _ValueA))
            {
                if (!short.TryParse(newName, out _ValueA) && DataTable != null) DataTable.Add(newName, typeof(short));
            }
            else
            {
                if (DataTable != null)
                {
                    if (!short.TryParse(newName, out _ValueA))
                    {
                        try
                        {
                            DataTable.Rename(oldName, newName);
                        }
                        catch (ArgumentException ex)
                        {
                            if (ex.ParamName == "oldName") DataTable.Add(newName, typeof(short));
                            else throw ex;
                        }
                    }
                    else DataTable.Remove(oldName);
                }
            }
            
        }

        protected override void DataTableRelease()
        {
            if (DataTable != null)
            {
                try
                {
                    DataTable.Remove(_VarA);
                }
                catch (ArgumentException) { }

                try
                {
                    DataTable.Remove(_VarB);
                }
                catch (ArgumentException) { }
            }
        }

        protected override void DataTableAlloc()
        {
            if (DataTable != null)
            {
                if (!short.TryParse(_VarA, out _ValueA) && !string.IsNullOrEmpty(_VarA)) DataTable.Add(_VarA, typeof(short));

                if (!short.TryParse(_VarB, out _ValueB) && !string.IsNullOrEmpty(_VarB)) DataTable.Add(_VarB, typeof(short));
                
            }
        }
        #endregion Functions

        #region Constructors
        public CompareComponent()
        {
            Class = ComponentClass.Input;
        }

        public CompareComponent(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Input;
        }
        #endregion Constructors

        #region Internal Data
        private string _VarA;
        private string _VarB;

        private short _ValueA;
        private short _ValueB;
        #endregion Internal Data
    }
}
