using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComponentUI
{
    public class ComponentUIBase : Button
    {
        static ComponentUIBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComponentUIBase), new FrameworkPropertyMetadata(typeof(ComponentUIBase)));
        }

        public ComponentUIBase(Core.Components.ComponentBase component)
        {
            LogicComponent = component;
            LogicComponent.PropertyChanged += LogicComponent_PropertyChanged;
        }

        protected virtual void LogicComponent_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
        
        public Core.Components.ComponentBase LogicComponent
        {
            get { return (Core.Components.ComponentBase)GetValue(LogicComponentProperty); }
            set { SetValue(LogicComponentProperty, value); }
        }
        public static readonly DependencyProperty LogicComponentProperty =
            DependencyProperty.Register("LogicComponent", typeof(Core.Components.ComponentBase), typeof(ComponentUIBase), new PropertyMetadata(null));

        public string Line1
        {
            get { return (string)GetValue(Line1Property); }
            set { SetValue(Line1Property, value); }
        }
        public static readonly DependencyProperty Line1Property =
            DependencyProperty.Register("Line1", typeof(string), typeof(ComponentUIBase), new PropertyMetadata(string.Empty));

        public string Line2
        {
            get { return (string)GetValue(Line2Property); }
            set { SetValue(Line2Property, value); }
        }
        public static readonly DependencyProperty Line2Property =
            DependencyProperty.Register("Line2", typeof(string), typeof(ComponentUIBase), new PropertyMetadata(string.Empty));
    }
}
