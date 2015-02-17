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
using Components.Logical;

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

            rung = new Rung();

            contact = new Contact();
            coil = new Coil();

            rung.Add(coil);
            rung.Add(contact);

            DataContext = this;

        }

        public Coil coil { get; set; }
        public Contact contact { get; set; }



        public Rung rung
        {
            get { return (Rung)GetValue(rungProperty); }
            set { SetValue(rungProperty, value); }
        }

        // Using a DependencyProperty as the backing store for rung.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty rungProperty =
            DependencyProperty.Register("rung", typeof(Rung), typeof(MainWindow), new PropertyMetadata(null));

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(contact.LeftLide.LogicLevel + " -> " +contact.Execute() + " -> " + contact.RightLide.LogicLevel);
            //Console.WriteLine(coil.LeftLide.LogicLevel + " -> " + coil.Execute());
            rung.Execute();
        }

        

        

        
    }
}
