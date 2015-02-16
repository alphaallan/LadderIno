using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Base class for LadderIno components logical structure
    /// </summary>
    public abstract class ComponentBase : INotifyPropertyChanged
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
        public char NamePerfix
        {
            get { return _NamePrefix; }
            protected set { _NamePrefix = value; }
        }


        /// <summary>
        /// Componente comment
        /// Used only for documentation purposes  
        /// </summary>
        public string Comment
        {
            get { return _Comment; }
            set 
            { 
                _Comment = value;
                RaisePropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Read component internal state
        /// </summary>
        public bool InternalState
        {
            get { return _InternalState; }
            protected set
            {
                _InternalState = value;
                RaisePropertyChanged("InternalState");
            }
        }

        /// <summary>
        /// Connection Node on Left (input)
        /// </summary>
        public Node LeftLide
        {
            get { return _LeftLide; }
            set
            {
                _LeftLide = value;
                RaisePropertyChanged("LeftLide");
            }
        }

        /// <summary>
        /// Connection Node on Right (output)
        /// </summary>
        public Node RightLide
        {
            get { return _RightLide; }
            set
            {
                if (value != null && value.Root == null) value.Root = this;
                _RightLide = value;
                RaisePropertyChanged("RightLide");
            }
        }
        #endregion Properties

        #region Functions
        /// <summary>
        /// Execute internal logical tests
        /// </summary>
        /// <returns>Internal State</returns>
        public bool Execute()
        {
            if (_LeftLide != null) RunLogicalTest();
            else InternalState = false;

            if (_RightLide != null) _RightLide.LogicLevel = (_RightLide.Root == this) ? _InternalState : (_InternalState || _RightLide.LogicLevel);
            return _InternalState;
        }

        /// <summary>
        /// Run component specific logic
        /// Must set internal state
        /// </summary>
        protected abstract void RunLogicalTest();

        /// <summary>
        /// Function to use INotifyPropertyChanged on inered classes
        /// </summary>
        /// <param name="prop">Property name</param>
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public override string ToString()
        {
            return this.Name + this.GetType().ToString();
        }
        #endregion Functions

        #region Constructors
        public ComponentBase() : this("NEW")
        {
            
        }

        public ComponentBase(string name) : this(name, new Node(), new Node())
        {
            
        }

        public ComponentBase(Node Left, Node Right) : this ("NEW", Left, Right)
        {

        }

        public ComponentBase(string name, Node Left, Node Right)
        {
            Name = name;
            this.LeftLide = LeftLide;
            this.RightLide = Right;
        }
        #endregion Constructors

        #region Internal Data
        private char _NamePrefix;
        private string _Name;
        private string _Comment;

        private bool _InternalState;

        private Node _LeftLide;
        private Node _RightLide;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data

        #region Enums
        public enum ComponentPrefix
        {
            AnalogInput = 'A',
            Conter = 'C',
            Input = 'X',
            Output = 'Y',
            Relay = 'R',
            Timer = 'T',
            None = ' '
        }
        #endregion Enums
    }
}
