using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
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
                    Data.LDIVariableTable.Rename(_Destination, value);
                    _Destination = value;
                }
                catch (ArgumentException ex)
                {
                    if (ex.ParamName == "oldName")
                    {
                        Data.LDIVariableTable.Add(value, typeof(short));
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
            if (!short.TryParse(_NameA, out ValueA) && !string.IsNullOrEmpty(_NameA))
            {
                ValueA = (short)Data.LDIVariableTable.GetValue(_NameA);
            }

            if (!short.TryParse(_NameB, out ValueB) && !string.IsNullOrEmpty(_NameB))
            {
                ValueB = (short)Data.LDIVariableTable.GetValue(_NameB);
            }
        }

        protected virtual void ParameterChangedHandler(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) oldName = "0";
            if (string.IsNullOrEmpty(newName)) newName = "0";

            if (short.TryParse(oldName, out ValueA))
            {
                if (!short.TryParse(newName, out ValueA)) Data.LDIVariableTable.Add(newName, typeof(short));
            }
            else
            {
                if (!short.TryParse(newName, out ValueA))
                {
                    try
                    {
                        Data.LDIVariableTable.Rename(oldName, newName);
                    }
                    catch (ArgumentException ex)
                    {
                        if (ex.ParamName == "oldName") Data.LDIVariableTable.Add(newName, typeof(short));
                        else throw ex;
                    }
                }
                else Data.LDIVariableTable.Remove(oldName);
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

        #region Destructor
        ~MathComponent()
        {
            try
            {
                Data.LDIVariableTable.Remove(_Destination);
            }
            catch (ArgumentException) { }

            try
            {
                Data.LDIVariableTable.Remove(_NameA);
            }
            catch (ArgumentException) { }

            try
            {
                Data.LDIVariableTable.Remove(_NameB);
            }
            catch (ArgumentException) { }
        }
        #endregion Destructor

        #region Internal Data
        string _Destination;
        private string _NameA;
        private string _NameB;

        protected short ValueA;
        protected short ValueB;
        #endregion Internal Data
    }
}
