using System.Windows;
using System.Windows.Controls;

namespace ComponentUI
{
    public abstract class ComponentUIBase : Control
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

        public string Comment
        {
            get { return LogicComponent.Comment; }
            set { LogicComponent.Comment = value; }
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
