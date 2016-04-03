using System.Windows.Data;

namespace ComponentUI
{
    public class Coil : NameableComponentUI
    {
        public Coil()
            : base(new Core.Components.Coil())
        {
            var component = LogicComponent as Core.Components.Coil;
            Line2 = "(" + (char)component.Mode + ")";
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is Core.Components.Coil && sender == LogicComponent)
            {
                var component = sender as Core.Components.Coil;

                if (e.PropertyName == "Mode") Line2 = "(" + (char)component.Mode + ")";
            }
        }

        #region Properties
        public Core.Components.Coil.CoilType Type
        {
            get { return (LogicComponent as Core.Components.Coil).Type; }
            set { (LogicComponent as Core.Components.Coil).Type = value; }
        }

        public Core.Components.Coil.CoilMode Mode
        {
            get { return (LogicComponent as Core.Components.Coil).Mode; }
            set { (LogicComponent as Core.Components.Coil).Mode = value; }
        }
        #endregion Properties
    }
}
