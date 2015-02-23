using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    /// <summary>
    /// LadderIno main core "RAM"
    /// this class manages the entire storage and manipulation of variables, including memory allocation 
    /// </summary>
    public static class LDIVariableTable
    {
        #region Properties
        /// <summary>
        /// Get how many variables are currently stored in core’s memory
        /// </summary>
        public static int Count
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
        /// <param name="type">Variable type</param>
        public static void Add(string name, Type type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");
            if (type == null) throw new ArgumentException("Variable type must be provided", "type");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (type != Table.Values[index].Type) throw new ArgumentException("Name already used", "name", new ArgumentException("Type Mismatch", "type"));
                Table.Values[index].NumRefs++;
            }
            else
            {
                Table.Add(name, new LDIVariable(type));
            }
        }

        /// <summary>
        /// Manage memory allocation and set its value
        /// In case of an existing variable it only increase its reference counter
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="type">Variable type</param>
        public static void Add(string name, Type type, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");
            if (type == null) throw new ArgumentException("Variable type must be provided", "type");
            if (value == null) throw new ArgumentException("Variable value must be provided", "value");
            if (value.GetType() != type) throw new ArgumentException("Type Mismatch", "type");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (type != Table.Values[index].Type) throw new ArgumentException("Name already used", "name", new ArgumentException("Type Mismatch", "type"));
                Table.Values[index].NumRefs++;
                Table.Values[index].Value = value;
            }
            else
            {
                Table.Add(name, new LDIVariable(type, value));
            }
        }

        /// <summary>
        /// Manage memory free
        /// In case of a variable with multiple references it only decrease variable’s reference counter
        /// </summary>
        /// <param name="name">Variable name</param>
        public static void Remove(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                Table.Values[index].NumRefs--;
                if (Table.Values[index].NumRefs == 0) Table.RemoveAt(index);
            }
            else throw new ArgumentException("Variable not found", "name");
        }
        
        /// <summary>
        /// Manage variable rename attempt.
        /// In case of a single reference variable it renames directly, otherwise it will try to add a variable with the new name 
        /// </summary>
        /// <param name="oldName">Variable old name</param>
        /// <param name="newName">Variable new name</param>
        public static void Rename(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) throw new ArgumentException("Variable old name must be provided", "oldName");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("Variable new name must be provided", "newName");

            int indexO = Table.IndexOfKey(oldName), 
                indexN = Table.IndexOfKey(newName);

            if (indexO != -1)
            {
                if(Table.Values[indexO].NumRefs == 1)
                {
                    if (indexN != -1) //new name exists in list
                    {
                        if (Table.Values[indexO].Type != Table.Values[indexN].Type) throw new ArgumentException("Name already used", "newName", new ArgumentException("Type Mismatch", "type"));
                        else Table.RemoveAt(indexO);
                    }
                    else Table.Keys[indexO] = newName; //new name does not exist in list
                }
                else
                {
                    Table.Values[indexO].NumRefs--;
                    Table.Add(newName, new LDIVariable(Table.Values[indexO].Type));
                }
            }
            else throw new ArgumentException("Variable name not found", "oldName");
        }

        /// <summary>
        /// Get a variable index in data table based on its name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable index</returns>
        public static int GetIndexOf(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);
            if (index != -1) return index;

            throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Get if an variable name exists in data table
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>True for existing variable</returns>
        public static bool Contains(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            return Table.ContainsKey(name);
        }

        /// <summary>
        /// Get a variable value by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable value</returns>
        public static object GetValue(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            int index = Table.IndexOfKey(name);
            if (index != -1) return Table.Values[index].Value;
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Get a variable value by index
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable value</returns>
        public static object GetValueAt(int index)
        {
            if (index < 0 || index >= Table.Count) throw new IndexOutOfRangeException("Index is not inside table limits");

            return Table.Values[index].Value;
        }

        /// <summary>
        /// Set variable value by name
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Value to be set</param>
        public static void SetValue(string name, object value)
        {
            int index = Table.IndexOfKey(name);

            if (index != -1)
            {
                if (value.GetType() != Table.Values[index].Type) throw new ArgumentException("Type Mismatch", "value");
                Table.Values[index].Value = value;
                return;
            }
            else throw new ArgumentException("Variable not found", "name");
        }

        /// <summary>
        /// Set variable value by index
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <param name="value">Value to be set</param>
        public static void SetValueAt(int index, object value)
        {
            if (index < 0 || index >= Table.Count) throw new IndexOutOfRangeException("Index is not inside table limits");
            if (value.GetType() != Table.Values[index].Type) throw new ArgumentException("Type Mismatch", "value");

            Table.Values[index].Value = value;
        }
        #endregion Functions

        #region Constructors
        /// <summary>
        /// Static class builder 
        /// </summary>
        static LDIVariableTable()
        {
            Table = new SortedList<string, LDIVariable>();
        }
        #endregion Constructors

        #region Internal Data
        static SortedList<string, LDIVariable> Table { get; set; }
        #endregion Internal Data

        #region Classes
        /// <summary>
        /// Class that represents one memory entry
        /// </summary>
        private class LDIVariable
        {
            public object Value { get; set; }
            public int NumRefs { get; set; }
            public Type Type { get; set; }

            /// <summary>
            /// Master builder 
            /// </summary>
            /// <param name="type">Variable type</param>
            /// <param name="numRefs">Number of references to this variable</param>
            /// <param name="value">Variable initial value</param>
            public LDIVariable(Type type, int numRefs, object value)
            {
                Value = value;
                NumRefs = numRefs;
                Type = type;
            }

            /// <summary>
            /// Builder (sets type's default value)
            /// </summary>
            /// <param name="type">Variable type</param>
            public LDIVariable(Type type)
                : this(type, 1, null)
            {
                //get default value for the given type
                Value = (type.IsValueType) ? Activator.CreateInstance(type) : null; 
            }

            /// <summary>
            /// Builder 
            /// </summary>
            /// <param name="type">Variable type</param>
            /// <param name="value">Variable initial value</param>
            public LDIVariable(Type type, object value)
                : this(type, 1, value)
            {
            }
        }
        #endregion Classes
    }
}
