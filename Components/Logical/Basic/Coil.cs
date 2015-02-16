﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    public class Coil : ComponentBase
    {
        #region Properties
        public CoilType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;

                NamePerfix = (char)((_Type == CoilType.OutputPin) ? ComponentPrefix.Output : ComponentPrefix.Relay); 

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
        public Coil(string name, CoilType type, CoilMode mode, Node Left, Node Right)
            : base(name,Left,Right)
        {
            this.Type = type;
            this.Mode = mode;
        }

        public Coil(string name, Node Left, Node Right)
            : base(name, Left, Right)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
        }

        public Coil(Node Left, Node Right)
            : base(Left, Right)
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
        }

        public Coil()
        {
            this.Type = CoilType.OutputPin;
            this.Mode = CoilMode.Normal;
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
