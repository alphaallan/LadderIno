using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ComponentUI
{
    public class Rung : Grid
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
        public Rung AddRow()
        {
            var row = new RowDefinition();
            row.Height = GridLength.Auto;
            RowDefinitions.Add(row);
            return this;
        }

        public Rung AddRow(int pos)
        {
            var row = new RowDefinition();
            row.Height = GridLength.Auto;
            RowDefinitions.Insert(pos, row);
            return this;
        }

        public Rung AddColumn()
        {
            var col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            ColumnDefinitions.Insert(0, col);
            return this;
        }

        public Rung AddColumn(double width, GridUnitType unit)
        {
            var col = new ColumnDefinition();
            col.Width = new GridLength(width, unit);
            ColumnDefinitions.Insert(0, col);
            return this;
        }

        public Rung AddColumn(int pos)
        {
            var col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            ColumnDefinitions.Insert(pos, col);
            return this;
        }

        public Rung AddColumn(int pos, double width, GridUnitType unit)
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
        #endregion Help Function

        /// <summary>
        /// Insert component with auto select position 
        /// </summary>
        /// <param name="component">Component to be added</param>
        public Rung Add(ComponentUIBase component)
        {
            _LogicalRung.Add(component.LogicComponent);

            if (component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output)
            {
                var temp = _Components.Where(x => x.Component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output);

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
        public Rung InsertAbove(ComponentUIBase component, ComponentUIBase anchor)
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

            IEnumerable<ComponentGridPosition> move_list = GetAllBetween(NodeA, NodeB).Where(x => x.Row > _anchor.Row);

            _LogicalRung.InsertAbove(component.LogicComponent, anchor.LogicComponent);

            if (!IsSlotEmpty(RowDefinitions.Count - 1, _anchor.Column)) AddRow(_anchor.Row);

            foreach (ComponentGridPosition item in move_list) item.SetPossition(item.Row + 1, item.Column);

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
        public Rung InsertUnder(ComponentUIBase component, ComponentUIBase anchor)
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

            IEnumerable<ComponentGridPosition> move_list = GetAllBetween(NodeA, NodeB).Where(x => x.Row > _anchor.Row);

            _LogicalRung.InsertAbove(component.LogicComponent, anchor.LogicComponent);

            if (!IsSlotEmpty(RowDefinitions.Count - 1, _anchor.Column)) AddRow(_anchor.Row);

            foreach (ComponentGridPosition item in move_list) item.SetPossition(item.Row + 1, item.Column);

            _Components.Add(new ComponentGridPosition(component, _anchor.Row + 1, _anchor.Column));

            Children.Add(component);
            return this;  
        }

        /// <summary>
        /// Insert before anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertBefore(ComponentUIBase component, ComponentUIBase anchor)
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
                    move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column) && (!(parallel.Contains(x.Component.LogicComponent)) || x.Row == _anchor.Row)));
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
        public Rung InsertBefore(ComponentUIBase component, Node anchor)
        {
            return this;        
        }

        /// <summary>
        /// Insert after anchor component
        /// </summary>
        /// <param name="component">New component</param>
        /// <param name="anchor">Anchor component</param>
        public Rung InsertAfter(ComponentUIBase component, ComponentUIBase anchor)
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
                move_list.AddRange(_Components.Where(x => (x.Column > _anchor.Column) && (!(parallel.Contains(x.Component.LogicComponent)) || x.Row == _anchor.Row)));
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
        public Rung InsertAfter(ComponentUIBase component, Node anchor)
        {
            return this;        
        }
        #endregion Insert Functions

        #region Delete Functions
        /// <summary>
        /// Delete compoenent from the rung
        /// </summary>
        /// <param name="component">Component to be deleted</param>
        public Rung Remove(ComponentUIBase component)
        {
            return this;        
        }

        /// <summary>
        /// Clear Rung (delete all components)
        /// </summary>
        public Rung Clear()
        {
            Children.Clear();
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            _LogicalRung.Clear();
            _Components.Clear();
            AddColumn();
            AddRow();
            return this;
        }
        #endregion Delete Functions

        #endregion Functions


        public Rung()
        {
            _Components = new List<ComponentGridPosition>();
            _LogicalRung = new Core.Components.Rung();
            Clear();
        }

        #region Internal Data
        private List<ComponentGridPosition> _Components;
        private Core.Components.Rung _LogicalRung;
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
