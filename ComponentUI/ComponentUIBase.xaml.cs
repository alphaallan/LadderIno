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
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ComponentUIBase : UserControl
    {
        public ComponentUIBase()
        {
            InitializeComponent();
            DataContext = this;
        }

        public SolidColorBrush ActiveBrush
        {
            get { return (SolidColorBrush)GetValue(ActiveBrushProperty); }
            set { SetValue(ActiveBrushProperty, value); }
        }
        public static readonly DependencyProperty ActiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(SolidColorBrush), typeof(ComponentUIBase), new PropertyMetadata(Brushes.Red));


        public Core.Components.ComponentBase LogicComponent
        {
            get { return (Core.Components.ComponentBase)GetValue(LogicComponentProperty); }
            set { SetValue(LogicComponentProperty, value); }
        }
        public static readonly DependencyProperty LogicComponentProperty =
            DependencyProperty.Register("LogicComponent", typeof(Core.Components.ComponentBase), typeof(ComponentUIBase), new PropertyMetadata(null));


        public string UIString
        {
            get { return (string)GetValue(UIStringProperty); }
            set { SetValue(UIStringProperty, value); }
        }
        public static readonly DependencyProperty UIStringProperty =
            DependencyProperty.Register("UIString", typeof(string), typeof(ComponentUIBase), new PropertyMetadata(string.Empty));

        
    }
}
