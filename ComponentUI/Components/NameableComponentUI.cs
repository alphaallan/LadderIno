using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ComponentUI
{
    public abstract class NameableComponentUI : ComponentUIBase
    {
        public NameableComponentUI(Core.Components.NameableComponent component)
            : base(component)
        {
            Binding name = new Binding("FullName");
            name.Source = component;
            SetBinding(NameableComponentUI.Line1Property, name);
        }

        #region Properties
        public string LadderName
        {
            get { return (LogicComponent as Core.Components.NameableComponent).Name; }
            set { (LogicComponent as Core.Components.NameableComponent).Name = value; }
        }

        public Core.Components.NameableComponent.ComponentPrefix LadderNamePerfix
        {
            get { return (LogicComponent as Core.Components.NameableComponent).NamePerfix; }
        }

        public string LadderFullName
        {
            get { return (LogicComponent as Core.Components.NameableComponent).FullName; }
        }
        #endregion Properties
    }
}
