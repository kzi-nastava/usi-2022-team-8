using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;
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
        List<ScheduleEditRequest> _delayedAppointments;
        Examination? _examination;
        Operation? _operation;
        public DelayExaminationOperationDialog(List<ScheduleEditRequest> delayedAppointments, Examination? examination, Operation? operation)
        {
            InitializeComponent();
            _delayedAppointments=delayedAppointments;
            _examination = examination;
            _operation = operation;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            foreach (ScheduleEditRequest delayedAppointment in _delayedAppointments)
            {
                dataGrid.Items.Add(delayedAppointment);
            }
            dataGrid.Items.Refresh();
        }
        private void DelayExamination(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationRepository notificationRepository = AppointmentNotificationRepository.GetInstance();
            ExaminationRepository.GetInstance().SwapExaminationValue(selectedAppointment.NewExamination);
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
            notificationRepository.Add(appointmentNotificationDto);
            if (selectedAppointment.CurrentExamination == null)
            {
                _examination.Appointment = selectedAppointment.CurrentOperation.Appointment;
                _examination.Room = selectedAppointment.CurrentOperation.Room;
                _examination.Doctor = selectedAppointment.CurrentOperation.Doctor;
            }
            else
            {
                _examination.Appointment = selectedAppointment.CurrentExamination.Appointment;
                _examination.Room = selectedAppointment.CurrentExamination.Room;
                _examination.Doctor = selectedAppointment.CurrentExamination.Doctor;
            }
            ExaminationDTO examinationDTO = new ExaminationDTO(_examination.Appointment, _examination.Room, _examination.Doctor, _examination.MedicalRecord);
            ExaminationRepository.GetInstance().Add(examinationDTO);
            appointmentNotificationDto = new AppointmentNotificationDTO(null, _examination.Appointment, _examination.Doctor, _examination.MedicalRecord.Patient);
            notificationRepository.Add(appointmentNotificationDto);

        }//EXTRACT
        private void DelayOperation(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationRepository notificationRepository = AppointmentNotificationRepository.GetInstance();
            OperationRepository.GetInstance().SwapOperationValue(selectedAppointment.NewOperation);
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
            notificationRepository.Add(appointmentNotificationDto);
            if (selectedAppointment.CurrentExamination == null)
            {
                _operation.Appointment = selectedAppointment.CurrentOperation.Appointment;
                _operation.Room = selectedAppointment.CurrentOperation.Room;
                _operation.Doctor = selectedAppointment.CurrentOperation.Doctor;
            }
            else
            {
                _operation.Appointment = selectedAppointment.CurrentExamination.Appointment;
                _operation.Room = selectedAppointment.CurrentExamination.Room;
                _operation.Doctor = selectedAppointment.CurrentExamination.Doctor;
            }
            OperationDTO operationDTO = new OperationDTO(_operation.Appointment, _operation.Duration, _operation.Room, _operation.Doctor, _operation.MedicalRecord);
            OperationRepository.GetInstance().Add(operationDTO);
            appointmentNotificationDto = new AppointmentNotificationDTO(null, _operation.Appointment, _operation.Doctor, _operation.MedicalRecord.Patient);
            notificationRepository.Add(appointmentNotificationDto);

        }//EXTRACT
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            
            ScheduleEditRequest selectedAppointment = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                if (_operation == null)
                {
                    DelayExamination(selectedAppointment);
                }
                else if (_examination == null)
                {
                    DelayOperation(selectedAppointment);
                }
            }
        }
    }
}
