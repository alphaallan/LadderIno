using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
{
    /// <summary>
    /// Base class for LadderIno components logical structure
    /// </summary>
    public abstract class ComponentBase : INotifyPropertyChanged
    {
        #region Properties
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

        /// <summary>
        /// Compoenet class (output, input or mixed)
        /// </summary>
        public ComponentClass Class
        {
            get { return _Class; }
            protected set
            {
                _Class = value;
                RaisePropertyChanged("Class");
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

            if (_RightLide != null && Class != ComponentClass.Output) _RightLide.LogicLevel = (_RightLide.Root == this) ? _InternalState : (_InternalState || _RightLide.LogicLevel);
            return _InternalState;
        }

        /// <summary>
        /// Run component specific logic
        /// Must set internal state and retrieve data from core's data table
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
            return this.GetType().ToString();
        }
        #endregion Functions

        #region Constructors
        public ComponentBase() : this(new Node(), new Node())
        {
            RightLide.Root = this;
        }

        public ComponentBase(Node Left, Node Right)
        {
            this.LeftLide = Left;
            this.RightLide = Right;
        }
        #endregion Constructors

        #region Internal Data
        private string _Comment;

        private bool _InternalState;

        private Node _LeftLide;
        private Node _RightLide;

        private ComponentClass _Class;
        
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data

        #region Enum
        public enum ComponentClass
        {
            Input,
            Mixed,
            Output,
        }
        #endregion Enum
    }
}
