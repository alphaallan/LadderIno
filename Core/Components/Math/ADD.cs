﻿
namespace Core.Components
{
    /// <summary>
    /// Component: ADD
    /// Description: Integer 16 bits sum function block
    /// Function: put on destination variable the sum of value A and value B
    /// </summary>
    public class ADD : MathComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel)
            {
                RetrieveData();
                if (DataTable != null) DataTable.SetValue(Destination, (short)(ValueA + ValueB));
            }
            InternalState = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public ADD()
        {
            
        }

        public ADD(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
