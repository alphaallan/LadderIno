using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    /// <summary>
    /// Base classe for compare operations
    /// </summary>
    public abstract class CompareComponentBase : ComponentBase
    {
        #region Properties
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
        public CompareComponentBase()
        {
            Class = ComponentClass.Input;
        }

        public CompareComponentBase(Node Left, Node Right)
            : base(Left, Right)
        {
            Class = ComponentClass.Input;
        }
        #endregion Constructors

        #region Internal Data
        Int16 _ValueA;
        Int16 _ValueB;
        #endregion Internal Data
    }
}
