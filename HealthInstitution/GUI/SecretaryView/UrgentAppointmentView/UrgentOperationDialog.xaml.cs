using HealthInstitution.Core.Operations.Model;
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
using System.Windows.Shapes;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for UrgentOperationDialog.xaml
    /// </summary>
    public partial class UrgentOperationDialog : Window
    {
        Operation _operation;
        public UrgentOperationDialog()
        {
            InitializeComponent();
            LoadRows();
        }
        public void SetScheduledOperation(Operation operation)
        { _operation = operation;}
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Add(_operation);
            dataGrid.Items.Refresh();
        }
    }
}
