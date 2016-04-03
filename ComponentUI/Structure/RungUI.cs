using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ComponentUI
{
    public class RungUI : Grid
    {
        #region Properties
        public bool RungPower
        {
            get { return _LogicalRung.RungPower; }
            set { _LogicalRung.RungPower = value; }
        }

        public string Comment
        {
            get { return _LogicalRung.Comment; }
            set { _LogicalRung.Comment = value; }
        }

        public Core.Data.LadderDataTable DataTable
        {
            get { return _LogicalRung.DataTable; }
            set { _LogicalRung.DataTable = value; }
        }

        public Core.Components.Rung LogicalRung
        {
            get { return _LogicalRung; }
            private set { _LogicalRung = value; }
        }
        #endregion Properties

        #region Functions

        /// <summary>
        /// Run logical test in the entire rung
        /// </summary>
        public void Execute()
        {
            _LogicalRung.Execute();
        }


        #region Insert Functions
        //This sector contains all the functions that insert a component in the rung

        #region Help Function
        public RungUI AddRow()
        {
            var row = new RowDefinition();
            row.Height = GridLength.Auto;
            RowDefinitions.Add(row);
            return this;
        }

        public RungUI AddRow(int pos)
        {
            var row = new RowDefinition();
            row.Height = GridLength.Auto;
            RowDefinitions.Insert(pos, row);
            return this;
        }

        public RungUI AddColumn()
        {
            var col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            ColumnDefinitions.Insert(0, col);
            return this;
        }

        public RungUI AddColumn(double width, GridUnitType unit)
        {
            var col = new ColumnDefinition();
            col.Width = new GridLength(width, unit);
            ColumnDefinitions.Insert(0, col);
            return this;
        }

        public RungUI AddColumn(int pos)
        {
            var col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            ColumnDefinitions.Insert(pos, col);
            return this;
        }

        public RungUI AddColumn(int pos, double width, GridUnitType unit)
        {
            var col = new ColumnDefinition();
            col.Width = new GridLength(width, unit);
            ColumnDefinitions.Insert(pos, col);
            return this;
        }

        public bool IsSlotEmpty(int row, int col)
        {
            return ((row < RowDefinitions.Count) && 
                    (col < ColumnDefinitions.Count) && 
                    (_Components.Where(x => x.Row == row && x.Column == col).Count() == 0));
        }

        private List<ComponentGridPosition> GetAllBetween(Core.Components.Node startNode, Core.Components.Node endNode)
        {
            var logicList = _LogicalRung.GetAllBetween(startNode, endNode);
            return _Components.Where(x => logicList.Contains(x.Component.LogicComponent)).ToList();
        }

        private Tuple<Core.Components.Node,Core.Components.Node> FindInterception(ComponentUIBase componentA, ComponentUIBase componentB)
        {
            return _LogicalRung.FindInterception(componentA.LogicComponent, componentB.LogicComponent);
        }

        private List<ComponentGridPosition> GetAllWiredUnder(ComponentGridPosition component)
        {
            var LC =_Components.Where(x => x.Row > component.Row && x.Column < component.Column);
            if(LC.Count() == 0) return new List<ComponentGridPosition>();

            var RC = _Components.Where(x => x.Row > component.Row && x.Column > component.Column);
            if (RC.Count() == 0) return new List<ComponentGridPosition>();

            var Nodes = LC.Select(x => x.Component.LogicComponent.RightLide).Distinct().Intersect(RC.Select(x => x.Component.LogicComponent.LeftLide));
            if(Nodes.Count() == 0) return new List<ComponentGridPosition>();

            return new List<ComponentGridPosition>(LC.Where(x => Nodes.Contains(x.Component.LogicComponent.RightLide)));
        }
        #endregion Help Function

        /// <summary>
        /// Insert component with auto select position 
        /// </summary>
        /// <param name="component">Component to be added</param>
        public RungUI Add(ComponentUIBase component)
        {
            _LogicalRung.Add(component.LogicComponent);

            if (component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output)
            {
                var temp = _Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output);
                if (_OutputBlockLegth == 0) _OutputBlockLegth = 1;

                if (temp.Count() == 0)
                {
                    _Components.Add(new ComponentGridPosition(component, 0, ColumnDefinitions.Count - 1));
                }
                else
                {
                    int max_row = temp.Max(x => x.Row);
                    if (max_row == RowDefinitions.Count - 1) AddRow();
                    
                    _Components.Add(new ComponentGridPosition(component, max_row + 1, ColumnDefinitions.Count - 1));
                }
            }
            else
            {
                AddColumn();
                foreach (var item in _Components) item.SetPossition(item.Row, item.Column + 1);
                _Components.Add(new ComponentGridPosition(component, 0, 0));
            }

            Children.Add(component);
            return this;
        }

        /// <summary>
        /// Insert above anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public RungUI InsertAbove(ComponentUIBase component, ComponentUIBase anchor)
        {
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            IEnumerable<ComponentGridPosition> column_components = _Components.Where(x => (x.Column == _anchor.Column));

            Core.Components.Node NodeA;
            Core.Components.Node NodeB;

            if (column_components.Count() > 1)
            {
                var nodes = FindInterception(column_components.First().Component, column_components.Last().Component);
                NodeA = nodes.Item1;
                NodeB = nodes.Item2;
            }
            else
            {
                //No other components um column
                NodeA = anchor.LogicComponent.LeftLide;
                NodeB = anchor.LogicComponent.RightLide;
            }

            List<ComponentGridPosition> move_list = GetAllBetween(NodeA, NodeB).Where(x => x.Row > _anchor.Row).ToList();

            _LogicalRung.InsertAbove(component.LogicComponent, anchor.LogicComponent);

            if (!IsSlotEmpty(RowDefinitions.Count - 1, _anchor.Column) || 
                (component.LogicComponent.Class != Core.Components.ComponentBase.ComponentClass.Output && _anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth)) 
                AddRow(_anchor.Row);

            if (_anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth) 
                move_list.AddRange(_Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output && x.Row > _anchor.Row && !move_list.Contains(x)));

            var WU = GetAllWiredUnder(_anchor);

            if (WU.Count() > 0)
            {
                int max_row = WU.Max(x => x.Row + 1);
                move_list.AddRange(WU);
                if (max_row > RowDefinitions.Count - 1) AddRow(_anchor.Row);
            }

            foreach (ComponentGridPosition item in move_list.Distinct()) item.SetPossition(item.Row + 1, item.Column);

            _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column));

            _anchor.SetPossition(_anchor.Row + 1, _anchor.Column);

            Children.Add(component);
            return this;
        }

        /// <summary>
        /// Insert under anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public RungUI InsertUnder(ComponentUIBase component, ComponentUIBase anchor)
        {
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            IEnumerable<ComponentGridPosition> column_components = _Components.Where(x => (x.Column == _anchor.Column));

            Core.Components.Node NodeA;
            Core.Components.Node NodeB;

            if (column_components.Count() > 1)
            {
                var nodes = FindInterception(column_components.First().Component, column_components.Last().Component);
                NodeA = nodes.Item1;
                NodeB = nodes.Item2;
            }
            else
            {
                //No other components um column
                NodeA = anchor.LogicComponent.LeftLide;
                NodeB = anchor.LogicComponent.RightLide;
            }

            List<ComponentGridPosition> move_list = GetAllBetween(NodeA, NodeB).Where(x => x.Row > _anchor.Row).ToList();

            _LogicalRung.InsertUnder(component.LogicComponent, anchor.LogicComponent);

            if (!IsSlotEmpty(RowDefinitions.Count - 1, _anchor.Column) ||
                (component.LogicComponent.Class != Core.Components.ComponentBase.ComponentClass.Output && _anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth))
                AddRow(_anchor.Row);

            if (_anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth)
                move_list.AddRange(_Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output && x.Row > _anchor.Row && !move_list.Contains(x)));

            var WU = GetAllWiredUnder(_anchor);

            if (WU.Count() > 0)
            {
                int max_row = WU.Max(x => x.Row + 1);
                move_list.AddRange(WU);
                if (max_row > RowDefinitions.Count - 1) AddRow(_anchor.Row);
            }

            foreach (ComponentGridPosition item in move_list.Distinct()) item.SetPossition(item.Row + 1, item.Column);

            _Components.Add(new ComponentGridPosition(component, _anchor.Row + 1, _anchor.Column));

            Children.Add(component);
            return this;  
        }

        /// <summary>
        /// Insert before anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public RungUI InsertBefore(ComponentUIBase component, ComponentUIBase anchor)
        {
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            IEnumerable<ComponentGridPosition> column_components = _Components.Where(x => (x.Column == _anchor.Column));
            List<ComponentGridPosition> move_list = new List<ComponentGridPosition>();
            IEnumerable<Core.Components.ComponentBase> parallel = null;
            bool addColumn = false;

            if (_anchor.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output)
            {
                Core.Components.Node NodeA;
                Core.Components.Node NodeB;

                if (column_components.Count() > 1)
                {
                    var nodes = FindInterception(column_components.First().Component, column_components.Last().Component);
                    NodeA = nodes.Item1;
                    NodeB = nodes.Item2;
                }
                else
                {
                    //No other components um column
                    NodeA = anchor.LogicComponent.LeftLide;
                    NodeB = anchor.LogicComponent.RightLide;
                }

                parallel = _LogicalRung.GetAllBetween(NodeA, NodeB);

                int parallel_min_col = _Components.Where(x => parallel.Contains(x.Component.LogicComponent)).Min(x => x.Column);
                addColumn = !(parallel_min_col < _anchor.Column && IsSlotEmpty(_anchor.Row, _anchor.Column - 1));

                int insert_pos = parallel_min_col;
                while (insert_pos < _anchor.Column && !IsSlotEmpty(_anchor.Row, insert_pos)) insert_pos++;

                _LogicalRung.InsertBefore(component.LogicComponent, anchor.LogicComponent);

                _Components.Add(new ComponentGridPosition(component, _anchor.Row, insert_pos));
            }
            else
            {
                if (column_components.Count() > 1)
                {
                    IEnumerable<ComponentGridPosition> test_componets = column_components.Where(x => x != _anchor);
                    int parallel_max_col;

                    foreach (var item in test_componets)
                    {
                        var nodes = FindInterception(item.Component, _anchor.Component);
                        var temp = _LogicalRung.GetAllBetween(nodes.Item1, nodes.Item2);
                        parallel = (parallel == null || parallel.Count() > temp.Count()) ? temp : parallel;
                    }

                    parallel_max_col = _Components.Where(x => parallel.Contains(x.Component.LogicComponent)).Max(x => x.Column);

                    addColumn = !IsSlotEmpty(_anchor.Row, parallel_max_col);
                    if (addColumn) move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column) && (!(parallel.Contains(x.Component.LogicComponent)) || x.Row == _anchor.Row)));
                }
                else
                {
                    move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column)));
                    addColumn = true;
                }

                move_list.Add(_anchor);

                _LogicalRung.InsertBefore(component.LogicComponent, anchor.LogicComponent);

                _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column));
            }

            if (addColumn)
            {
                if (_anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth) _OutputBlockLegth++;
                AddColumn(_anchor.Column);
                move_list.AddRange(_Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output && !move_list.Contains(x)));
            }

            foreach (ComponentGridPosition item in move_list) item.SetPossition(item.Row, item.Column + 1);

            Children.Add(component);
            return this;        
        }

        /// <summary>
        /// Insert before anchor node
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor Node</param>
        public RungUI InsertBefore(ComponentUIBase component, Node anchor)
        {
            return this;        
        }

        /// <summary>
        /// Insert after anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public RungUI InsertAfter(ComponentUIBase component, ComponentUIBase anchor)
        {
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            IEnumerable<ComponentGridPosition> column_components = _Components.Where(x => (x.Column == _anchor.Column));
            List<ComponentGridPosition> move_list = new List<ComponentGridPosition>();
            bool addColumn = false;

            if (column_components.Count() > 1)
            {
                IEnumerable<ComponentGridPosition> test_componets = column_components.Where(x => x != _anchor);
                IEnumerable<Core.Components.ComponentBase> parallel = null;
                int parallel_max_col;

                foreach (var item in test_componets)
                {
                    var nodes = FindInterception(item.Component, _anchor.Component);
                    var temp = _LogicalRung.GetAllBetween(nodes.Item1, nodes.Item2);
                    parallel = (parallel == null || parallel.Count() > temp.Count()) ? temp : parallel;
                }

                parallel_max_col = _Components.Where(x => parallel.Contains(x.Component.LogicComponent)).Max(x => x.Column);

                addColumn = !IsSlotEmpty(_anchor.Row, parallel_max_col);
                if (addColumn) move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column) && (!(parallel.Contains(x.Component.LogicComponent)) || x.Row == _anchor.Row)));
            }
            else
            {
                move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column)));
                addColumn = true;
            }

            _LogicalRung.InsertAfter(component.LogicComponent, anchor.LogicComponent);

            _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column + 1));

            if (addColumn)
            {
                if (_OutputBlockLegth == 0 && component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output) _OutputBlockLegth = 1;
                else if (_anchor.Column >= ColumnDefinitions.Count - _OutputBlockLegth) _OutputBlockLegth++;
                AddColumn(_anchor.Column);
                move_list.AddRange(_Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output && !move_list.Contains(x)));
            }

            foreach (ComponentGridPosition item in move_list) item.SetPossition(item.Row, item.Column + 1);

            Children.Add(component);

            return this;        
        }

        /// <summary>
        /// Insert before anchor node
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor node</param>
        public RungUI InsertAfter(ComponentUIBase component, Node anchor)
        {
            return this;        
        }
        #endregion Insert Functions

        #region Delete Functions
        /// <summary>
        /// Delete compoenent from the rung
        /// </summary>
        /// <param name="component">Component to be deleted</param>
        public RungUI Remove(ComponentUIBase component)
        {
            ComponentGridPosition _component = _Components.Where(x => x.Component == component).First();


            if (_LogicalRung.Components.Where(x => x.LeftLide == component.LogicComponent.LeftLide && x.RightLide == component.LogicComponent.RightLide).Count() > 1)
            {
                foreach (ComponentGridPosition item in GetAllBetween(component.LogicComponent.LeftLide, component.LogicComponent.RightLide).Where(x => x.Row > _component.Row)) item.SetPossition(item.Row - 1, item.Column);
            }
            else
            {
                IEnumerable<ComponentGridPosition> RR = _Components.Where(x => x.Component.LogicComponent.RightLide == component.LogicComponent.RightLide && x != _component);

                if (RR.Count() > 0)
                {
                    IEnumerable<ComponentGridPosition> column_components = _Components.Where(x => x.Column == _component.Column);

                    if (column_components.Count() == 1)
                    {
                        foreach (ComponentGridPosition item in _Components.Where(x => x.Column > _component.Column)) item.SetPossition(item.Row, item.Column - 1);
                    }
                    else
                    {
                        foreach (ComponentGridPosition item in GetAllBetween(component.LogicComponent.LeftLide, component.LogicComponent.RightLide).Where(x => x.Row > _component.Row)) item.SetPossition(item.Row - 1, item.Column);
                    }
                }
                else
                {
                    foreach (ComponentGridPosition item in _Components.Where(x => x.Component.LogicComponent.LeftLide == component.LogicComponent.RightLide)) item.SetPossition(item.Row, item.Column - 1);
                }
            }

            _Components.Remove(_component);
            _LogicalRung.Remove(component.LogicComponent);
            Children.Remove(component);

            var row = RowDefinitions.Where(x => _Components.Where(y => (y.Row == RowDefinitions.IndexOf(x))).Count() == 0);

            if (row.Count() != 0)
            {
                int row_index = RowDefinitions.IndexOf(row.First());
                foreach (ComponentGridPosition item in _Components.Where(x => x.Row > row_index)) item.SetPossition(item.Row - 1, item.Column);
                RowDefinitions.RemoveAt(row_index);
            }

            var column = ColumnDefinitions.Where(x => _Components.Where(y => (y.Column == ColumnDefinitions.IndexOf(x))).Count() == 0);

            if (column.Count() != 0)
            {
                int column_index = ColumnDefinitions.IndexOf(column.First());
                if (_component.Column >= ColumnDefinitions.Count - _OutputBlockLegth) _OutputBlockLegth--;
                foreach (ComponentGridPosition item in _Components.Where(x => x.Column > column_index)) item.SetPossition(item.Row, item.Column - 1);
                ColumnDefinitions.RemoveAt(column_index);
            }

            return this;        
        }

        /// <summary>
        /// Clear Rung (delete all components)
        /// </summary>
        public RungUI Clear()
        {
            Children.Clear();
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            _LogicalRung.Clear();
            _Components.Clear();
            _Wires.Clear();
            AddColumn();
            AddRow();
            return this;
        }
        #endregion Delete Functions

        #region Wiring Functions
        public RungUI PlaceWires()
        {
            foreach (var wire in _Wires) Children.Remove(wire);
            _Wires.Clear();

            if (_Components.Count == 0) return this;

            #region Parallel Wiring
            List<Core.Components.Node> Nodes = new List<Core.Components.Node>();
            IEnumerable<Core.Components.Node> LN = _LogicalRung.Components.Select(x => x.LeftLide);
            IEnumerable<Core.Components.Node> DLN = LN.Distinct();
            IEnumerable<Core.Components.Node> RN = _LogicalRung.Components.Select(x => x.RightLide);
            IEnumerable<Core.Components.Node> DRN = RN.Distinct();

            Nodes.AddRange(DLN.Where(x => LN.Count(y => y == x) > 1));
            Nodes.AddRange(DRN.Where(x => RN.Count(y => y == x) > 1));
            Nodes = Nodes.Distinct().ToList();
            if (Nodes.Contains(_LogicalRung.Components.Last().RightLide)) Nodes.Remove(_LogicalRung.Components.Last().RightLide);

            foreach(var node in Nodes)
            {
                var temp = _Components.Where(x => x.Component.LogicComponent.LeftLide == node || x.Component.LogicComponent.RightLide == node);
                var tempL = temp.Where(x => x.Component.LogicComponent.LeftLide == node);
                var tempR = temp.Where(x => x.Component.LogicComponent.RightLide == node);

                int col = 0,
                    sr = 0,
                    er = 0;

                if (tempL.Count() > 0)
                {
                    col = tempL.Min(x => x.Column);
                    sr = tempL.Min(x => x.Row);
                    er = tempL.Max(x => x.Row);
                }

                if (tempR.Count() > 0)
                {
                    col = Math.Max(col, tempR.Max(x => x.Column));
                    sr = Math.Min(sr, tempR.Min(x => x.Row));
                    er = Math.Max(er, tempR.Max(x => x.Row));
                }

                ComponentUI.VerticalWire wire = new VerticalWire(node, col, sr, er);
                _Wires.Add(wire);
            }
            #endregion

            #region Serial Wiring
            for (int row = 0; row < RowDefinitions.Count; row++)
            {
                foreach (var component in _Components.Where(x => x.Row == row))
                {
                    IEnumerable<ComponentGridPosition> temp;

                    if (component.Component.LogicComponent.Class != Core.Components.ComponentBase.ComponentClass.Output)
                    {
                        temp = _Components.Where(x => x.Component.LogicComponent.LeftLide == component.Component.LogicComponent.RightLide);
                        if (temp.Count() > 0)
                        {
                            int ec = temp.Min(x => x.Column);

                            if (ec > component.Column + 1)
                            {
                                ComponentUI.HorizontalWire wire = new HorizontalWire(component.Component.LogicComponent.RightLide, row, component.Column + 1, ec);
                                _Wires.Add(wire);
                            }
                        }
                    }

                    temp = _Components.Where(x => x.Component.LogicComponent.RightLide == component.Component.LogicComponent.LeftLide);

                    if (temp.Count() > 0)
                    {
                        int sc = temp.Max(x => x.Column);
                        if (sc < component.Column - 1 && _Components.Where(x => x.Row == component.Row && x.Column < component.Column && x.Column > sc).Count() == 0)
                        {
                            ComponentUI.HorizontalWire wire = new HorizontalWire(component.Component.LogicComponent.LeftLide, row, sc + 1, component.Column);
                            _Wires.Add(wire);
                        }
                    }
                }
            }
            #endregion

            foreach (var wire in _Wires) Children.Insert(0, wire);

            return this;
        }
        #endregion Wiring Functions

        #endregion Functions

        public RungUI()
        {
            _Components = new List<ComponentGridPosition>();
            _LogicalRung = new Core.Components.Rung();
            _Wires = new List<Node>();
            _OutputBlockLegth = 0;
            Clear();
        }

        #region Internal Data
        private List<ComponentGridPosition> _Components;
        private Core.Components.Rung _LogicalRung;
        private List<ComponentUI.Node> _Wires;
        private int _OutputBlockLegth;
        #endregion Internal Data

        #region Internal Class
        class ComponentGridPosition
        {
            public ComponentUIBase Component { get; private set; }

            public int Row
            {
                get { return _Row; }
                private set
                {
                    _Row = value;
                    if (Component != null) Grid.SetRow(Component, value);
                }
            }
            private int _Row;

            public int Column
            {
                get { return _Column; }
                private set
                {
                    _Column = value;
                    if (Component != null) Grid.SetColumn(Component, value);
                }
            }
            private int _Column;

            public ComponentGridPosition(ComponentUIBase component, int row, int column)
            {
                Component = component;
                Row = row;
                Column = column;
            }

            public void SetPossition(int row, int column)
            {
                Row = row;
                Column = column;
            }
        }
        #endregion Internal Class
    }
}
