using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
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
            ScheduleEditRequest selectedAppointment = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                if (Operation == null)
                {
                    ExaminationRepository.GetInstance().SwapExaminationValue(selectedAppointment.NewExamination);
                    if (selectedAppointment.CurrentExamination == null)
                        Examination.Appointment = selectedAppointment.CurrentOperation.Appointment;
                    else
                        Examination.Appointment = selectedAppointment.CurrentExamination.Appointment;
                    ExaminationRepository.GetInstance().AddExamination(Examination.Appointment, Examination.Room, Examination.Doctor, Examination.MedicalRecord);
                }
                if (Examination == null)
                {
                    OperationRepository.GetInstance().SwapOperationValue(selectedAppointment.NewOperation);
                    if (selectedAppointment.CurrentExamination == null)
                        Operation.Appointment = selectedAppointment.CurrentOperation.Appointment;
                    else
                        Operation.Appointment = selectedAppointment.CurrentExamination.Appointment;
                    OperationRepository.GetInstance().Add(Operation.Appointment, Operation.Duration, Operation.Room, Operation.Doctor, Operation.MedicalRecord);
                }
            }
        }
    }
}
