
namespace Core.Components
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
    public class Coil : NameableComponent
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
                    if(DataTable != null) DataTable.SetValue(FullName, InternalState);
                    break;

                case CoilMode.Negated:
                    InternalState = !LeftLide.LogicLevel;
                    if(DataTable != null) DataTable.SetValue(FullName, InternalState);
                    break;

                case CoilMode.Reset:
                    InternalState = LeftLide.LogicLevel;
                    if (DataTable != null && LeftLide.LogicLevel) DataTable.SetValue(FullName, false);
                    break;

                case CoilMode.Set:
                    InternalState = LeftLide.LogicLevel;
                    if (DataTable != null && LeftLide.LogicLevel) DataTable.SetValue(FullName, true);
                    break;
            }

        }
        #endregion Functions

        #region Constructors
        public Coil(string name, CoilType type, CoilMode mode, Node Left)
            : base(typeof(bool), name,Left,null)
        {
            this.Type = type;
            this.Mode = mode;
            this.Class = ComponentClass.Output;
        }

        public Coil(string name, Node Left)
            : base(typeof(bool), name, Left, null)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
            this.Class = ComponentClass.Output;
        }

        public Coil(Node Left)
            : base(typeof(bool), Left, null)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
            this.Class = ComponentClass.Output;
        }

        public Coil()
            : base(typeof(bool), new Node(), null)
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
