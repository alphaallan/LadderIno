using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
{
    /// <summary>
    /// Base class for counters
    /// </summary>
    public abstract class CounterComponent : NameableComponent
    {
        #region Properties
        /// <summary>
        /// Counter limit value
        /// </summary>
        public string Limit
        {
            get { return _Limit; }
            set
            {
                if (short.TryParse(value, out LimitValue) || string.IsNullOrEmpty(value))
                {
                    if (!short.TryParse(_Limit, out LimitValue) && DataTable != null)
                    {
                        try
                        {
                            DataTable.Remove(_Limit);
                        }
                        catch (ArgumentException)
                        {

                        }
                    }
                }
                else if (DataTable != null) 
                {
                    if (short.TryParse(_Limit, out LimitValue))
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
        #endregion Properties

        #region Functions
        protected void RetrieveData()
        {
            if (!short.TryParse(_Limit, out LimitValue) && !string.IsNullOrEmpty(_Limit) && DataTable != null)
            {
                LimitValue = (short) DataTable.GetValue(_Limit);
            }

            CurrentValue = (short)((DataTable != null) ? DataTable.GetValue(FullName) : 0);
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
            CurrentValue = startValue;
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
            CurrentValue = startValue;
            NamePerfix = ComponentPrefix.Conter;
            Class = ComponentClass.Mixed;
        }
        #endregion Constructors

        #region Internal Data
        private string _Limit;

        protected bool LastInput;
        protected short LimitValue;
        protected short CurrentValue;
        #endregion Internal Data
    }
}
