using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Notifications;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorNotificationsDialog.xaml
    /// </summary>
    public partial class DoctorNotificationsDialog : Window
    {
        IDoctorService _doctorService;
        IAppointmentNotificationService _appointmentNotificationService;
        IRestRequestNotificationService _restRequestNotificationService;
        Doctor _loggedDoctor;
        public DoctorNotificationsDialog(IDoctorService doctorService, IAppointmentNotificationService appointmentNotificationService, IRestRequestNotificationService restRequestNotificationService)
        {
            InitializeComponent();
            _doctorService = doctorService;
            _appointmentNotificationService = appointmentNotificationService;
            _restRequestNotificationService = restRequestNotificationService;
        }

        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            DataContext = new DoctorNotificationsDialogViewModel(doctor, _doctorService, _appointmentNotificationService, _restRequestNotificationService);
        }
    }
}
