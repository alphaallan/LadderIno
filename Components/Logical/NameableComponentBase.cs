using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Base class for LadderIno components that have name 
    /// </summary>
    public abstract class NameableComponentBase : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Component Name
        /// Used by compiler as identifier
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Length > 40) value = value.Substring(0, 40);
                    _Name = value;
                }
                else _Name = string.Empty;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Name prefix 
        /// </summary>
        public ComponentPrefix NamePerfix
        {
            get { return _NamePrefix; }
            protected set 
            { 
                _NamePrefix = value;
                RaisePropertyChanged("NamePerfix");
            }
        }
        #endregion Properties

        #region Functions
        public override string ToString()
        {
            return ((char)this._NamePrefix) + this._Name + " (" + this.GetType().ToString() + ")" ;
        }
        #endregion Functions

        #region Constructors
        public NameableComponentBase() : this("NEW")
        {
            
        }

        public NameableComponentBase(string name)
        {
            Name = name;            
        }

        public NameableComponentBase(Node Left, Node Right) 
            : this ("NEW", Left, Right)
        {

        }

        public NameableComponentBase(string name, Node Left, Node Right)
            : base(Left, Right)
        {
            Name = name;
        }
        #endregion Constructors

        #region Internal Data
        private ComponentPrefix _NamePrefix;
        private string _Name;
        #endregion Internal Data

        #region Enums
        public enum ComponentPrefix
        {
            AnalogInput = 'A',
            Conter = 'C',
            Input = 'X',
            Output = 'Y',
            PWM = 'D',
            Relay = 'R',
            Timer = 'T',
            None = ' '
        }
        #endregion Enums
        
    }
}
