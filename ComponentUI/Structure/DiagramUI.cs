using Core.Components;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ComponentUI
{
    public class DiagramUI : Control
    {
        static DiagramUI()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DiagramUI), new FrameworkPropertyMetadata(typeof(DiagramUI)));
        }

        public DiagramUI()
        {
            LogicalDiagram = new Diagram();
        }

        #region Properties

        public ObservableCollection<RungUI> Rungs
        {
            get { return (ObservableCollection<RungUI>)GetValue(RungsProperty); }
            set { SetValue(RungsProperty, value); }
        }
        public static readonly DependencyProperty RungsProperty =
            DependencyProperty.Register("Rungs", typeof(ObservableCollection<RungUI>), typeof(DiagramUI), new PropertyMetadata(new ObservableCollection<RungUI>()));

        public Diagram LogicalDiagram
        {
            get { return _LogicalDiagram; }
            private set { _LogicalDiagram = value; }
        }
        private Diagram _LogicalDiagram;

        public Core.Data.LadderDataTable DataTable
        {
            get { return LogicalDiagram.DataTable; }
            set { LogicalDiagram.DataTable = value; }
        }

        public bool MasterRelay
        {
            get { return LogicalDiagram.MasterRelay; }
            set { LogicalDiagram.MasterRelay = value; }
        }
        #endregion Properties

        #region Functions
        /// <summary>
        /// Execute a program cycle
        /// </summary>
        public DiagramUI Execute()
        {
            LogicalDiagram.Execute();
            return this;
        }

        public DiagramUI RefreshPins()
        {
            LogicalDiagram.RefreshPins();
            return this;
        }

        #region Insert Functions
        /// <summary>
        /// Add a new rung in the bottom of the diagram
        /// </summary>
        public void Add()
        {
            Add(new RungUI());
        }

        /// <summary>
        /// Add an existing rung in the bottom of the diagram
        /// </summary>
        /// <param name="rung">Rung to be inserted</param>
        public void Add(RungUI rung)
        {
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            Rungs.Add(rung);
            LogicalDiagram.Add(rung.LogicalRung);
            rung.DataTable = DataTable;
        }

        /// <summary>
        /// Insert a new rung above the anchor rung
        /// </summary>
        /// <param name="anchor">Anchor rung</param>
        public void InsertAbove(RungUI anchor)
        {
            InsertAbove(new RungUI(), anchor);
        }

        /// <summary>
        /// Insert an existing rung above an anchor rung
        /// </summary>
        /// <param name="rung">Rung to be inserted</param>
        /// <param name="anchor">Anchor rung</param>
        public void InsertAbove(RungUI rung, RungUI anchor)
        {
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            LogicalDiagram.InsertAbove(rung.LogicalRung, anchor.LogicalRung);

            int anchorIndex = Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                Rungs.Insert(anchorIndex, rung);
                rung.DataTable = DataTable;
            }
            else throw new ArgumentException("Anchor rung is not inserted in current diagram", "anchor");
        }

        /// <summary>
        /// Insert a new rung under the anchor rung
        /// </summary>
        /// <param name="anchor">Anchor rung</param>
        public void InsertUnder(RungUI anchor)
        {
            InsertUnder(new RungUI(), anchor);
        }

        /// <summary>
        /// Insert an existing rung under an anchor rung
        /// </summary>
        /// <param name="rung">Rung  to be inserted</param>
        /// <param name="anchor">Anchor rung</param>
        public void InsertUnder(RungUI rung, RungUI anchor)
        {
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            LogicalDiagram.InsertUnder(rung.LogicalRung, anchor.LogicalRung);

            int anchorIndex = Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                Rungs.Insert(anchorIndex + 1, rung);
                rung.DataTable = DataTable;
            }
            else throw new ArgumentException("Anchor rung is not inserted in current diagram", "anchor");
        }

        #endregion Insert Functions

        #region Delete Functions
        /// <summary>
        /// Remove a rung from the ladder diagram
        /// </summary>
        /// <param name="rung">Rung to be removed</param>
        public void Remove(RungUI rung)
        {
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            if (Rungs.Contains(rung))
            {
                Rungs.Remove(rung);
                LogicalDiagram.Rungs.Remove(rung.LogicalRung);
            }
            else throw new ArgumentException("Rung is not inserted in current diagram", "rung");

        }

        /// <summary>
        /// Remove all rungs from the diagram
        /// </summary>
        public void Clear()
        {
            Rungs.Clear();
            LogicalDiagram.Clear();
        }
        #endregion Delete Functions

        #endregion Functions

        private static void LogicalDiagramChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DiagramUI)
            {

            }
        }

        private void ReloadStack()
        {
            
        }
    }
}
