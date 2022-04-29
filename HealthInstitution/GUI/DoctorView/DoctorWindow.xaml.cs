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
using HealthInstitution.GUI.LoginWindow;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        public Doctor loggedDoctor { get; set; }
        public DoctorWindow(Doctor loggedDoctor)
        {
            this.loggedDoctor = loggedDoctor;
            InitializeComponent();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow.LoginWindow lw = new LoginWindow.LoginWindow();
                lw.ShowDialog();
            }
        }

        private void Examinations_Click(object sender, RoutedEventArgs e)
        {
            ExaminationForm examinationForm = new ExaminationForm(this.loggedDoctor);
            examinationForm.ShowDialog();
        }

        private void Operations_Click(object sender, RoutedEventArgs e)
        {
            OperationForm operationForm = new OperationForm(this.loggedDoctor);
            operationForm.ShowDialog();
        }

        private void ScheduleReview_Click(object sender, RoutedEventArgs e)
        {
            ScheduledExaminationForm scheduledExaminationForm = new ScheduledExaminationForm(this.loggedDoctor);
            scheduledExaminationForm.ShowDialog();
        }
    }

}
