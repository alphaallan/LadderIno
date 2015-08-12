
namespace Core.Components
{
    /// <summary>
    /// Auxiliar structure component Short Circuit.
    /// This always will be deleted
    /// </summary>
    public class SC : ComponentBase
    {
        protected override void RunLogicalTest()
        {
            InternalState = LeftLide.LogicLevel;
        }

        public SC(Node Left, Node Right) 
            : base(Left, Right)
        {
            Class = ComponentClass.Input;
        }

        public SC(Node Left)
            : base(Left, null)
        {
            Class = ComponentClass.Input;
        }

        public SC() 
            : base()
        {

        }
    }
}
