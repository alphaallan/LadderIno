using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Contact
    /// Description: Basic boolean input component
    /// Function: Let flux pass whenever its input is true
    /// Mode: Inverted = let flux pass when input is false
    /// </summary>
    public class Contact : NameableComponentBase
    {
        #region Properties
        /// <summary>
        /// Define whenever the contact is closed (high logical level on input)
        /// </summary>
        public bool IsClosed
        {
            get { return _IsClosed; }
            set 
            { 
                _IsClosed = value;
                RaisePropertyChanged("IsClosed");
            }
        }

        /// <summary>
        /// Define if the contact is normally open
        /// </summary>
        public bool IsInverted
        {
            get { return _IsInverted; }
            set
            {
                _IsInverted = value;
                RaisePropertyChanged("IsInverted");
            }
        }

        /// <summary>
        /// Define what type of input the contact reads.
        /// Input pin, output pin or internal relay
        /// </summary>
        public ContactType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;

                NamePerfix = ((_Type == ContactType.InputPin) ? ComponentPrefix.Input : 
                              (_Type == ContactType.InternalRelay) ? ComponentPrefix.Relay : ComponentPrefix.Output); 

                RaisePropertyChanged("Type");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            InternalState = (_IsInverted ^ _IsClosed);
        }
        #endregion Functions

        #region Constructors
        public Contact(string name, bool inverted, ContactType type, Node Left, Node Right)
            : base(name,Left,Right)
        {
            this.IsInverted = inverted;
            this.Type = type;
            this.Class = ComponentClass.Input;
        }

        public Contact(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
        }

        public Contact(Node Left, Node Right)
            : base(Left, Right)
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
        }

        public Contact()
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
        }
        #endregion Constructors

        #region Internal Data
        ContactType _Type;
        bool _IsClosed;
        bool _IsInverted;
        #endregion Internal Data

        #region Enum
        public enum ContactType
        {
            InputPin,
            InternalRelay,
            OutputPin
        }
        #endregion Enum
    }
}
