using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ladderino.ViewModel
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public class KeyValuePair<Tkey, Tvalue> : INotifyPropertyChanged
        {
            public KeyValuePair(Tkey key, Tvalue val)
            {
                Key = key;
                Value = val;
            }

            public Tkey Key
            {
                get { return _Key; }
                set
                {
                    _Key = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Key")); }
                }
            }

            public Tvalue Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public override string ToString()
            {
                return this.Key.ToString() + " - " + this.Value.ToString();
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                KeyValuePair<Tkey, Tvalue> objCast = obj as KeyValuePair<Tkey, Tvalue>;
                if (objCast == null) return false;
                else return Equals(objCast);
            }
            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
            public bool Equals(KeyValuePair<Tkey, Tvalue> other)
            {
                if (other == null) return false;
                return (this.Key.Equals(other.Key));
            }

            private Tkey _Key;
            private Tvalue _Value;
        }

        public class KeyValueCollection<Tkey, Tvalue> : ObservableCollection<KeyValuePair<Tkey, Tvalue>>
        {

        }

        /// <summary>
        /// https://code.msdn.microsoft.com/windowsdesktop/Easy-MVVM-Examples-fb8c409f
        /// </summary>
        public class RelayCommand : ICommand
        {
            #region Fields

            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            #endregion // Fields

            #region Constructors

            public RelayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }
            #endregion // Constructors

            #region ICommand Members

            public bool CanExecute(object param)
            {
                return _canExecute == null ? true : _canExecute(param);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object param)
            {
                _execute(param);
            }

            #endregion // ICommand Members
        }

    }
}
