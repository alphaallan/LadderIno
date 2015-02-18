﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
{
    /// <summary>
    /// Component: SUB
    /// Description: Integer 16 bits subtraction function block
    /// Function: put on destination variable the diference between value A and value B
    /// </summary>
    public class SUB : MathComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            RetrieveData();
            if (LeftLide.LogicLevel) Data.LDIVariableTable.SetValue(Destination, (short)(ValueA - ValueB));
            InternalState = LeftLide.LogicLevel;
        }
        #endregion Functions

        #region Constructors
        public SUB()
        {
            
        }

        public SUB(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
