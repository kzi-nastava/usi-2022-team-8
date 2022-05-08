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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor _loggedDoctor;
        public DoctorWindow(Doctor doctor)
        {
            this._loggedDoctor = doctor;
            InitializeComponent();
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
            }
        }

        private void examinations_Click(object sender, RoutedEventArgs e)
        {
            new ExaminationTable(this._loggedDoctor).ShowDialog();
        }

        private void operations_Click(object sender, RoutedEventArgs e)
        {
            new OperationTable(this._loggedDoctor).ShowDialog();
        }

        private void scheduleReview_Click(object sender, RoutedEventArgs e)
        {
            new ScheduledExaminationTable(this._loggedDoctor).ShowDialog();
        }
    }

}
