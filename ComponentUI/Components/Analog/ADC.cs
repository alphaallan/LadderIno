using System.Windows.Data;

namespace ComponentUI.Components
{
    public class ADC : NameableComponentUI
    {
        public ADC()
            : base(new Core.Components.ADC())
        {
            var component = LogicComponent as Core.Components.Contact;
            Binding name = new Binding("FullName");
            name.Source = component;
            SetBinding(ADC.Line1Property, name);

            Line2 = "{READ ADC}";

            MouseDoubleClick += Contact_MouseDoubleClick;
        }

        void Contact_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var component = LogicComponent as Core.Components.ADC;
        }

        protected override void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        #region Properties
        /// <summary>
        /// Destination Variable
        /// </summary>
        public string Destination
        {
            get { return (LogicComponent as Core.Components.ADC).Destination; }
            set { (LogicComponent as Core.Components.ADC).Destination = value; }
        }

        /// <summary>
        /// Input value in ADC
        /// </summary>
        public short InputValue
        {
            get { return (LogicComponent as Core.Components.ADC).InputValue; }
            set { (LogicComponent as Core.Components.ADC).InputValue = value; }
        }
        #endregion Properties
    }
}
