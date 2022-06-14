using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.GUI.DoctorView;
using System.Windows;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.DoctorRatings;
using System.Windows.Controls;
using HealthInstitution.ViewModels.GUIViewModels;

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            EquipmentTransferRefreshingService.UpdateByTransfer();
            RenovationRefreshingService.UpdateByRenovation();

            LoginWindow window = new LoginWindow()
            {
                DataContext = new LoginViewModel()
            };
            window.ShowDialog();
        }
    }
}