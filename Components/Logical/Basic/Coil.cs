using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Component: Coil
    /// Description: Basic boolean output
    /// Function: Set output level dependin on its mode and input (LeftLide logical level)
    /// Mode:
    ///     Normal = Set output equals input level
    ///     Negated = Set output level opposite input level
    ///     Set = SetOnly mode, set and lock output high on input level high
    ///     Reset = ResetOnly mode, set and lock output low on input level high
    /// </summary>
    public class Coil : NameableComponentBase
    {
        #region Properties
        public CoilType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;

                NamePerfix = (_Type == CoilType.OutputPin) ? ComponentPrefix.Output : ComponentPrefix.Relay; 

                RaisePropertyChanged("Type");
            }
        }

        public CoilMode Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                RaisePropertyChanged("Mode");
            }
        }
        #endregion Properties

        #region Functions
        protected override void RunLogicalTest()
        {
            switch (_Mode)
            {
                case CoilMode.Normal:
                    InternalState = LeftLide.LogicLevel;
                    break;

                case CoilMode.Negated:
                    InternalState = !LeftLide.LogicLevel;
                    break;

                case CoilMode.Reset:
                    if (InternalState && LeftLide.LogicLevel) InternalState = false;
                    break;

                case CoilMode.Set:
                    if (!InternalState && LeftLide.LogicLevel) InternalState = true;
                    break;
            }
        }
        #endregion Functions

        #region Constructors
        public Coil(string name, CoilType type, CoilMode mode, Node Left)
            : base(name,Left,null)
        {
            this.Type = type;
            this.Mode = mode;
            this.Class = ComponentClass.Output;
        }

        public Coil(string name, Node Left)
            : base(name, Left, null)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
            this.Class = ComponentClass.Output;
        }

        public Coil(Node Left)
            : base(Left, null)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
            this.Class = ComponentClass.Output;
        }

        public Coil() : base(new Node(), null)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
            this.Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Internal Data
        CoilMode _Mode;
        CoilType _Type;
        #endregion Internal Data

        #region Enums
        public enum CoilMode
        {
            Normal = ' ',
            Set = 'S',
            Reset = 'R',
            Negated = '/'
        }

        public enum CoilType
        {
            InternalRelay,
            OutputPin
        }
        #endregion Enums

    }
}
