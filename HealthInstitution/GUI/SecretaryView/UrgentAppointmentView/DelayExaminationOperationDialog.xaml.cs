using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.Scheduling;
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
        IAppointmentDelayingService _appointmentDelayingService;
        public DelayExaminationOperationDialog(IAppointmentDelayingService appointmentDelayingService)
        {
            InitializeComponent();
            _appointmentDelayingService = appointmentDelayingService;
            
        }
        public void SetSelectedAppointment(List<ScheduleEditRequest> delayedAppointments, Examination? examination, Operation? operation)
        {
            _delayedAppointments = delayedAppointments;
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
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedAppointment = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedAppointment != null)
            {
                if (_examination != null)
                    _appointmentDelayingService.DelayExamination(selectedAppointment,_examination);
                else if(_operation != null)
                    _appointmentDelayingService.DelayOperation(selectedAppointment, _operation);
            }
        }
    }
}
