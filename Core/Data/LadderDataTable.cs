using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Core.Data
{
    /// <summary>
    /// LadderIno main core "RAM"
    /// this class manages the entire storage and manipulation of variables, including memory allocation 
    /// </summary>
    public class LadderDataTable
    {
        #region Properties
        /// <summary>
        /// Get how many variables are currently stored in core’s memory
        /// </summary>
        public int Count
        {
            get { return Table.Count; }
        }
        #endregion Properties

        #region Functions
        /// <summary>
        /// Manage memory allocation.
        /// In case of an existing variable it only increase its reference counter
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="dataType">Variable data type</param>
        /// <param name="type">Variable type</param>
        public LadderDataTable Add(string name, Type dataType, LDVarClass type = LDVarClass.Data)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Variable name must be provided", "name");
            if (dataType == null) throw new ArgumentNullException("Variable type must be provided", "dataType");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (dataType != Table.Values[index].DataType) throw new ArgumentException("Name already used", "name", new FormatException("Value Type Mismatch"));
                Table.Values[index].NumRefs++;
                Trace.WriteLine("New reference to variable " + name + ", total number of references: " + Table.Values[index].NumRefs, "Ladder Data Table");
            }
            else
            {
                Table.Add(name, new LDIVariable(dataType, type));
                Trace.WriteLine("New Variable inserted: " + name + ", Type: " + dataType, "Ladder Data Table");
            }

            return this;
        }

        /// <summary>
        /// Manage memory allocation and set its value
        /// In case of an existing variable it only increase its reference counter
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="dataType">Variable data type</param>
        public LadderDataTable Add(string name, Type dataType, LDVarClass type, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Variable name must be provided", "name");
            if (dataType == null) throw new ArgumentNullException("Variable type must be provided", "dataType");
            if (value == null && Nullable.GetUnderlyingType(dataType) == null) throw new ArgumentNullException("Variable value must be provided for non-nullable types", "value");
            if (value.GetType() != dataType) throw new ArgumentException("Type Mismatch", "type");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (dataType != Table.Values[index].DataType) throw new ArgumentException("Name already used", "name", new FormatException("Value Type Mismatch"));
                Table.Values[index].NumRefs++;
                Table.Values[index].Value = value;
                Trace.WriteLine("New reference to variable " + name + ", total number of references: " + Table.Values[index].NumRefs + ", Value set: " + value, "Ladder Data Table");
            }
            else
            {
                Table.Add(name, new LDIVariable(dataType, type, value));
                Trace.WriteLine("New Variable inserted: " + name + ", Type: " + dataType + ", Value set: " + value, "Ladder Data Table");
            }
            return this;
        }

        /// <summary>
        /// Manage memory free
        /// In case of a variable with multiple references it only decrease variable’s reference counter
        /// </summary>
        /// <param name="name">Variable name</param>
        public LadderDataTable Remove(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                Table.Values[index].NumRefs--;
                Trace.WriteLine("Reference to variable " + name + " removed, total number of references: " + Table.Values[index].NumRefs, "Ladder Data Table");

                if (Table.Values[index].NumRefs == 0)
                {
                    Table.RemoveAt(index);
                    Trace.WriteLine("Variable " + name + " removed from data table", "Ladder Data Table");
                }
            }
            else throw new ArgumentException("Variable not found", "name");
            return this;
        }
        
        /// <summary>
        /// Manage variable rename attempt.
        /// In case of a single reference variable it renames directly, otherwise it will try to add a variable with the new name 
        /// </summary>
        /// <param name="oldName">Variable old name</param>
        /// <param name="newName">Variable new name</param>
        public LadderDataTable Rename(string oldName, string newName)
        {
            Trace.WriteLine("Rename invoked", "Ladder Data Table");
            if (string.IsNullOrEmpty(oldName)) throw new ArgumentNullException("Variable old name must be provided", "oldName");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentNullException("Variable new name must be provided", "newName");

            int indexO = Table.IndexOfKey(oldName), 
                indexN = Table.IndexOfKey(newName);

            if (indexO != -1)
            {
                if(Table.Values[indexO].NumRefs == 1)
                {
                    if (indexN != -1) //new name exists in list
                    {
                        if (Table.Values[indexO].DataType != Table.Values[indexN].DataType) throw new ArgumentException("Name already used", "newName", new FormatException("Value Type Mismatch"));
                        else Table.RemoveAt(indexO);
                        Trace.WriteLine("New variable name already exists, reference added", "Ladder Data Table");
                    }
                    else //new name does not exist in list
                    {
                        var temp = new LDIVariable(Table.Values[indexO].DataType, Table.Values[indexO].Class, Table.Values[indexO].Value);
                        Table.RemoveAt(indexO);
                        Table.Add(newName, temp);
                        Trace.WriteLine(oldName + " Renamed to " + newName, "Ladder Data Table");
                    }
                }
                else
                {
                    Trace.WriteLine("Multiple references to " + oldName + " detected, trying to add " + newName, "Ladder Data Table");
                    Table.Values[indexO].NumRefs--;
                    Table.Add(newName, new LDIVariable(Table.Values[indexO].DataType, Table.Values[indexO].Class, Table.Values[indexO].Value));
                    Trace.WriteLine("New Variable inserted: " + newName + ", Type: " + Table.Values[indexO].DataType + ", Value set: " + Table.Values[indexO].Value, "Ladder Data Table");
                }
            }
            else throw new ArgumentException("Variable name not found", "oldName");
            return this;
        }

        /// <summary>
        /// Get a variable index in data table based on its name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable index</returns>
        public int GetIndexOf(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);
            Trace.WriteLineIf(index != -1, "Variable '" + name + "' found at index: " + index, "Ladder Data Table");
            if (index != -1) return index;

            throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Get a variable's name by index
        /// </summary>
        /// <param name="index">Variable index</param>
        /// <returns>Variable value</returns>
        public string GetName(int index)
        {
            if (index < 0 || index >= Table.Count) throw new ArgumentOutOfRangeException("Index is not inside table limits");
            Trace.WriteLine("Name at [" + index + "]: " + Table.Keys[index], "Ladder Data Table");
            return Table.Keys[index];
        }

        /// <summary>
        /// Get all names in data table
        /// </summary>
        /// <returns></returns>
        public IList<string> GetNames()
        {
            return Table.Keys;
        }

        /// <summary>
        /// Get if an variable name exists in data table
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>True for existing variable</returns>
        public bool Contains(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            return Table.ContainsKey(name);
        }

        /// <summary>
        /// Get a variable value by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable value</returns>
        public object GetValue(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);
            Trace.WriteLineIf(index != -1, "Variable " + name + " found at index: " + index + ", Value: " + Table.Values[index].Value, "Ladder Data Table");
            if (index != -1) return Table.Values[index].Value;
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Get a variable value by index
        /// </summary>
        /// <param name="index">Variable index</param>
        /// <returns>Variable value</returns>
        public object GetValue(int index)
        {
            if (index < 0 || index >= Table.Count) throw new ArgumentOutOfRangeException("Index is not inside table limits");
            Trace.WriteLine("Value at [" + index + "] (" + Table.Keys[index] + "): " + Table.Values[index].Value, "Ladder Data Table");
            return Table.Values[index].Value;
        }

        /// <summary>
        /// Set variable value by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetValue(string name, object value)
        {
            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (value.GetType() != Table.Values[index].DataType) throw new FormatException("Value Type Mismatch");
                if (!Table.Values[index].Locked)
                {
                    Trace.WriteLine("Value of " + name + " changed from " + Table.Values[index].Value + " to " + value, "Ladder Data Table");
                    Table.Values[index].Value = value;
                }
                else 
                {
                    Trace.WriteLine("Change in value of " + name + " ignored", "Ladder Data Table");
                }
                return this;
            }
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Set variable value by index
        /// </summary>
        /// <param name="index">Variable index</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetValue(int index, object value)
        {
            if (index < 0 || index >= Table.Count) throw new ArgumentOutOfRangeException("Index is not inside table limits");
            if (value.GetType() != Table.Values[index].DataType) throw new FormatException("Value Type Mismatch");

            if (!Table.Values[index].Locked)
            {
                Trace.WriteLine("Value at [" + index + "] (" + Table.Keys[index] + "): changed from " + Table.Values[index].Value + " to " + value, "Ladder Data Table");
                Table.Values[index].Value = value;
            }
            else
            {
                Trace.WriteLine("Change in value at [" + index + "] (" + Table.Keys[index] + ") ignored", "Ladder Data Table");
            }

            return this;
        }

        /// <summary>
        /// Set variable class by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetVarClass(string name, LDVarClass value)
        {
            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (Table.Values[index].NumRefs > 1 && Table.Values[index].Class != value) throw new InvalidOperationException("Can't change variable LD Class");

                Trace.WriteLine("Class of " + name + " changed to " + value, "Ladder Data Table");
                Table.Values[index].Class = value;
                
                return this;
            }
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Set variable class by index
        /// </summary>
        /// <param name="index">Variable index</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetVarClass(int index, LDVarClass value)
        {
            if (index < 0 || index >= Table.Count) throw new ArgumentOutOfRangeException("Index is not inside table limits");
            if (Table.Values[index].NumRefs > 1 && Table.Values[index].Class != value) throw new InvalidOperationException("Can't change variable LD Class");

            Trace.WriteLine("Class at [" + index + "] (" + Table.Keys[index] + "): changed to " + value, "Ladder Data Table");
            Table.Values[index].Class = value;

            return this;
        }

        /// <summary>
        /// Set Variable lock state by index
        /// </summary>
        /// <param name="index">Variable index</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetLock(int index, bool value)
        {
            if (index < 0 || index >= Table.Count) throw new ArgumentOutOfRangeException("Index is not inside table limits");
            Trace.WriteLine("Value at [" + index + "] (" + Table.Keys[index] + "): lock state changed to " + value, "Ladder Data Table");

            Table.Values[index].Locked = value;
            return this;
        }

        /// <summary>
        /// Set variable lock state by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Value to be set</param>
        public LadderDataTable SetLock(string name, object value)
        {
            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (value.GetType() != Table.Values[index].DataType) throw new FormatException("Value Type Mismatch");
                Trace.WriteLine("Lock state of " + name + " changed to " + value, "Ladder Data Table");
                Table.Values[index].Value = value;
                return this;
            }
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Lock variable by name
        /// </summary>
        /// <param name="name">Variable name</param>
        public LadderDataTable Lock(string name)
        {
            SetLock(name, true);
            return this;
        }

        /// <summary>
        /// Lock variable by index
        /// </summary>
        /// <param name="index">Variable index</param>
        public LadderDataTable Lock(int index)
        {
            SetLock(index, true);
            return this;
        }

        /// <summary>
        /// Unlock variable by name
        /// </summary>
        /// <param name="name">Variable name</param>
        public LadderDataTable Unlock(string name)
        {
            SetLock(name, false);
            return this;
        }

        /// <summary>
        /// Unlock variable by index
        /// </summary>
        /// <param name="index">Variable index</param>
        public LadderDataTable Unlock(int index)
        {
            SetLock(index, false);
            return this;
        }

        /// <summary>
        /// Retrieve all data stored data
        /// </summary>
        /// <returns>Tuple (Name, DataType, VarClass, Value)</returns>
        public List<Tuple<string, Type, LDVarClass, object>> ListAllData()
        {
            return Table.Select(x => new Tuple<string, Type, LDVarClass, object>(x.Key, x.Value.DataType, x.Value.Class, x.Value.Value)).ToList();
        }
        #endregion Functions

        #region Constructors
        /// <summary>
        /// Default builder 
        /// </summary>
        public LadderDataTable()
        {
            Table = new SortedList<string, LDIVariable>();
            Trace.WriteLine("New LDI Variable table started", "Ladder Data Table");
        }
        #endregion Constructors

        #region Internal Data
        SortedList<string, LDIVariable> Table { get; set; }
        #endregion Internal Data

        #region Classes
        /// <summary>
        /// Class that represents one memory entry
        /// </summary>
        private class LDIVariable : ICloneable, INotifyPropertyChanged
        {
            public object Value
            {
                get { return _Value; }
                set
                {
                    _Value = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
                }
            }

            public int NumRefs
            {
                get { return _NumRefs; }
                set
                {
                    _NumRefs = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NumRefs")); }
                }
            }

            public Type DataType
            {
                get { return _DataType; }
                set
                {
                    _DataType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DataType")); }
                }
            }

            public LDVarClass Class
            {
                get { return _Type; }
                set
                {
                    _Type = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Type")); }
                }
            }

            public bool Locked
            {
                get { return _Locked; }
                set
                {
                    _Locked = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Locked")); }
                }
            }

            /// <summary>
            /// Master builder 
            /// </summary>
            /// <param name="dataType">Variable data type</param>
            /// <param name="type">Variable type</param>
            /// <param name="numRefs">Number of references to this variable</param>
            /// <param name="value">Variable initial value</param>
            public LDIVariable(Type dataType, LDVarClass type, int numRefs, object value)
            {
                Value = value;
                NumRefs = numRefs;
                DataType = dataType;
                Class = type;
                Locked = false;
            }

            /// <summary>
            /// Builder (sets type's default value)
            /// </summary>
            /// <param name="dataType">Variable type</param>
            public LDIVariable(Type dataType, LDVarClass type)
                : this(dataType, type, 1, null)
            {
                //get default value for the given type
                Value = (dataType.IsValueType) ? Activator.CreateInstance(dataType) : null;
                Locked = false;
            }

            /// <summary>
            /// Builder 
            /// </summary>
            /// <param name="dataType">Variable type</param>
            /// <param name="value">Variable initial value</param>
            public LDIVariable(Type dataType, LDVarClass type, object value)
                : this(dataType, type, 1, value)
            {
            }

            public object Clone()
            {
                return new LDIVariable(DataType, Class, NumRefs, Value);
            }

            #region Internal Data
            public event PropertyChangedEventHandler PropertyChanged;
            private object _Value;
            private int _NumRefs;
            private Type _DataType;
            private LDVarClass _Type;
            private bool _Locked;
            #endregion Internal Data

        }
        #endregion Classes
    }
}
