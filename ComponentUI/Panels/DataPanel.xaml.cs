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
using Core.Components;
using Core.Data;

namespace ComponentUI.Panels
{
    /// <summary>
    /// Interaction logic for DataPanel.xaml
    /// </summary>
    public partial class DataPanel : UserControl
    {
        public DataPanel()
        {
            InitializeComponent();
        }

        public Diagram LadderProgram 
        {
            get
            {
                return _LadderProgram;
            }
            set
            {
                if (_LadderProgram != null && _LadderProgram.DataTable != null)
                {
                    LadderProgram.DataTable.VariableAdded -= DataTable_VariableAdded;
                    LadderProgram.DataTable.VariableClassChanged -= DataTable_VariableClassChanged;
                    LadderProgram.DataTable.VariableRemoved -= DataTable_VariableRemoved;
                    LadderProgram.DataTable.VariableRenamed -= DataTable_VariableRenamed;
                }

                _LadderProgram = value;

                Refresh();
                if (_LadderProgram != null && LadderProgram.DataTable != null)
                {
                    LadderProgram.DataTable.VariableAdded += DataTable_VariableAdded;
                    LadderProgram.DataTable.VariableClassChanged += DataTable_VariableClassChanged;
                    LadderProgram.DataTable.VariableRemoved += DataTable_VariableRemoved;
                    LadderProgram.DataTable.VariableRenamed += DataTable_VariableRenamed;
                }
            }
        }
        Diagram _LadderProgram;

        void DataTable_VariableRenamed(object sender, VarRenamedArgs e)
        {
            Refresh();
        }

        void DataTable_VariableRemoved(object sender, VarRemovedArgs e)
        {
            //Refresh();
        }

        void DataTable_VariableClassChanged(object sender, VarClassChangedArgs e)
        {
            Refresh();
        }

        void DataTable_VariableAdded(object sender, VarAddedArgs e)
        {
            Refresh();
        }

        public void Refresh() 
        {
            RefreshPins();
            RefreshVariables();
            RefreshFunctions();
        }


        private void RefreshPins()
        {
            if (LadderProgram != null && LadderProgram.Pins != null)
            {
                PinGrid.ItemsSource = LadderProgram.Pins.OrderBy(x => x.Variable);
            }
            else
            {
                PinGrid.ItemsSource = null;
            }
        }

        private void RefreshVariables()
        {
            if (LadderProgram != null && LadderProgram.DataTable != null)
            {
                VariableGrid.ItemsSource = LadderProgram.DataTable.ListAllData()
                                                                  .Where(x => x.Item3 == LDVarClass.Data)
                                                                  .Select(x => new KeyValuePair<string, object>(x.Item1, x.Item4))
                                                                  .OrderBy(x => x.Key);
            }
            else
            {
                VariableGrid.ItemsSource = null;
            }
        }

        private void RefreshFunctions()
        {
            if (LadderProgram != null && LadderProgram.DataTable != null)
            {
                FunctionGrid.ItemsSource = LadderProgram.DataTable.ListAllData()
                                                                  .Where(x => x.Item3 == LDVarClass.InFunction || x.Item3 == LDVarClass.OutFunction)
                                                                  .Select(x => new Tuple<string, string>(x.Item1, x.Item3.ToString()))
                                                                  .OrderBy(x => x.Item1);
            }
            else
            {
                FunctionGrid.ItemsSource = null;
            }
        }
    }
}
