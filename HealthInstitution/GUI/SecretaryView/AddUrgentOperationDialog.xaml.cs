using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
    /// Interaction logic for AddUrgentOperationDialog.xaml
    /// </summary>
    public partial class AddUrgentOperationDialog : Window
    {
        public AddUrgentOperationDialog()
        {
            InitializeComponent();
        }
        private void SpecialtyTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            specialtyTypeComboBox.Items.Clear();
            foreach (SpecialtyType specialtyType in Enum.GetValues(typeof(SpecialtyType)))
                specialtyTypeComboBox.Items.Add(specialtyType); 
            specialtyTypeComboBox.SelectedIndex = 0;
            specialtyTypeComboBox.Items.Refresh();
        }
        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            patientComboBox.Items.Clear();
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedIndex = 0;
            patientComboBox.Items.Refresh();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SpecialtyType specialtyType = (SpecialtyType)specialtyTypeComboBox.SelectedItem;
                Patient patient = (Patient)patientComboBox.SelectedItem;
                MedicalRecord medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
                List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying = OperationRepository.GetInstance().ReserveUrgentOperation(patient.Username, specialtyType,15);
                Operation urgentOperation;

                if (examinationsAndOperationsForDelaying.Count()==1)
                {
                    urgentOperation = OperationRepository.GetInstance().GetById(examinationsAndOperationsForDelaying[0].Item1);
                    OperationDoctorRepository.GetInstance().Save();
                    System.Windows.MessageBox.Show("Urgent operation has ordered successfully.");
                    UrgentOperationDialog urgentOperationDialog = new UrgentOperationDialog(urgentOperation);
                    urgentOperationDialog.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("There are no free appointments in next two hours. Please select examination or operation to be delayed.");
                    List<ScheduleEditRequest> delayedAppointments = new List<ScheduleEditRequest>();
                    foreach(Tuple<int,int,DateTime> tuple in examinationsAndOperationsForDelaying)
                    {
                        if (tuple.Item2 == 1)
                        {
                            Examination currentExamination = ExaminationRepository.GetInstance().GetById(tuple.Item1);
                            Examination newExamination = new Examination(currentExamination.Id, ExaminationStatus.Scheduled, tuple.Item3, currentExamination.Room, currentExamination.Doctor, currentExamination.MedicalRecord, "");
                            delayedAppointments.Add(new ScheduleEditRequest(0, currentExamination, newExamination, Core.RestRequests.Model.RestRequestState.OnHold));
                            
                        }
                        if (tuple.Item2 == 0)
                        {
                            Operation currentOperation = OperationRepository.GetInstance().GetById(tuple.Item1);
                            Operation newOperation = new Operation(currentOperation.Id, ExaminationStatus.Scheduled, tuple.Item3,currentOperation.Duration, currentOperation.Room, currentOperation.Doctor, currentOperation.MedicalRecord);
                            delayedAppointments.Add(new ScheduleEditRequest(0, currentOperation, newOperation, Core.RestRequests.Model.RestRequestState.OnHold));
                        }
                        
                    }
                    urgentOperation = new Operation(examinationsAndOperationsForDelaying[0].Item1, ExaminationStatus.Scheduled, new DateTime(1, 1, 1), 15, null, null, medicalRecord);
                    DelayExaminationOperationDialog delayExaminationOperationDialog = new DelayExaminationOperationDialog(delayedAppointments,null,urgentOperation);
                    delayExaminationOperationDialog.ShowDialog();
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
