using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComponentUI
{
    public class Coil : ComponentUIBase
    {
        public Coil()
            : base(new Core.Components.Coil())
        {
            var component = LogicComponent as Core.Components.Coil;
            Binding name = new Binding("FullName");
            name.Source = component;
            SetBinding(Coil.Line1Property, name);

            Line2 = "(" + (char)component.Mode + ")";

        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Core.Components.Coil && sender == this.LogicComponent)
            {
                var component = sender as Core.Components.Coil;

                if (e.PropertyName == "Mode") Line2 = "(" + (char)component.Mode + ")";
            }
        }
    }
}
