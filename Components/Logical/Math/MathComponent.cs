using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Math components base
    /// </summary>
    public abstract class MathComponent : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Destination Variable
        /// </summary>
        public Int16 Destination
        {
            get { return _Destination; }
            set
            {
                _Destination = value;
                RaisePropertyChanged("Destination");
            }
        }

        /// <summary>
        /// Variable A
        /// </summary>
        public Int16 ValueA
        {
            get { return _ValueA; }
            set
            {
                _ValueA = value;
                RaisePropertyChanged("ValueA");
            }
        }

        /// <summary>
        /// Variable B (can be a constant)
        /// </summary>
        public Int16 ValueB
        {
            get { return _ValueB; }
            set
            {
                _ValueB = value;
                RaisePropertyChanged("ValueB");
            }
        }
        #endregion Properties

        #region Constructors
        public MathComponent()
        {
            Class = ComponentClass.Output;
        }

        public MathComponent(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Output;
        }
        #endregion Constructors

        #region Internal Data
        Int16 _Destination;
        Int16 _ValueA;
        Int16 _ValueB;
        #endregion Internal Data
    }
}
