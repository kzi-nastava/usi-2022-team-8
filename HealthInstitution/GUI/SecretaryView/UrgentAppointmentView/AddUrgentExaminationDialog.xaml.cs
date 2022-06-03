using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
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
    /// Interaction logic for AddUrgentExaminationDialog.xaml
    /// </summary>
    public partial class AddUrgentExaminationDialog : Window
    {
        MedicalRecord _selectedMedicalRecord;
        SpecialtyType _selectedSpecialtyType;
        public AddUrgentExaminationDialog()
        {
            InitializeComponent();
        }
        private void SpecialtyTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            specialtyTypeComboBox.Items.Clear();
            foreach (SpecialtyType specialtyType in Enum.GetValues(typeof(SpecialtyType)))
                specialtyTypeComboBox.Items.Add(specialtyType);
            specialtyTypeComboBox.SelectedIndex = 0;
        }
        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            patientComboBox.Items.Clear();
            List<Patient> patients = PatientService.GetAll();
            foreach (Patient patient in patients)
                if (patient.Blocked==Core.SystemUsers.Users.Model.BlockState.NotBlocked)
                    patientComboBox.Items.Add(patient);
            patientComboBox.SelectedIndex = 0;
        }
        private void ShowReservedExaminationWithoutDelaying(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying)
        {
            Examination urgentExamination = ExaminationRepository.GetInstance().GetById(examinationsAndOperationsForDelaying[0].Item1);
            System.Windows.MessageBox.Show("Urgent examination has ordered successfully.");
            UrgentExaminationDialog urgentExaminationDialog = new UrgentExaminationDialog(urgentExamination);
            urgentExaminationDialog.ShowDialog();
        }
        private void ShowDelayingAppointmentSelectionDialog(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying, MedicalRecord medicalRecord)
        {
            System.Windows.MessageBox.Show("There are no free appointments in next two hours. Please select examination or operation to be delayed.");
            List<ScheduleEditRequest> delayedAppointments = AppointmentDelayingService.PrepareDataForDelaying(examinationsAndOperationsForDelaying);
            Operation urgentOperation = new Operation(examinationsAndOperationsForDelaying[0].Item1, new DateTime(1, 1, 1), 15, null, null, medicalRecord);
            DelayExaminationOperationDialog delayExaminationOperationDialog = new DelayExaminationOperationDialog(delayedAppointments, null, urgentOperation);
            delayExaminationOperationDialog.ShowDialog();
        }
        private void PickDataFromForm()
        {
            _selectedSpecialtyType = (SpecialtyType)specialtyTypeComboBox.SelectedItem;
            Patient patient = (Patient)patientComboBox.SelectedItem;
            _selectedMedicalRecord = MedicalRecordService.GetByPatientUsername(patient);
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PickDataFromForm();
                List<Tuple<int,int,DateTime>> examinationsAndOperationsForDelaying = UrgentService.ReserveUrgentExamination(_selectedMedicalRecord.Patient.Username, _selectedSpecialtyType);
                if (examinationsAndOperationsForDelaying.Count()==1)
                    ShowReservedExaminationWithoutDelaying(examinationsAndOperationsForDelaying);
                else
                {
                    ShowDelayingAppointmentSelectionDialog(examinationsAndOperationsForDelaying, _selectedMedicalRecord);
                }
                Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
