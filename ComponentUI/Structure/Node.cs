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
    public class Node : Control
    {
        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        public Core.Components.Node LogicNode
        {
            get { return (Core.Components.Node)GetValue(LogicNodeProperty); }
            set { SetValue(LogicNodeProperty, value); }
        }
        public static readonly DependencyProperty LogicNodeProperty =
            DependencyProperty.Register("LogicNode", typeof(Core.Components.Node), typeof(Node), new PropertyMetadata(null));
    }

    public class HalfConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value) / 2; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
