﻿
namespace Core.Components
{
    /// <summary>
    /// Component: Contact
    /// Description: Basic boolean input component
    /// Function: Let flux pass whenever its input is true
    /// Mode: Inverted = let flux pass when input is false
    /// </summary>
    public class Contact : NameableComponent
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
                if (DataTable != null && _Type == ContactType.InputPin) DataTable.SetValue(FullName, value);
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

                VarClass = ((_Type == ContactType.InputPin) ? Data.LDVarClass.Input :
                              (_Type == ContactType.InternalRelay) ? Data.LDVarClass.Data : Data.LDVarClass.Output);

                NamePerfix = ((_Type == ContactType.InputPin) ? ComponentPrefix.Input : 
                              (_Type == ContactType.InternalRelay) ? ComponentPrefix.Relay : ComponentPrefix.Output);

                
                if (DataTable != null) DataTable.SetVarClass(FullName, VarClass);

                RaisePropertyChanged("Type");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel) IsClosed = (bool)((DataTable != null) ? DataTable.GetValue(FullName) : IsClosed);
            InternalState = (LeftLide.LogicLevel && (_IsInverted ^ _IsClosed));
        }
        #endregion Functions

        #region Constructors
        public Contact(string name, bool inverted, ContactType type, Node Left, Node Right)
            : base(typeof(bool), name, Left, Right)
        {
            this.IsInverted = inverted;
            this.Type = type;
            this.Class = ComponentClass.Input;
            this.VarClass = Data.LDVarClass.Input;
        }

        public Contact(string name, Node Left, Node Right)
            : base(typeof(bool), name, Left, Right)
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
            this.VarClass = Data.LDVarClass.Input;
        }

        public Contact(Node Left, Node Right)
            : base(typeof(bool), Left, Right)
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
            this.VarClass = Data.LDVarClass.Input;
        }

        public Contact()
            : base(typeof(bool))
        {
            this.Type = ContactType.InputPin;
            this.Class = ComponentClass.Input;
            this.VarClass = Data.LDVarClass.Input;
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
