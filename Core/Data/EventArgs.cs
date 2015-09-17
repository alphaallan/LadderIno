using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{
    public class VarRenamedArgs : EventArgs
    {
        public string OldName { get; private set; }
        public string NewName { get; private set; }

        public VarRenamedArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }

    public class VarClassChangedArgs : EventArgs
    {
        public string Name { get; private set; }
        public LDVarClass OldClass { get; private set; }
        public LDVarClass NewClass { get; private set; }

        public VarClassChangedArgs(string name, LDVarClass oldClass, LDVarClass newClass)
        {
            Name = name;
            OldClass = oldClass;
            NewClass = newClass;
        }
    }

    public class VarAddedArgs : EventArgs
    {
        public string Name { get; private set; }
        public LDVarClass VarClass { get; private set; }

        public VarAddedArgs(string name, LDVarClass varClass)
        {
            Name = name;
            VarClass = varClass;
        }
    }

    public class VarRemovedArgs : EventArgs
    {
        public string Name { get; private set; }

        public VarRemovedArgs(string name)
        {
            Name = name;
        }
    }
}
