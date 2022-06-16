using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.ViewModels.GUIViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands
{
    public class LogoutCommand : CommandBase
    {
        public Window Window { get; set; }
        public LogoutCommand(Window window)
        {
            Window = window;
        }

        public override void Execute(object? parameter)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Window.Close();
                var window = DIContainer.GetService<LoginWindow>();
                window.ShowDialog();
            }
        }
    }
}