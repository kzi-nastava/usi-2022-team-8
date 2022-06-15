using HealthInstitution.Core.Examinations.Model;
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
    /// Interaction logic for UrgentExamination.xaml
    /// </summary>
    public partial class UrgentExaminationDialog : Window
    {
        Examination _examination;
        public UrgentExaminationDialog()
        {
            InitializeComponent();
            
        }
        public void SetScheduledExamination(Examination examination)
        {
            _examination = examination;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Add(_examination);
            dataGrid.Items.Refresh();
        }
    }
}
