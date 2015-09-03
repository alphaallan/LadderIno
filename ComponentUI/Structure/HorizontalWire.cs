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
    public class HorizontalWire : Control
    {
        static HorizontalWire()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWire), new FrameworkPropertyMetadata(typeof(HorizontalWire)));
        }

        public Core.Components.Node LogicNode
        {
            get { return (Core.Components.Node)GetValue(LogicNodeProperty); }
            set { SetValue(LogicNodeProperty, value); }
        }
        public static readonly DependencyProperty LogicNodeProperty =
            DependencyProperty.Register("LogicNode", typeof(Core.Components.Node), typeof(HorizontalWire), new PropertyMetadata(null));
    }
}
