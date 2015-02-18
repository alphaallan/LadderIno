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
        public static int Count
        {
            get { return Table.Count; }
        }
        #endregion Properties

        #region Functions
        public static void Add(string name, Type type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");
            if (type == null) throw new ArgumentException("Variable type must be provided", "type");

            int c = 0;
            for (; c < Table.Count && Table[c].Name != name; c++);

            if (c < Table.Count)
            {
                if (type != Table[c].Type) throw new ArgumentException("Name already used", "name", new ArgumentException("Type Mismatch", "type"));
                Table[c].NumRefs++;
            }
            else
            {
                Table.Add(new LDIVariable(name, type));
            }
        }

        public static void Add(string name, Type type, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");
            if (type == null) throw new ArgumentException("Variable type must be provided", "type");
            if (value == null) throw new ArgumentException("Variable value must be provided", "value");
            if (value.GetType() != type) throw new ArgumentException("Type Mismatch", "type");

            int c = 0;
            for (; c < Table.Count && Table[c].Name != name; c++) ;

            if (c < Table.Count)
            {
                if (type != Table[c].Type) throw new ArgumentException("Name already used", "name", new ArgumentException("Type Mismatch", "type"));
                Table[c].NumRefs++;
                Table[c].Value = value;
            }
            else
            {
                Table.Add(new LDIVariable(name, type, value));
            }
        }

        public static void Remove(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            int c = 0;
            for (; c < Table.Count && Table[c].Name != name; c++) ;

            if (c < Table.Count)
            {
                Table[c].NumRefs--;
                if (Table[c].NumRefs == 0) Table.RemoveAt(c);
            }
            else throw new ArgumentException("Variable not found", "name");
        }

        public static void Rename(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName)) throw new ArgumentException("Variable old name must be provided", "oldName");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("Variable new name must be provided", "newName");

            int c = 0, i = 0 ;
            for (; c < Table.Count && Table[c].Name != oldName; c++) ;
            for (; i < Table.Count && Table[i].Name != newName; i++) ;

            if (c < Table.Count)
            {
                if(Table[c].NumRefs == 1)
                {
                    if (i < Table.Count) //new name exists in list
                    {
                        if (Table[c].Type != Table[i].Type) throw new ArgumentException("Name already used", "newName", new ArgumentException("Type Mismatch", "type"));
                        else Table.RemoveAt(c);
                    }
                    else Table[c].Name = newName; //new name does not exist in list
                }
                else
                {
                    Table[c].NumRefs--;
                    Table.Add(new LDIVariable(newName, Table[c].Type));
                }
            }
            else throw new ArgumentException("Variable name not found", "oldName");
        }

        public static int GetIndexOf(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            for (int c=0; c < Table.Count; c++)
                if (Table[c].Name == name) return c;

            throw new ArgumentException("Variable not found", "name");
        }

        public static bool Contains(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            for (int c = 0; c < Table.Count; c++)
                if (Table[c].Name == name) return true;

            return false;
        }

        public static object GetValue(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Variable name must be provided", "name");

            for (int c = 0; c < Table.Count; c++)
                if (Table[c].Name == name) return Table[c].Value;

            throw new ArgumentException("Variable not found", "name");
        }

        public static object GetValueAt(int index)
        {
            if (index < 0 || index >= Table.Count) throw new IndexOutOfRangeException("Index is not inside table limits");

            return Table[index].Value;
        }

        public static void SetValue(string name, object value)
        {
            for (int c = 0; c < Table.Count; c++)
            {
                if (Table[c].Name == name)
                {
                    if (value.GetType() != Table[c].Type) throw new ArgumentException("Type Mismatch", "value");
                    Table[c].Value = value;
                    return;
                }
            }

            throw new ArgumentException("Variable not found", "name");
        }

        public static void SetValueAt(int index, object value)
        {
            if (index < 0 || index >= Table.Count) throw new IndexOutOfRangeException("Index is not inside table limits");
            if (value.GetType() != Table[index].Type) throw new ArgumentException("Type Mismatch", "value");

            Table[index].Value = value;
        }
        #endregion Functions

        #region Constructors
        static LDIVariableTable()
        {
            Table = new List<LDIVariable>();
        }
        #endregion Constructors

        #region Internal Data
        static List<LDIVariable> Table { get; set; }
        #endregion Internal Data

        #region Classes
        /// <summary>
        /// Class that represents one memory entry
        /// </summary>
        private class LDIVariable
        {
            public string Name { get; set;}
            public object Value { get; set; }
            public int NumRefs { get; set; }
            public Type Type { get; set; }

            public LDIVariable(string name, Type type, int numRefs, object value)
            {
                Name = name;
                Value = value;
                NumRefs = numRefs;
                Type = type;
            }

            public LDIVariable(string name, Type type)
                : this(name, type, 1, null)
            {
                //get default value for the given type
                Value = (type.IsValueType) ? Activator.CreateInstance(type) : null; 
            }

            public LDIVariable(string name, Type type, object value)
                : this(name, type, 1, value)
            {
            }
        }
        #endregion Classes
    }
}
