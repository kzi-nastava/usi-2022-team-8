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
            int activeNotifications = 0;
            foreach (AppointmentNotification notification in this._loggedDoctor.Notifications)
            {
                if (notification.ActiveForDoctor)
                    activeNotifications++;
            }
            if (activeNotifications > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = new DoctorNotificationsDialog(this._loggedDoctor);
                doctorNotificationsDialog.ShowDialog();
            }
        }*/
    }
}
