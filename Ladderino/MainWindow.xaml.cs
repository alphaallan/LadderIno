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

            var cont1 = new ComponentUI.Contact();
            (cont1.LogicComponent as Core.Components.Contact).IsInverted = true;
            (cont1.LogicComponent as Core.Components.Contact).Name = "1";

            var cont2 = new ComponentUI.Contact();
            (cont2.LogicComponent as Core.Components.NameableComponent).Name = "2";

            var cont3 = new ComponentUI.Contact();
            (cont3.LogicComponent as Core.Components.NameableComponent).Name = "3";

            var cont4 = new ComponentUI.Contact();
            (cont4.LogicComponent as Core.Components.NameableComponent).Name = "4";

            var cont5 = new ComponentUI.Contact();
            (cont5.LogicComponent as Core.Components.NameableComponent).Name = "5";

            var coil1 = new ComponentUI.Coil();
            (coil1.LogicComponent as Core.Components.NameableComponent).Name = "1";
            var coil2 = new ComponentUI.Coil();
            (coil2.LogicComponent as Core.Components.NameableComponent).Name = "2";

            rung.Add(coil1);
            rung.Add(cont1);
            rung.InsertAbove(coil2, coil1)
                .InsertUnder(cont2, cont1)
                .InsertAfter(cont4, cont1)
                .InsertBefore(cont5, cont4)
                .InsertAbove(cont3, cont4);

            rung.PlaceWires();

            rung.DataTable = new Core.Data.LadderDataTable();

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
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
            //rung.Background = Brushes.BlueViolet;
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
