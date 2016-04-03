using System.Windows.Data;

namespace ComponentUI
{
    public class Contact : NameableComponentUI
    {
        public Contact()
            : base(new Core.Components.Contact())
        {
            var component = LogicComponent as Core.Components.Contact;
            Line2 = (component.IsInverted) ? "]/[" : "] [";

            MouseDoubleClick += Contact_MouseDoubleClick;
        }

        void Contact_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var component = LogicComponent as Core.Components.Contact;
            if(component.Type == Core.Components.Contact.ContactType.InputPin) component.IsClosed = !component.IsClosed;
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Core.Components.Contact && sender == LogicComponent)
            {
                var component = sender as Core.Components.Contact;

                if (e.PropertyName == "IsInverted") Line2 = (component.IsInverted) ? "]/[" : "] [";
            }
        }

        #region Properties
        /// <summary>
        /// Define whenever the contact is closed (high logical level on input)
        /// </summary>
        public bool IsClosed
        {
            get { return (LogicComponent as Core.Components.Contact).IsClosed; }
            set { (LogicComponent as Core.Components.Contact).IsClosed = value; }
        }

        /// <summary>
        /// Define if the contact is normally open
        /// </summary>
        public bool IsInverted
        {
            get { return (LogicComponent as Core.Components.Contact).IsInverted; }
            set { (LogicComponent as Core.Components.Contact).IsInverted = value; }
        }

        /// <summary>
        /// Define what type of input the contact reads.
        /// Input pin, output pin or internal relay
        /// </summary>
        public Core.Components.Contact.ContactType Type
        {
            get { return (LogicComponent as Core.Components.Contact).Type; }
            set { (LogicComponent as Core.Components.Contact).Type = value; }
        }
        #endregion Properties
    }
}
