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
using System.Windows.Forms;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        public DoctorWindow(Doctor doctor)
        {
            InitializeComponent();
            //ShowNotificationsDialog();
            DataContext = new DoctorWindowViewModel(doctor);
        }
       /* private void ShowNotificationsDialog()
        {
            if (_doctorService.GetActiveAppointmentNotification(_loggedDoctor).Count + _doctorService.GetActiveRestRequestNotification(_loggedDoctor).Count > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = DIContainer.GetService<DoctorNotificationsDialog>();
                doctorNotificationsDialog.SetLoggedDoctor(this._loggedDoctor);
                doctorNotificationsDialog.ShowDialog();
                    
            }
        }*/
    }
}
