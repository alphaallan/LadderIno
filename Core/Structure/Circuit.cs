using Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace Core.Components
{
    /// <summary>
    /// Circuit structure for rung
    /// </summary>
    public class Circuit : ComponentBase
    {
        protected override void RunLogicalTest() { }

        public List<ComponentBase> Components { get; set; }
        public Circuit Parent { get; set; }
        public CircuitMode Mode { get; private set; }

        public Circuit(CircuitMode mode, Circuit parent)
        {
            Components = new List<ComponentBase>();
            Mode = mode;
            Parent = parent;
        }

        /// <summary>
        /// Build Circuit structure from a rung
        /// </summary>
        /// <param name="rung"></param>
        /// <returns></returns>
        public Circuit(Rung rung)
        {
            this.Components = new List<ComponentBase>();
            this.Mode = CircuitMode.Serial;
            this.Parent = null;

            List<NodeConnections> nodes = new List<NodeConnections>().RunAnalisys(rung);

            Stack<int> CircuitModeStack = new Stack<int>();
            CircuitModeStack.Push(-1);
            Stack<Circuit> Circuits = new Stack<Circuit>();
            
            this.LeftLide = rung.Components.First().LeftLide;
            this.RightLide = rung.Components.Last().RightLide;
            Circuits.Push(this);

            foreach (ComponentBase component in rung.Components)
            {
                NodeConnections NodeA = nodes.GetNodeConnections(component.LeftLide);
                NodeConnections NodeB = nodes.GetNodeConnections(component.RightLide);

                //Decide when add a parallel sub-circuit
                if (NodeA.OutComponents.Count > 1)
                {
                    if (CircuitModeStack.Peek() == -1)
                    {
                        CircuitModeStack.Push(NodeA.OutComponents.Count - 1);
                        Circuit parent = Circuits.Peek();
                        Circuit son = new Circuit(CircuitMode.Parallel, parent);
                        son.LeftLide = component.LeftLide;
                        parent.Components.Add(son);
                        Circuits.Push(son);
                    }
                    else CircuitModeStack.Push(CircuitModeStack.Pop() - 1);
                }

                // Decide when add a serial sub-circuit
                if (NodeB.InComponents.Count == 1 && CircuitModeStack.Peek() != -1)
                {
                    CircuitModeStack.Push(-1);
                    Circuit parent = Circuits.Peek();
                    Circuit son = new Circuit(CircuitMode.Serial, parent);
                    son.LeftLide = component.LeftLide;
                    parent.Components.Add(son);
                    Circuits.Push(son);
                }

                Circuits.Peek().Components.Add(component);

                //Close sub-circuit
                if ((NodeB.InComponents.Count > 1 || NodeA.InComponents.Count > 1) && CircuitModeStack.Count > 1 && CircuitModeStack.Peek() <= 0)
                {
                    CircuitModeStack.Pop();
                    Circuits.Pop().RightLide = component.RightLide;
                }
            }
        }
    }
}
