
namespace Core.Components
{
    /// <summary>
    /// Component: Not Equals
    /// Description: Not Equals compare block
    /// Function: True if value A is different of value B
    /// </summary>
    public class NEQ : CompareComponent
    {
        #region Functions
        protected override void RunLogicalTest()
        {
            if (LeftLide.LogicLevel) RetrieveData();
            InternalState = (LeftLide.LogicLevel && (ValueA != ValueB));
        }
        #endregion Functions

        #region Constructors
        public NEQ()
        {
            
        }

        public NEQ(Node Left, Node Right)
            : base(Left, Right)
        {
            
        }
        #endregion Constructors
    }
}
