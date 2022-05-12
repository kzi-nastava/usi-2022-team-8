using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
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
    /// Interaction logic for DelayExaminationOperationDialog.xaml
    /// </summary>
    public partial class DelayExaminationOperationDialog : Window
    {
        List<ScheduleEditRequest> DelayedAppointments;
        Examination Examination;
        Operation Operation;
        public DelayExaminationOperationDialog(List<ScheduleEditRequest> delayedAppointments, Examination examination, Operation operation)
        {
            InitializeComponent();
            DelayedAppointments=delayedAppointments;
            Examination = examination;
            Operation = operation;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            foreach (ScheduleEditRequest delayedAppointment in DelayedAppointments)
            {
                dataGrid.Items.Add(delayedAppointment);
            }
            dataGrid.Items.Refresh();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            NotificationRepository notificationRepository = NotificationRepository.GetInstance();
            ScheduleEditRequest selectedAppointment = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                if (Operation == null)
                {
                    ExaminationRepository.GetInstance().SwapExaminationValue(selectedAppointment.NewExamination);
                    notificationRepository.Add(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
                    if (selectedAppointment.CurrentExamination == null)
                    {
                        Examination.Appointment = selectedAppointment.CurrentOperation.Appointment;
                        Examination.Room = selectedAppointment.CurrentOperation.Room;
                        Examination.Doctor = selectedAppointment.CurrentOperation.Doctor;
                    }
                    else
                    {
                        Examination.Appointment = selectedAppointment.CurrentExamination.Appointment;
                        Examination.Room = selectedAppointment.CurrentExamination.Room;
                        Examination.Doctor = selectedAppointment.CurrentExamination.Doctor;
                    }
                    ExaminationDTO examinationDTO = new ExaminationDTO(Examination.Appointment, Examination.Room, Examination.Doctor, Examination.MedicalRecord);
                    ExaminationRepository.GetInstance().Add(examinationDTO);
                    notificationRepository.Add(new DateTime(1, 1, 1), Examination.Appointment, Examination.Doctor, Examination.MedicalRecord.Patient);
                }
                else if (Examination == null)
                {
                    OperationRepository.GetInstance().SwapOperationValue(selectedAppointment.NewOperation);
                    notificationRepository.Add(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
                    if (selectedAppointment.CurrentExamination == null)
                    {
                        Operation.Appointment = selectedAppointment.CurrentOperation.Appointment;
                        Operation.Room = selectedAppointment.CurrentOperation.Room;
                        Operation.Doctor = selectedAppointment.CurrentOperation.Doctor;
                    }
                    else
                    {
                        Operation.Appointment = selectedAppointment.CurrentExamination.Appointment;
                        Operation.Room = selectedAppointment.CurrentExamination.Room;
                        Operation.Doctor = selectedAppointment.CurrentExamination.Doctor;
                    }
                    OperationDTO operationDTO = new OperationDTO(Operation.Appointment, Operation.Duration, Operation.Room, Operation.Doctor, Operation.MedicalRecord);
                    OperationRepository.GetInstance().Add(operationDTO);
                    notificationRepository.Add(new DateTime(1, 1, 1), Operation.Appointment, Operation.Doctor, Operation.MedicalRecord.Patient);
                }
            }
        }
    }
}
