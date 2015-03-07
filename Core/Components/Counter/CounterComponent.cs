using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components
{
    /// <summary>
    /// Base class for counters
    /// </summary>
    public abstract class CounterComponent : NameableComponent
    {
        #region Properties
        /// <summary>
        /// Counter limit variable
        /// </summary>
        public string Limit
        {
            get { return _Limit; }
            set
            {
                if (short.TryParse(value, out _LimitValue) || string.IsNullOrEmpty(value))
                {
                    if (!short.TryParse(_Limit, out _LimitValue) && DataTable != null)
                    {
                        try
                        {
                            DataTable.Remove(_Limit);
                        }
                        catch (ArgumentException){ }
                    }
                }
                else if (DataTable != null) 
                {
                    if (short.TryParse(_Limit, out _LimitValue))
                    {
                        DataTable.Add(value, typeof(short));
                    }
                    else
                    {
                        try
                        {
                            DataTable.Rename(_Limit, value);
                        }
                        catch (ArgumentException ex)
                        {
                            if (ex.ParamName == "oldName") DataTable.Add(value, typeof(short));
                            else throw ex;
                        }
                    }
                }

                _Limit = value;
                RaisePropertyChanged("Limit");
            }
        }

        /// <summary>
        /// Counter limit current value
        /// </summary>
        public short LimitValue
        {
            get { return _LimitValue; }
            set 
            {
                if (short.TryParse(_Limit, out _LimitValue) || string.IsNullOrEmpty(_Limit))
                {
                    _Limit = value.ToString();
                    RaisePropertyChanged("Limit");
                }
                else if (DataTable != null) DataTable.SetValue(_Limit, value);

                _LimitValue = value;
                RaisePropertyChanged("LimitValue");
            }
        }

        /// <summary>
        /// Counter Current Value
        /// </summary>
        public short CurrentValue
        {
            get { return _CurrentValue; }
            set 
            { 
                _CurrentValue = value;
                if (DataTable != null) DataTable.SetValue(FullName, value);
                RaisePropertyChanged("CurrentValue");
            }
        }
        #endregion Properties

        #region Functions
        protected void RetrieveData()
        {
            if (!short.TryParse(_Limit, out _LimitValue) && !string.IsNullOrEmpty(_Limit) && DataTable != null)
            {
                _LimitValue = (short) DataTable.GetValue(_Limit);
            }

            _CurrentValue = (short)((DataTable != null) ? DataTable.GetValue(FullName) : _CurrentValue);
        }

        protected override void DataTableRelease()
        {
            base.DataTableRelease();

            try
            {
                if (DataTable != null) DataTable.Remove(_Limit);
            }
            catch (ArgumentException) { }
        }

        protected override void DataTableAlloc()
        {
            base.DataTableAlloc();

            if (DataTable != null) DataTable.Add(_Limit, typeof(short));
        }
        #endregion Functions

        #region Constructors
        public CounterComponent() 
            : base(typeof(short))
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(Node Left, Node Right)
            : base(typeof(short), Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(short startValue, Node Left, Node Right)
            : this(Left, Right)
        {
            _CurrentValue = startValue;
        }

        public CounterComponent(string name, Node Left, Node Right)
            : base(typeof(short), name, Left, Right)
        {
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }

        public CounterComponent(string name, short startValue, Node Left, Node Right)
            : base(typeof(short), name, Left, Right)
        {
            _CurrentValue = startValue;
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }
        #endregion Constructors

        #region Internal Data
        private string _Limit;

        protected bool LastInput;
        private short _LimitValue;
        private short _CurrentValue;
        #endregion Internal Data
    }
}
