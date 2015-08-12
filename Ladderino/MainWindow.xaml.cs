using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Ladderino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RungStack;
            Canvas rung1 = CreateRung();
            var coil = new ComponentUI.Coil();
            var contact = new ComponentUI.Contact();
            var but1 = new Button();
            but1.Content = "Asda";
            RungStack.Children.Add(rung1);
            rung1.Children.Add(contact);
            rung1.Children.Add(coil);

            RungStack.Background = Brushes.Green;
            coil.LogicComponent.RightLide = new Core.Components.Node(coil.LogicComponent);
            coil.LogicComponent.RightLide.LogicLevel = true;
            coil.LogicComponent.LeftLide.LogicLevel = true;
            coil.LogicComponent.Execute();
            SetCanvasPos(coil, 200, 0);
            Button but = new Button();
            but.Content = "TExto";
            SetCanvasPos(but, 300, 10);
        }


        private Canvas CreateRung()
        {
            Canvas rung = new Canvas();
            rung.MinHeight = this.FontSize * 2;
            rung.Background = Brushes.BlueViolet;
            Binding size = new Binding("ActualWidth");
            size.Source = RungStack;
            rung.SetBinding(Canvas.MinWidthProperty, size);
            //rung.Focusable = true;
            rung.IsEnabled = true;
            return rung;
        }

        private void SetCanvasPos(UIElement element, double x, double y)
        {
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((RungStack.Children[0] as Canvas).ActualWidth.ToString());
        }
    }
}
