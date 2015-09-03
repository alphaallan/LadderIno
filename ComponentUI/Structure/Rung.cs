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
            foreach (var item in _Components) item.SetPossition(item.Row, item.Column + 1);
            return this;
        }

        public Rung AddColumn(double width, GridUnitType unit)
        {
            var col = new ColumnDefinition();
            col.Width = new GridLength(width, unit);
            ColumnDefinitions.Insert(0, col);
            foreach (var item in _Components) item.SetPossition(item.Row, item.Column + 1);
            return this;
        }

        public Rung AddColumn(int pos)
        {
            var col = new ColumnDefinition();
            col.Width = GridLength.Auto;
            ColumnDefinitions.Insert(pos, col);
            foreach (var item in _Components.Where(x => pos < x.Column)) item.SetPossition(item.Row, item.Column + 1);
            return this;
        }

        public Rung AddColumn(int pos, double width, GridUnitType unit)
        {
            var col = new ColumnDefinition();
            col.Width = new GridLength(width, unit);
            ColumnDefinitions.Insert(pos, col);
            foreach (var item in _Components.Where(x => pos < x.Column)) item.SetPossition(item.Row, item.Column + 1);
            return this;
        }

        public bool IsSlotEmpty(int row, int col)
        {
            return ((row < RowDefinitions.Count) && 
                    (col < ColumnDefinitions.Count) && 
                    (_Components.Where(x => x.Row == row && x.Column == col).Count() == 0));
        }

        private List<ComponentGridPosition> FindComponentToMove(ComponentUIBase anchor)
        {
            var list = new List<ComponentGridPosition>();

            foreach (var item in _Components.Where(x => x.Component.LogicComponent.LeftLide == anchor.LogicComponent.LeftLide))
            {
                list.Add(item);
                var node = item.Component.LogicComponent.RightLide;
                if (item.Component.LogicComponent.RightLide != anchor.LogicComponent.RightLide) list.AddRange(FindComponentToMove(item.Component));
            }

            return list;
        }

        private List<ComponentGridPosition> GetAllBetween(Core.Components.Node startNode, Core.Components.Node endNode)
        {
            var logicList = _LogicalRung.GetAllBetween(startNode, endNode);
            return _Components.Where(x => logicList.Contains(x.Component.LogicComponent)).ToList();
        }

        public Core.Components.Node FindInterception(ComponentUIBase componentA, ComponentUIBase componentB)
        {
            if (componentA == componentB) throw new Exception("Components A and B are the same");

            var logic_components = _LogicalRung.Components;

            int indexA = logic_components.IndexOf(componentA.LogicComponent);
            int indexB = logic_components.IndexOf(componentB.LogicComponent);
            
            if (indexA == -1) throw new ArgumentException("Component not inserted in current Rung", "componentA");
            if (indexB == -1) throw new ArgumentException("Component not inserted in current Rung", "componentB");

            if (indexA < indexB)
            {
                int temp = indexA;
                indexA = indexB;
                indexB = temp;
            }

            int finishA = indexB - 1;
            int finishB = indexB;
            while (finishB < logic_components.Count && logic_components[finishB].RightLide != logic_components[finishA].RightLide) finishB++;

            if(finishB < logic_components.Count) throw new Exception("Interception not found");

            return logic_components[finishA].RightLide;
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
            _LogicalRung.InsertAbove(component.LogicComponent, anchor.LogicComponent);
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column));

            if (!IsSlotEmpty(RowDefinitions.Count - 1, _anchor.Column)) AddRow(_anchor.Row);

            var column_components = _Components.Where(x => (x.Column == _anchor.Column));
            //Core.Components.Node nodeA =

            var move_list = GetAllBetween(anchor.LogicComponent.LeftLide, anchor.LogicComponent.RightLide).Where(x => x.Row > _anchor.Row);

            foreach (ComponentGridPosition item in move_list) item.SetPossition(item.Row + 1, item.Column);

            if (component.LogicComponent.Class == Core.Components.ComponentBase.ComponentClass.Output)
            {
                
            }
            else
            {
                
            }

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
            _LogicalRung.InsertUnder(component.LogicComponent, anchor.LogicComponent);
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
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
            _LogicalRung.InsertBefore(component.LogicComponent, anchor.LogicComponent);
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column));


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
            _LogicalRung.InsertAfter(component.LogicComponent, anchor.LogicComponent);
            ComponentGridPosition _anchor = _Components.Where(x => x.Component == anchor).First();
            _Components.Add(new ComponentGridPosition(component, _anchor.Row, _anchor.Column + 1));


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
