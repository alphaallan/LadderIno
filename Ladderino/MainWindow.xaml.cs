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

        ComponentUI.Rung rung;
        public MainWindow()
        {
            InitializeComponent();
            //RungStack;
            rung = CreateRung();
            RungStack.Children.Add(rung);

            var cont = new ComponentUI.Contact();
            (cont.LogicComponent as Core.Components.Contact).IsInverted = true;
            (cont.LogicComponent as Core.Components.Contact).Name = "2";


            rung.Add(new ComponentUI.Coil());
            rung.Add(new ComponentUI.Coil());
            rung.Add(new ComponentUI.Contact());
            rung.Add(cont);
            rung.DataTable = new Core.Data.LadderDataTable();

            RungStack.Background = Brushes.Green;
           

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// Função que notifica a atualização do relógio a cada segundo
        /// </summary>
        private void Timer_Click(object sender, EventArgs e)
        {
            rung.Execute();
        }

        private ComponentUI.Rung CreateRung()
        {
            //Canvas rung = new ComponentUI.Rung();
            //rung.MinHeight = this.FontSize * 2;
            //rung.Background = Brushes.BlueViolet;
            //Binding size = new Binding("ActualWidth");
            //size.Source = RungStack;
            //rung.SetBinding(Canvas.MinWidthProperty, size);
            //rung.IsEnabled = true;
            //return rung;

            ComponentUI.Rung rung = new ComponentUI.Rung();
            rung.MinHeight = this.FontSize * 2;
            rung.Background = Brushes.BlueViolet;
            Binding size = new Binding("ActualWidth");
            size.Source = RungStack;
            rung.SetBinding(Canvas.MinWidthProperty, size);
            rung.IsEnabled = true;

            
            
            
            return rung;
        }

        private void SetCanvasPos(UIElement element, double x, double y)
        {
            Canvas.SetLeft(element, x);
            Canvas.SetTop(element, y);
        }

    }
}
