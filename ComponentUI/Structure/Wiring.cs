using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

    public class VerticalWire : Node
    {
        static VerticalWire()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalWire), new FrameworkPropertyMetadata(typeof(VerticalWire)));
        }

        public VerticalWire(Core.Components.Node logicNode, int column, int startRow, int endRow)
        {
            Grid.SetColumn(this, column);
            Grid.SetRow(this, startRow);
            Grid.SetRowSpan(this, endRow - startRow + 1);
            this.LogicNode = logicNode;
        }

        public VerticalWire()
        {

        }
    }

    public class HorizontalWire : Node
    {
        static HorizontalWire()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalWire), new FrameworkPropertyMetadata(typeof(HorizontalWire)));
        }

        public HorizontalWire(Core.Components.Node logicNode, int row, int startColumn, int endColumn)
        {
            Grid.SetRow(this, row);
            Grid.SetColumn(this, startColumn);
            Grid.SetColumnSpan(this, endColumn - startColumn);
            this.LogicNode = logicNode;
        }

        public HorizontalWire()
        {

        }
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
