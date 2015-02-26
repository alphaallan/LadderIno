using Core.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        /// Master control relay, it is used to turn the hole program on and off. 
        /// It can also be used to abort program’s execution
        /// </summary>
        public bool MasterRelay
        {
            get { return _MasterRelay; }
            set
            {
                _MasterRelay = value;
                Trace.WriteLine("MasterRelay seted to: " + value);
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
        public void Execute()
        {
            if (MasterRelay)
            {
                Trace.WriteLine("Diagram execution started");
                foreach (Rung rung in _Rungs) rung.Execute();
            }
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
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            _Rungs.Add(rung);
            Trace.WriteLine("Rung inserted");
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
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            int anchorIndex = _Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                _Rungs.Insert(anchorIndex, rung);
                Trace.WriteLine("Rung inserted");
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
            if (rung == null) throw new ArgumentException("Null rung", "rung");
            if (anchor == null) throw new ArgumentException("Null rung", "anchor");
            if (anchor == rung) throw new ArgumentException("Anchor rung and new rung are the same", "anchor");

            int anchorIndex = _Rungs.IndexOf(anchor);
            if (anchorIndex != -1)
            {
                _Rungs.Insert(anchorIndex + 1, rung);
                Trace.WriteLine("Rung inserted");
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
            if (rung == null) throw new ArgumentException("Null rung", "rung");

            if (_Rungs.Contains(rung))
            {
                _Rungs.Remove(rung);
                Trace.WriteLine("Rung removed");
            }
            else throw new ArgumentException("Rung is not inserted in current diagram", "rung");

        }

        /// <summary>
        /// Remove all rungs from the diagram
        /// </summary>
        public void Clear()
        {
            _Rungs.Clear();
            Trace.WriteLine("Diagram cleared");
        }
        #endregion Delete Functions

        #endregion Functions

        #region Constructors
        /// <summary>
        /// Default Builder
        /// </summary>
        public Diagram()
        {
            Rungs = new ObservableCollection<Rung>();
            Trace.WriteLine("New diagram created");
        }
        #endregion Constructors

        #region Internal Data
        ObservableCollection<Rung> _Rungs;
        bool _MasterRelay;

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Internal Data
    }
}
