using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
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
    /// Interaction logic for PatientSelectionDialog.xaml
    /// </summary>
    public partial class PatientSelectionDialog : Window
    {
        public PatientSelectionDialog()
        {
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                if(patient.Blocked==Core.SystemUsers.Users.Model.BlockState.NotBlocked)
                    dataGrid.Items.Add(patient);
            }
            dataGrid.Items.Refresh();
        }

        private void SelectPatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null)
            {
                //TODO
                dataGrid.SelectedItem = null;

            }
        }
    }
}
