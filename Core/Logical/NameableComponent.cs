﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Components.Logical
{
    /// <summary>
    /// Base class for LadderIno components that have name 
    /// </summary>
    public abstract class NameableComponent : ComponentBase
    {
        #region Properties
        /// <summary>
        /// Component Name
        /// Used by compiler as identifier
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                NameChangedHandler(FullName, ((char)this._NamePrefix) + value);
                _Name = value;

                RaisePropertyChanged("Name");
                RaisePropertyChanged("FullName");
            }
        }

        /// <summary>
        /// Name prefix 
        /// </summary>
        public ComponentPrefix NamePerfix
        {
            get { return _NamePrefix; }
            protected set 
            {
                NameChangedHandler(FullName, ((char)value) + _Name);
                _NamePrefix = value;
                RaisePropertyChanged("NamePerfix");
                RaisePropertyChanged("FullName");
            }
        }

        /// <summary>
        /// Component name with prefix
        /// </summary>
        public string FullName
        {
            get { return ((char)this._NamePrefix) + this._Name; }
        }
        #endregion Properties

        #region Functions
        /// <summary>
        /// Handle Core's data table manipulation on name change.
        /// </summary>
        /// <param name="oldName">Full Name before Change</param>
        /// <param name="newName">New Full Name</param>
        protected virtual void NameChangedHandler(string oldName, string newName)
        {
            try
            {
                Data.LDIVariableTable.Rename(oldName, newName);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName == "oldName") Data.LDIVariableTable.Add(newName, DataType);
                else throw ex;
            }
        }

        public override string ToString()
        {
            return ((char)this._NamePrefix) + this._Name + " (" + this.GetType().ToString() + ")" ;
        }
        #endregion Functions

        #region Constructors
        public NameableComponent(Type dType) 
            : this(dType,"NEW")
        {
            
        }

        public NameableComponent(Type dType, string name)
        {
            DataType = dType;
            Name = name;            
        }

        public NameableComponent(Type dType, Node Left, Node Right) 
            : this (dType, "NEW", Left, Right)
        {

        }

        public NameableComponent(Type dType, string name, Node Left, Node Right)
            : base(Left, Right)
        {
            DataType = dType;
            Name = name;
        }
        #endregion Constructors

        #region Destructor
        ~NameableComponent()
        {
            try
            {
                Data.LDIVariableTable.Remove(FullName);
            }
            catch (ArgumentException) { }
        }
        #endregion Destructor

        #region Internal Data
        private ComponentPrefix _NamePrefix = ComponentPrefix.None;

        private readonly Type DataType;
        private string _Name;
        #endregion Internal Data

        #region Enums
        public enum ComponentPrefix
        {
            AnalogInput = 'A',
            Conter = 'C',
            Input = 'X',
            Output = 'Y',
            Relay = 'R',
            Timer = 'T',
            None = ' '
        }
        #endregion Enums
        
    }
}
