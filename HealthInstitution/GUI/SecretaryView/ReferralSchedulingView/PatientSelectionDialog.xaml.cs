using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
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
        IMedicalRecordService _medicalRecordService;
        public PatientSelectionDialog(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<MedicalRecord> medicalRecords = _medicalRecordService.GetAll();
            foreach (MedicalRecord medicalRecord in medicalRecords)
            {
                if(medicalRecord.Patient.Blocked==BlockState.NotBlocked)
                    dataGrid.Items.Add(medicalRecord);
            }
            dataGrid.Items.Refresh();
        }

        private void SelectPatient_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecord selectedMedicalRecord = (MedicalRecord)dataGrid.SelectedItem;
            if (selectedMedicalRecord != null)
            {
                PatientReferralsDialog patientReferralsDialog = new PatientReferralsDialog(selectedMedicalRecord);
                patientReferralsDialog.ShowDialog();
                dataGrid.SelectedItem = null;

            }
        }
    }
}
