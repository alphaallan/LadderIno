using ComponentUI;
using System;
using System.Windows;

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
            
            #region Declare Components
            RungUI Rung1 = new RungUI();
            RungUI Rung2 = new RungUI();

            var Y1 = new Coil();
            Y1.LadderName = "1";

            var Y2 = new Coil();
            Y2.LadderName = "2";

            var X1 = new Contact();
            X1.LadderName = "1";

            var X1I = new Contact();
            X1I.LadderName = "1";
            X1I.IsInverted = true;

            var X2 = new Contact();
            X2.LadderName = "2";

            var X2I = new Contact();
            X2I.LadderName = "2";
            X2I.IsInverted = true;

            var Y1C = new Contact();
            Y1C.LadderName = "1";
            Y1C.Type = Core.Components.Contact.ContactType.OutputPin;

            var Y1CI = new Contact();
            Y1CI.LadderName = "1";
            Y1CI.Type = Core.Components.Contact.ContactType.OutputPin;
            Y1CI.IsInverted = true;

            var Y2C = new Contact();
            Y2C.LadderName = "2";
            Y2C.Type = Core.Components.Contact.ContactType.OutputPin;

            var Y2CI = new Contact();
            Y2CI.LadderName = "2";
            Y2CI.Type = Core.Components.Contact.ContactType.OutputPin;
            Y2CI.IsInverted = true;
            #endregion Declare Components

            #region Build Circuit
            Rung1.Add(Y1);
            Rung1.InsertBefore(X2I, Y1);
            Rung1.Add(X1);
            Rung1.InsertUnder(Y1C, X1);
            Rung1.InsertAfter(Y2CI, X1);
            Rung1.InsertBefore(new OSR(), Y2CI);
            Rung1.PlaceWires();
            RungStack.Add(Rung1);

            Rung2.Add(Y2);
            Rung2.InsertBefore(X1I, Y2);
            Rung2.Add(X2);
            Rung2.InsertUnder(Y2C, X2);
            Rung2.InsertAfter(Y1CI, X2);
            Rung2.InsertBefore(new OSR(), Y1CI);
            Rung2.PlaceWires();

            RungStack.Add(Rung2);
            #endregion Build Circuit

            RungStack.MasterRelay = true;
            dat.LadderProgram = RungStack.LogicalDiagram;

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
            RungStack.Execute();
        }
    }
}
