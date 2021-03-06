﻿
namespace Core.Components
{
    /// <summary>
    /// Component: DIV
    /// Description: Integer 16 bits divide function block
    /// Function: put on destination variable the proportion between value A and value B
    /// </summary>
    public class DIV : MathComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel) 
            { 
                RetrieveData();
                if (DataTable != null) DataTable.SetValue(Destination, (short)(ValueA / ValueB));
            }
            InternalState = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public DIV()
        {
            
        }

        public DIV(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
