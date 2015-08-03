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
    /// Interaction logic for VerticalWire.xaml
    /// </summary>
    public partial class VerticalWire : UserControl
    {
        public VerticalWire()
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
            DependencyProperty.Register("ActiveBrush", typeof(SolidColorBrush), typeof(VerticalWire), new PropertyMetadata(Brushes.Red));

    }
}
