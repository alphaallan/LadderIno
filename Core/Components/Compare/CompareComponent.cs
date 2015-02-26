using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (!short.TryParse(_NameA, out ValueA) && !string.IsNullOrEmpty(_NameA))
            {
                ValueA = (short)((DataTable != null) ? DataTable.GetValue(_NameA) : 0);
            }

            if (!short.TryParse(_NameB, out ValueB) && !string.IsNullOrEmpty(_NameB))
            {
                ValueB = (short)((DataTable != null) ? DataTable.GetValue(_NameB) : 0);
            }
        }

        protected virtual void ParameterChangedHandler(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) oldName = "0";
            if (string.IsNullOrEmpty(newName)) newName = "0";

            if (short.TryParse(oldName, out ValueA))
            {
                if (!short.TryParse(newName, out ValueA) && DataTable != null) DataTable.Add(newName, typeof(short));
            }
            else
            {
                if (DataTable != null)
                {
                    if (!short.TryParse(newName, out ValueA))
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
            if (DataTable != null)
            {
                if (!short.TryParse(_NameA, out ValueA) && !string.IsNullOrEmpty(_NameA)) DataTable.Add(_NameA, typeof(short));

                if (!short.TryParse(_NameB, out ValueB) && !string.IsNullOrEmpty(_NameB)) DataTable.Add(_NameB, typeof(short));
                
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
        private string _NameA;
        private string _NameB;

        protected short ValueA;
        protected short ValueB;
        #endregion Internal Data
    }
}
