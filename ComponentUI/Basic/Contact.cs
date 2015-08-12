using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComponentUI
{
    public class Contact : ComponentUIBase
    {
        public Contact()
            : base(new Core.Components.Contact())
        {
            var component = LogicComponent as Core.Components.Contact;
            Binding name = new Binding("FullName");
            name.Source = component;
            SetBinding(Contact.Line1Property, name);

            Line2 = (component.IsInverted) ? "]/[" : "] [";

           
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Core.Components.Contact && sender == this.LogicComponent)
            {
                var component = sender as Core.Components.Contact;

                if (e.PropertyName == "IsInverted") Line2 = (component.IsInverted) ? "]/[" : "] [";
            }
        }
    }
}
