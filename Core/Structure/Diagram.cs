using Core.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Core.Components
{
    /// <summary>
    /// Container class for the ladder diagram
    /// </summary>
    public class Diagram : INotifyPropertyChanged
    {
        #region Properties

        public ObservableCollection<Rung> Rungs
        {
            get { return _Rungs; }
            private set
            {
                _Rungs = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Rungs")); }                
            }
        }

        /// <summary>
        /// Rungs datacontext
        /// </summary>
        public LadderDataTable DataTable
        {
            get { return _DataTable; }
            set
            {
                if (value.Count != 0) throw new ArgumentException("Cannot use a non-empty data table", "DataTable");
                
                if (_DataTable != null)
                {
                    _DataTable.VariableAdded -= DataTableVarAdded;
                    _DataTable.VariableClassChanged -= DataTableVarClassChanged;
                    _DataTable.VariableRemoved -= DataTableVarRemoved;
                    _DataTable.VariableRenamed -= DataTableVarRenamed;
                }

                _DataTable = value;

                if (_DataTable != null)
                {
                    _DataTable.VariableAdded += DataTableVarAdded;
                    _DataTable.VariableClassChanged += DataTableVarClassChanged;
                    _DataTable.VariableRemoved += DataTableVarRemoved;
                    _DataTable.VariableRenamed += DataTableVarRenamed;
                }

                foreach (Rung rung in _Rungs) rung.DataTable = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DataTable")); }
            }
        }

        /// <summary>
        /// Diagram Pinout
        /// </summary>
        public ObservableCollection<LDPin> Pins
        {
            get { return _Pins; }
            private set
            {
                _Pins = value;
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Pins")); }
            }
        }


        /// <summary>
        /// Master control relay, it is used to turn the hole program on and off. 
        /// It can also be used to abort program’s execution
        /// </summary>
        public bool MasterRelay
        {
            get { return _MasterRelay; }
            set
            {
                _MasterRelay = value;
                Trace.WriteLine("MasterRelay seted to: " + value, "Diagram");
                if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MasterRelay")); }
            }
        }

        /// <summary>
        /// Get the current number of rows in diagram 
        /// </summary>
        public int Count
        {
            get { return _Rungs.Count; }
        }
        #endregion Properties

        #region Functions
        /// <summary>
        /// Execute a program cycle
        /// </summary>
        public Diagram Execute()
        {
            if (_DataTable == null) throw new NullReferenceException("No data table associated to the diagram");
            if (MasterRelay)
            {
                Trace.WriteLine("Diagram execution started", "Diagram");
                Trace.Indent();
                foreach (Rung rung in _Rungs) rung.Execute();
                Trace.Unindent();
            }

            return this;
        }

        public Diagram RefreshPins()
        {
            if (DataTable != null)
            {
                List<Tuple<string, LDVarClass>> variables = DataTable.ListAllData()
                                                                          .Where(x => x.Item3.IsPin())
                                                                          .Select(x => new Tuple<string, LDVarClass>(x.Item1, x.Item3)).ToList();

                IEnumerable<string> varNames = variables.Select(x => x.Item1);
                IEnumerable<string> pinNames = _Pins.Select(x => x.Variable);

                foreach (string exItem in pinNames.Except(varNames)) _Pins.Remove(_Pins.Where(x => x.Variable == exItem).First());
                foreach (var item in variables) if (!pinNames.Contains(item.Item1)) _Pins.Add(new Data.LDPin(item.Item1, item.Item2.ToPin()));

            }
            else _Pins.Clear();

            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Pins")); }

            return this;
        }
        
        #region Insert Functions
        /// <summary>
        /// Add a new rung in the bottom of the diagram
        /// </summary>
        public void Add()
        {
            Add(new Rung());
        }

        /// <summary>
        /// Add an existing rung in the bottom of the diagram
        /// </summary>
        /// <param name="rung">Rung to be inserted</param>
        public void Add(Rung rung)
        {
            Trace.WriteLine("Auto insert called -> ", "Diagram");
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            Trace.Indent();
            _Rungs.Add(rung);
            Trace.WriteLine("Rung inserted", "Diagram");
            rung.DataTable = DataTable;
            Trace.Unindent();
        }

        /// <summary>
        /// Insert a new rung above the anchor rung
        /// </summary>
        /// <param name="anchor">Anchor rung</param>
        public void InsertAbove(Rung anchor)
        {
            InsertAbove(new Rung(), anchor);
        }

        /// <summary>
        /// Insert an existing rung above an anchor rung
        /// </summary>
        /// <param name="rung">Rung to be inserted</param>
        /// <param name="anchor">Anchor rung</param>
        public void InsertAbove(Rung rung, Rung anchor)
        {
            Trace.WriteLine("Insert above called", "Diagram");
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            int anchorIndex = _Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                Trace.Indent();
                _Rungs.Insert(anchorIndex, rung);
                Trace.WriteLine("Rung inserted", "Diagram");
                rung.DataTable = DataTable;
                Trace.Unindent();
            }
            else throw new ArgumentException("Anchor rung is not inserted in current diagram", "anchor");
        }

        /// <summary>
        /// Insert a new rung under the anchor rung
        /// </summary>
        /// <param name="anchor">Anchor rung</param>
        public void InsertUnder(Rung anchor)
        {
            InsertUnder(new Rung(), anchor);
        }

        /// <summary>
        /// Insert an existing rung under an anchor rung
        /// </summary>
        /// <param name="rung">Rung  to be inserted</param>
        /// <param name="anchor">Anchor rung</param>
        public void InsertUnder(Rung rung, Rung anchor)
        {
            Trace.WriteLine("Insert under called -> ", "Diagram");
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            int anchorIndex = _Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                Trace.Indent();
                _Rungs.Insert(anchorIndex + 1, rung);
                Trace.WriteLine("Rung inserted", "Diagram");
                rung.DataTable = DataTable;
                Trace.Unindent();
            }
            else throw new ArgumentException("Anchor rung is not inserted in current diagram", "anchor");
        }

        #endregion Insert Functions

        #region Delete Functions
        /// <summary>
        /// Remove a rung from the ladder diagram
        /// </summary>
        /// <param name="rung">Rung to be removed</param>
        public void Remove(Rung rung)
        {
            Trace.WriteLine("Remove called", "Diagram");
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            if (_Rungs.Contains(rung))
            {
                Trace.Indent();
                _Rungs.Remove(rung);
                Trace.WriteLine("Rung removed", "Diagram");
                GC.Collect(); // Call gabarge collector
                Trace.Unindent();
            }
            else throw new ArgumentException("Rung is not inserted in current diagram", "rung");

        }

        /// <summary>
        /// Remove all rungs from the diagram
        /// </summary>
        public void Clear()
        {
            Trace.WriteLine("Diagram Clear invoked", "Diagram");
            _Rungs.Clear();
            _Pins.Clear();
            Trace.Indent();
            GC.Collect();
            Trace.Unindent();
            Trace.WriteLine("Diagram cleared", "Diagram");
        }
        #endregion Delete Functions

        #endregion Functions

        #region Constructors
        /// <summary>
        /// Default Builder
        /// </summary>
        public Diagram()
        {
            Trace.WriteLine("New diagram created", "Diagram");
            Rungs = new ObservableCollection<Rung>();
            Pins = new ObservableCollection<Data.LDPin>();
            DataTable = new Data.LadderDataTable();
        }
        #endregion Constructors

        #region Internal Data
        ObservableCollection<Rung> _Rungs;
        bool _MasterRelay;

        private Data.LadderDataTable _DataTable;
        private ObservableCollection<Data.LDPin> _Pins;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data

        #region Event Handlers

        #region DataTable Event Handlers
        private void DataTableVarAdded(object sender, Data.VarAddedArgs e)
        {
            if (sender == _DataTable && e.VarClass.IsPin() && _Pins.Count(x => x.Variable == e.Name) == 0) _Pins.Add(new Data.LDPin(e.Name, e.VarClass.ToPin()));
        }

        private void DataTableVarRemoved(object sender, Data.VarRemovedArgs e)
        {
            if (sender == _DataTable && _Pins.Count(x => x.Variable == e.Name) > 0) _Pins.Remove(_Pins.Where(x => x.Variable == e.Name).First());
        }

        private void DataTableVarRenamed(object sender, Data.VarRenamedArgs e)
        {
            if (sender == _DataTable && _Pins.Count(x => x.Variable == e.OldName) > 0) _Pins.Where(x => x.Variable == e.OldName).First().Variable = e.NewName;
        }

        private void DataTableVarClassChanged(object sender, Data.VarClassChangedArgs e)
        {
            if (sender == _DataTable && (e.OldClass.IsPin() || e.NewClass.IsPin()))
            {
                if (e.OldClass.IsPin() && !e.NewClass.IsPin())
                {
                    if (_Pins.Count(x => x.Variable == e.Name) > 0) _Pins.Remove(_Pins.Where(x => x.Variable == e.Name).First());
                }
                else if (!e.OldClass.IsPin() && e.NewClass.IsPin())
                {
                    if (_Pins.Count(x => x.Variable == e.Name) == 0) _Pins.Add(new Data.LDPin(e.Name, e.NewClass.ToPin()));
                }
                else
                {
                    if (_Pins.Count(x => x.Variable == e.Name) > 0)
                    {
                        Data.LDPin pin = _Pins.Where(x => x.Variable == e.Name).First();
                        pin.Type = e.NewClass.ToPin();
                        pin.Pin = "NONE";
                    }
                    else _Pins.Add(new Data.LDPin(e.Name, e.NewClass.ToPin()));
                }
            }
        }
        #endregion

        #endregion
    }
}
