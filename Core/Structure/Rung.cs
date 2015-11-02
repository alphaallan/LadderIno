using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Core.Components
{
    /// <summary>
    /// Program rung 
    /// Represents one line in a ladder diagram.
    /// </summary>
    public class Rung : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Get or set (internally only) rung’s component collection
        /// </summary>
        public ObservableCollection<ComponentBase> Components
        {
            get { return _Components; }//set as internal
            private set
            {
                _Components = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Components")); }                
            }
        }

        /// <summary>
        /// Get or set rung power
        /// </summary>
        public bool RungPower
        {
            get { return PowerRail.LogicLevel; }
            set
            {
                PowerRail.LogicLevel = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RungPower")); }
            }
        }

        /// <summary>
        /// Get or set Comment in Rung
        /// </summary>
        public string Comment
        {
            get { return _Comment; }
            set
            {
                _Comment = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Comment")); }
            }
        }

        /// <summary>
        /// Rung datacontext
        /// </summary>
        public Data.LadderDataTable DataTable
        {
            get { return _DataTable; }
            set
            {
                _DataTable = value;
                foreach (ComponentBase comp in _Components) comp.DataTable = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DataTable")); }
            }
        }
        #endregion Properties

        #region Functions

        /// <summary>
        /// Run logical test in the entire rung
        /// </summary>
        public Rung Execute()
        {
            if (_DataTable == null) throw new NullReferenceException("No data table associated to the Rung");
            Trace.WriteLine("RungTest Started " + _Components.Count + " elements in rung", "Rung");
            Trace.Indent();
            if (RungPower)
            {
                foreach (ComponentBase comp in _Components) comp.Execute();
            }
            Trace.Unindent();

            return this;
        }

        #region Search and Collection
        public List<ComponentBase> GetAllBetween(Node startNode, Node endNode)
        {
            Trace.WriteLine("GetAllBetween Called", "Rung");
            int start = 0, end = _Components.Count - 1;
            System.Collections.Generic.List<ComponentBase> temp = new System.Collections.Generic.List<ComponentBase>();

            if (startNode == endNode) throw new Exception("Start and End nodes are the same");

            while (start < _Components.Count && _Components[start].LeftLide != startNode) start++;
            if (start == _Components.Count) throw new ArgumentException("Node not inserted in current Rung", "startNode");
            

            while (end >= 0 && _Components[end].RightLide != endNode) end--;
            if (end < 0) throw new ArgumentException("Node not inserted in current Rung", "endNode");
            
            if (end < start) throw new Exception("End and Start node are in reverse order");

            for (int index = start; index <= end; index++ ) temp.Add(_Components[index]);

            return temp;
        }

        public Tuple<Node, Node> FindInterception(ComponentBase componentA, ComponentBase componentB)
        {
            if (componentA == componentB) throw new Exception("Components A and B are the same");

            int indexA = _Components.IndexOf(componentA);
            int indexB = _Components.IndexOf(componentB);

            if (indexA == -1) throw new ArgumentException("Component not inserted in current Rung", "componentA");
            if (indexB == -1) throw new ArgumentException("Component not inserted in current Rung", "componentB");

            if (indexA > indexB)
            {
                int temp = indexA;
                indexA = indexB;
                indexB = temp;
            }

            #region Search Node B
            bool foundB = false;
            int rni = indexA - 1;

            while (!foundB && ++rni < indexB)
            {
                for (int rnj = indexB; rnj < _Components.Count; rnj++)
                {
                    if (_Components[rnj].RightLide == _Components[rni].RightLide)
                    {
                        foundB = true;
                        break;
                    }
                }
            }
            if (!foundB) return null;
            #endregion

            #region Search Node A
            bool foundA = false;
            int lni = indexB + 1;

            while (!foundA && --lni > indexA)
            {
                for (int lnj = indexA; lnj >= 0; lnj--)
                {
                    if (_Components[lnj].LeftLide == _Components[lni].LeftLide)
                    {
                        foundA = true;
                        break;
                    }
                }
            }
            if (!foundA) return null;
            #endregion

            return new Tuple<Node, Node>(_Components[lni].LeftLide, _Components[rni].RightLide);
        }

        public Node GetOutputFrontier()
        {
            IEnumerable<ComponentBase> outComponents = _Components.Where(x => x.Class == ComponentBase.ComponentClass.Output);

            if (outComponents.Count() == 1)
            {
                return outComponents.First().LeftLide;
            }
            else if (outComponents.Count() > 1)
            {
                return FindInterception(outComponents.First(), outComponents.Last()).Item1;
            }

            return null;
        }
        #endregion

        #region Insert Functions
        //This sector contains all the functions that insert a component in the rung

        #region Auxiliar
        /// <summary>
        /// Check component nodes to avoid null references
        /// </summary>
        /// <param name="component">Component to be verified</param>
        private Rung CheckNodes(ComponentBase component)
        {
            if (component.LeftLide == null) component.LeftLide = new Node();
            if (component.RightLide == null) component.RightLide = new Node();
            return this;
        }

        /// <summary>
        /// Verify component pair to be sure the insertion can happen 
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        private Rung CheckComponentPair(ComponentBase component, ComponentBase anchor)
        {
            if (anchor == null) throw new ArgumentNullException("Null component", "anchor");
            if (component == null) throw new ArgumentNullException("Null component", "component");
            if (!_Components.Contains(anchor)) throw new ArgumentException("Anchor component not inserted in current rung", "anchor");
            if (_Components.Contains(component)) throw new ArgumentException("Component already inserted in current rung", "component");
            if (component == anchor) throw new ArgumentException("Root components and inserter component are the same", "anchor");
            CheckNodes(component);
            return this;
        }

        /// <summary>
        /// Verify component-node pair to be sure the insertion can happen 
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor node</param>
        private Rung CheckComponentPair(ComponentBase component, Node anchor)
        {
            if (anchor == null) throw new ArgumentNullException("Null node", "anchor");
            if (component == null) throw new ArgumentNullException("Null component", "component");
            if (_Components.Contains(component)) throw new ArgumentException("Component already inserted in current rung", "component");
            CheckNodes(component);
            foreach (ComponentBase comp in _Components) if (comp.LeftLide == anchor || comp.RightLide == anchor) return this;
            throw new ArgumentException("Anchor node not inserted in current rung", "anchor");
        }
        #endregion Auxiliar

        /// <summary>
        /// Inset component with auto select position 
        /// </summary>
        /// <param name="component">Component to be added</param>
        public Rung Add(ComponentBase component)
        {
            Trace.WriteLine("Auto insert called", "Rung");
            if (component == null) throw new ArgumentNullException("Null component","component");
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                if (_Components.Count == 0)
                {
                    SC SC = new SC(PowerRail);
                    SC.RightLide = new Node(SC);

                    component.RightLide = GroundRail;
                    component.LeftLide = SC.RightLide;

                    _Components.Add(SC);
                    _Components.Add(component);

                    Trace.WriteLine(component.GetType() + " inserted at the empty rung", "Rung");
                }
                else
                {
                    ComponentBase LC = _Components[_Components.Count - 1];

                    component.LeftLide = (LC.Class != ComponentBase.ComponentClass.Output) ? LC.RightLide : LC.LeftLide;
                    component.RightLide = GroundRail;

                    _Components.Add(component);
                    Trace.WriteLine(component.GetType() + " inserted", "Rung");
                }
            }
            else
            {
                if (_Components.Count == 0)
                {
                    component.LeftLide = PowerRail;
                    component.RightLide.Root = component;

                    _Components.Add(component);
                    Trace.WriteLine(component.GetType() + " inserted at the empty rung", "Rung");
                }
                else 
                {
                    ComponentBase FC = _Components[0];

                    component.LeftLide = PowerRail;

                    if (FC is SC)
                    {
                        component.RightLide = FC.RightLide;
                        component.RightLide.Root = component;
                        _Components.Remove(FC);
                        Trace.Write(" Short Circuit removed -> ", "Rung");
                    }
                    else
                    {
                        component.RightLide.Root = component;
                        FC.LeftLide = component.RightLide;
                    }

                    _Components.Insert(0, component);

                    Trace.WriteLine(component.GetType() + " inserted", "Rung");
                }
            }
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert above anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertAbove(ComponentBase component, ComponentBase anchor)
        {
            Trace.WriteLine("Insert above called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                if (anchor.Class != ComponentBase.ComponentClass.Output) throw new Exception("Can not insert output component above an input component");

                component.LeftLide = anchor.LeftLide;                
                component.RightLide = GroundRail;
                _Components.Insert(_Components.IndexOf(anchor), component);
            }
            else
            {
                if (anchor.Class == ComponentBase.ComponentClass.Output) throw new Exception("Can not insert input component above an output component");

                if (anchor.RightLide.Root == anchor) anchor.RightLide.Root = component;

                component.LeftLide = anchor.LeftLide;
                component.RightLide = anchor.RightLide;
                _Components.Insert(_Components.IndexOf(anchor), component);

                if (anchor is SC) _Components.Remove(anchor);
            }

            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert under anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertUnder(ComponentBase component, ComponentBase anchor)
        {
            Trace.WriteLine("Insert under called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                if (anchor.Class != ComponentBase.ComponentClass.Output) throw new Exception("Can not insert output component under an input component");

                component.LeftLide = anchor.LeftLide;
                component.RightLide = GroundRail;
                _Components.Add(component);
            }
            else
            {
                if (anchor.Class == ComponentBase.ComponentClass.Output) throw new Exception("Can not insert input component under an output component");

                component.LeftLide = anchor.LeftLide;
                component.RightLide = anchor.RightLide;
                _Components.Insert(_Components.IndexOf(anchor) + 1, component);

                if (anchor is SC)
                {
                    component.RightLide.Root = component;
                    _Components.Remove(anchor);
                }
            }
            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert before anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertBefore(ComponentBase component, ComponentBase anchor)
        {
            Trace.WriteLine("Insert before called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                throw new Exception("Output components can only be inserted with straight connection to the power rail");
            }
            else
            {
                component.LeftLide = anchor.LeftLide;
                anchor.LeftLide = component.RightLide;

                _Components.Insert(_Components.IndexOf(anchor), component);

                if (anchor is SC)
                {
                    component.RightLide = anchor.RightLide;
                    _Components.Remove(anchor);
                }

                component.RightLide.Root = component;
            }
            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert before anchor node
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor Node</param>
        public Rung InsertBefore(ComponentBase component, Node anchor)
        {
            Trace.WriteLine("Insert before called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                throw new Exception("Output components can only be inserted with straight connection to the power rail");
            }
            else
            {
                component.LeftLide.Root = anchor.Root;
                component.RightLide = anchor;
                component.RightLide.Root = component;

                foreach (ComponentBase comp in _Components) if (comp.RightLide == anchor) comp.RightLide = component.LeftLide;

                _Components.Insert(_Components.IndexOf(component.LeftLide.Root) + 1, component);
            }
            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert after anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertAfter(ComponentBase component, ComponentBase anchor)
        {
            Trace.WriteLine("Insert after called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (anchor.RightLide.Root == null) anchor.RightLide.Root = anchor;

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                if (anchor.Class == ComponentBase.ComponentClass.Output) throw new Exception("Can not insert output component in series with an output component");

                component.LeftLide = anchor.RightLide;
                component.RightLide = GroundRail;
                _Components.Add(component);
            }
            else
            {
                if (anchor.Class == ComponentBase.ComponentClass.Output) throw new Exception("Can not insert input component after an output component");

                component.RightLide = anchor.RightLide;
                anchor.RightLide = component.LeftLide;
                anchor.RightLide.Root = anchor;

                if (component.RightLide.Root == anchor) component.RightLide.Root = component;

                _Components.Insert(_Components.IndexOf(anchor) + 1, component);

                if (anchor is SC)
                {
                    component.LeftLide = anchor.LeftLide;
                    _Components.Remove(anchor);
                }
            }
            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Insert before anchor node
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor node</param>
        public Rung InsertAfter(ComponentBase component, Node anchor)
        {
            Trace.WriteLine("Insert after called with anchor: " + anchor.GetType(), "Rung");
            CheckComponentPair(component, anchor);
            Trace.Indent();

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                throw new Exception("Output components can only be inserted with straight connection to the power rail");
            }
            else
            {
                component.LeftLide = anchor;
                component.RightLide.Root = component;

                ComponentBase FirstConnection = null;

                foreach (ComponentBase comp in _Components) 
                    if (comp.LeftLide == anchor) 
                    { 
                        comp.LeftLide = component.RightLide;
                        if (FirstConnection == null) FirstConnection = comp;
                    }

                _Components.Insert(_Components.IndexOf(FirstConnection), component);
            }
            Trace.WriteLine(component.GetType() + " inserted", "Rung");
            component.DataTable = DataTable;
            Trace.Unindent();
            return this;
        }
        #endregion Insert Functions

        #region Delete Functions
        /// <summary>
        /// Delete compoenent from the rung
        /// </summary>
        /// <param name="component">Component to be deleted</param>
        public Rung Remove(ComponentBase component)
        {
            Trace.WriteLine("Insert under called with component: " + component.GetType(), "Rung");
            if (!_Components.Contains(component)) throw new ArgumentException("Component not inserted in current Rung", "component");
            Trace.Indent();

            if(_Components.Where(x => x.LeftLide == component.LeftLide && x.RightLide == component.RightLide).Count() == 1)
            {
                IEnumerable<ComponentBase> RR = _Components.Where(x => x.RightLide == component.RightLide && x != component);

                if (RR.Count() > 0)
                {
                    if (component.RightLide.Root == component) component.RightLide.Root = RR.First();
                    if (_Components.Where(x => x.LeftLide == component.LeftLide).Count() == 1)
                        foreach (ComponentBase comp in _Components.Where(x => x.RightLide == component.LeftLide)) comp.RightLide = component.RightLide;
                }
                else
                {
                    foreach (ComponentBase comp in _Components.Where(x => x.LeftLide == component.RightLide)) comp.LeftLide = component.LeftLide;
                }

            }
            else if (component.RightLide.Root == component)
            {
                component.RightLide.Root = _Components.Where(x => x.LeftLide == component.LeftLide && x.RightLide == component.RightLide && x != component).First();
            }

            Trace.WriteLine(component.GetType() + " removed", "Rung");
            _Components.Remove(component);
            GC.Collect(); // Call gabarge collector
            Trace.Unindent();
            return this;
        }

        /// <summary>
        /// Clear Rung (delete all components)
        /// </summary>
        public void Clear()
        {
            _Components.Clear();
            Trace.Write("Rung Cleared", "Rung");
        }
        #endregion Delete Functions

        #endregion Functions

        #region Constructors
        /// <summary>
        /// Default Builder
        /// </summary>
        public Rung()
        {
            Components = new ObservableCollection<ComponentBase>();
            PowerRail = new Node();
            GroundRail = new Node();
            PowerRail.LogicLevel = true;
            Trace.WriteLine("New rung created", "Rung");
        }
        #endregion Constructors

        #region Destructor
        ~Rung()
        {
            Trace.WriteLine("Rung Destructor Called", "Rung");
            //DataTable = null; //This will force memory deallocate by all components to avoid another call to the garbage collector
        }
        #endregion Destructor

        #region Internal Data
        ObservableCollection<ComponentBase> _Components;
        Node PowerRail;
        Node GroundRail;

        string _Comment;

        private Data.LadderDataTable _DataTable;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data
    }
}
