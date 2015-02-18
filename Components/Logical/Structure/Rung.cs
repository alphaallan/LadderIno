using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Logical
{
    public class Rung : INotifyPropertyChanged
    {
        #region Properties
        public ObservableCollection<ComponentBase> Components
        {
            get { return _Components; }//set as internal
            private set
            {
                _Components = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Components")); }                
            }
        }

        public bool RungPower
        {
            get { return PowerRail.LogicLevel; }
            set
            {
                PowerRail.LogicLevel = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RungPower")); }
            }
        }
        #endregion Properties

        #region Functions

        public void Execute()
        {
            foreach (ComponentBase comp in _Components)
            {
                comp.Execute();
            }
        }


        #region Insert Functions

        #region Auxiliar
        private void CheckNodes(ComponentBase component)
        {
            if (component.LeftLide == null) component.LeftLide = new Node();
            if (component.RightLide == null) component.RightLide = new Node();
        }

        private void CheckComponentPair(ComponentBase component, ComponentBase anchor)
        {
            if (anchor == null || component == null) throw new Exception("Null component");
            if (!_Components.Contains(anchor)) throw new Exception("Anchor component not inserted in current rung");
            if (_Components.Contains(component)) throw new Exception("Component already inserted in current rung");
            if (component == anchor) throw new Exception("Root components and inserter component are the same");
            CheckNodes(component);
        }
        #endregion Auxiliar

        public void Add(ComponentBase component)
        {
            if (component == null) throw new Exception("Null component");

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                if (_Components.Count == 0)
                {
                    ShortCircuit SC = new ShortCircuit(PowerRail);
                    SC.RightLide = new Node(SC);

                    component.RightLide = GroundRail;
                    component.LeftLide = SC.RightLide;

                    _Components.Add(SC);
                    _Components.Add(component);
                }
                else
                {
                    ComponentBase LC = _Components[_Components.Count - 1];

                    component.LeftLide = (LC.Class != ComponentBase.ComponentClass.Output) ? LC.RightLide : LC.LeftLide;
                    component.RightLide = GroundRail;

                    _Components.Add(component);
                }
            }
            else
            {
                if (_Components.Count == 0)
                {
                    component.LeftLide = PowerRail;
                    component.RightLide.Root = component;

                    _Components.Add(component);
                }
                else 
                {
                    ComponentBase FC = _Components[0];

                    component.LeftLide = PowerRail;

                    if (FC is ShortCircuit)
                    {
                        component.RightLide = FC.RightLide;
                        component.RightLide.Root = component;
                        _Components.Remove(FC);
                    }
                    else
                    {
                        component.RightLide.Root = component;
                        FC.LeftLide = component.RightLide;
                    }

                    _Components.Insert(0, component);
                }
            }
        }

        public void InsertAbove(ComponentBase component, ComponentBase anchor)
        {
            CheckComponentPair(component, anchor);

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

                if (anchor is ShortCircuit) _Components.Remove(anchor);
            }
        }

        public void InsertUnder(ComponentBase component, ComponentBase anchor)
        {
            CheckComponentPair(component, anchor);

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

                if (anchor is ShortCircuit)
                {
                    component.RightLide.Root = component;
                    _Components.Remove(anchor);
                }
            }
        }

        public void InsertBefore(ComponentBase component, ComponentBase anchor)
        {
            CheckComponentPair(component, anchor);

            if (component.Class == ComponentBase.ComponentClass.Output)
            {
                throw new Exception("Output components can only be inserted with straight connection to the power rail");
            }
            else
            {
                component.LeftLide = anchor.LeftLide;
                anchor.LeftLide = component.RightLide;

                _Components.Insert(_Components.IndexOf(anchor), component);

                if (anchor is ShortCircuit)
                {
                    component.RightLide = anchor.RightLide;
                    _Components.Remove(anchor);
                }

                component.RightLide.Root = component;
            }
        }

        public void InsertAfter(ComponentBase component, ComponentBase anchor)
        {
            CheckComponentPair(component, anchor);

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

                if (anchor is ShortCircuit)
                {
                    component.LeftLide = anchor.LeftLide;
                    _Components.Remove(anchor);
                }
            }
        }

        #endregion Insert Functions

        #region Delete Functions
        public void Remove(ComponentBase component)
        {
            if (!_Components.Contains(component)) throw new Exception("Component not inserted in current Rung");

            int componentIndex = _Components.IndexOf(component);

            if (component.RightLide.Root == component)
            {

            }
            else
            {

            }
        }

        public void Clear()
        {
            _Components.Clear();
        }
        #endregion Delete Functions

        #endregion Functions

        #region Constructors
        public Rung()
        {
            Components = new ObservableCollection<ComponentBase>();
            PowerRail = new Node();
            GroundRail = new Node();
            PowerRail.LogicLevel = true;
        }
        #endregion Constructors

        #region Internal Data
        ObservableCollection<ComponentBase> _Components;
        Node PowerRail;
        Node GroundRail;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data
    }
}
