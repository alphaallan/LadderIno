using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Core.Data
{
    public class LDPin : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// LD variable assosciated to the pin
        /// </summary>
        public string Variable
        {
            get { return _Variable; }
            set
            {
                _Variable = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Variable")); }
            }
        }

        /// <summary>
        /// Pin type
        /// </summary>
        public PinType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Type")); }
            }
        }

        /// <summary>
        /// Arduino Pin
        /// </summary>
        public string Pin
        {
            get { return _Pin; }
            set
            {
                _Pin = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Pin")); }
            }
        }


        public LDPin(string variable, PinType type, string pin)
        {
            Variable = variable;
            Pin = pin;
            Type = type;
        }

        /// <summary>
        /// Builder
        /// </summary>
        /// <param name="type">Pin type</param>
        public LDPin(string variable, PinType type)
            : this(variable, type, "NONE")
        {

        }

        public object Clone()
        {
            return new LDPin(Variable, Type, Pin);
        }

        #region Internal Data
        public event PropertyChangedEventHandler PropertyChanged;
        private string _Variable;
        private PinType _Type;
        private string _Pin;
        #endregion Internal Data

    }
}
