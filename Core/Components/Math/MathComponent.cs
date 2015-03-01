using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Math components base
    /// </summary>
    public abstract class MathComponent : ComponentBase
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

                RaisePropertyChanged("Destination");
            }
        }

        /// <summary>
        /// Variable A
        /// </summary>
        public string NameA
        {
            get { return _NameA; }
            set
            {
                ParameterChangedHandler(NameA, value);
                _NameA = (string.IsNullOrEmpty(value)) ? "0" : value;
                RaisePropertyChanged("NameA");
            }
        }

        /// <summary>
        /// Variable B (can be a constant)
        /// </summary>
        public string NameB
        {
            get { return _NameB; }
            set
            {
                ParameterChangedHandler(NameB, value);
                _NameB = (string.IsNullOrEmpty(value)) ? "0" : value;
                RaisePropertyChanged("NameB");
            }
        }
        #endregion Properties
        
        #region Functions
        protected void RetrieveData()
        {
            if (!short.TryParse(_NameA, out _ValueA) && !string.IsNullOrEmpty(_NameA) && DataTable != null)
            {
                _ValueA = (short)DataTable.GetValue(_NameA);
            }

            if (!short.TryParse(_NameB, out _ValueB) && !string.IsNullOrEmpty(_NameB) && DataTable != null)
            {
                _ValueB = (short)DataTable.GetValue(_NameB);
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
            else if (DataTable != null) 
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

        protected override void DataTableRelease()
        {
            base.DataTableRelease();

            if (DataTable != null)
            {
                try
                {
                    DataTable.Remove(_Destination);
                }
                catch (ArgumentException) { }

                try
                {
                    DataTable.Remove(_NameA);
                }
                catch (ArgumentException) { }

                try
                {
                    DataTable.Remove(_NameB);
                }
                catch (ArgumentException) { }
            }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();
            if (DataTable != null)
            {
                DataTable.Add(_Destination, typeof(short));

                if (!short.TryParse(_NameA, out _ValueA) && !string.IsNullOrEmpty(_NameA)) DataTable.Add(_NameA, typeof(short));
                if (!short.TryParse(_NameB, out _ValueB) && !string.IsNullOrEmpty(_NameB)) DataTable.Add(_NameB, typeof(short));
            }
        }
        #endregion Functions

        #region Constructors
        public MathComponent()
        {
            Class = ComponentClass.Output;
        }

        public MathComponent(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Internal Data
        string _Destination;
        private string _NameA;
        private string _NameB;

        protected short _ValueA;
        protected short _ValueB;
        #endregion Internal Data
    }
}
