using HealthInstitution.Commands;
using HealthInstitution.Commands.DoctorCommands.DoctorWindow;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels
{
    public class DoctorWindowViewModel : ViewModelBase
    {
        Doctor _loggedDoctor;
        IDoctorService _doctorService;
        public DoctorWindowViewModel(Doctor loggedDoctor, Window thisWindow, IDoctorService doctorService)
        {
            _loggedDoctor = loggedDoctor;   
            _doctorService = doctorService;
            ShowNotificationsDialog();   
            OperationsTableCommand = new OperationsTableCommand(loggedDoctor);
            ExaminationsTableCommand = new ExaminationsTableCommand(loggedDoctor);
            ScheduleReviewCommand = new ScheduleReviewCommand(loggedDoctor);
            DrugManagementCommand = new DrugManagementCommand();
            LogoutCommand = new LogoutCommand(thisWindow);
            RestRequestTableCommand = new RestRequestTableCommand(loggedDoctor);
        }

        public ICommand OperationsTableCommand { get; }
        public ICommand ExaminationsTableCommand { get; }
        public ICommand ScheduleReviewCommand { get; }
        public ICommand DrugManagementCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RestRequestTableCommand { get; }

        private void ShowNotificationsDialog()
        {
            if (_doctorService.GetActiveAppointmentNotification(_loggedDoctor).Count + _doctorService.GetActiveRestRequestNotification(_loggedDoctor).Count > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = DIContainer.GetService<DoctorNotificationsDialog>();
                doctorNotificationsDialog.SetLoggedDoctor(this._loggedDoctor);
                doctorNotificationsDialog.ShowDialog();

            }
        }
    }

}