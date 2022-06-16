using HealthInstitution.Commands;
using HealthInstitution.Commands.DoctorCommands.DoctorWindow;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels
{
    public class DoctorWindowViewModel : ViewModelBase
    {
        public DoctorWindowViewModel(Doctor loggedDoctor)
        {
            OperationsTableCommand = new OperationsTableCommand(loggedDoctor);
            ExaminationsTableCommand = new ExaminationsTableCommand(loggedDoctor);
            ScheduleReviewCommand = new ScheduleReviewCommand(loggedDoctor);
            DrugManagementCommand = new DrugManagementCommand();
            LogoutCommand = new LogoutCommand();
        }

        public ICommand OperationsTableCommand { get; }
        public ICommand ExaminationsTableCommand { get; }
        public ICommand ScheduleReviewCommand { get; }
        public ICommand DrugManagementCommand { get; }
        public ICommand LogoutCommand { get; }
    }
}