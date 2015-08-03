using Core.Components;
using Core.Data;
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
            Contact cont = new Contact();
            cont.Name = "1";
            cont.IsClosed = false;
            cont.LeftLide.LogicLevel = true;
            comp.LogicComponent = cont;
            comp.UIString = cont.FullName + "\r\n]  [";
            cont.Execute();
        }
    }
}
