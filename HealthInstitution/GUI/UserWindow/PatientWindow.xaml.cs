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
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.TrollCounters.Model;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private User loggedPatient;

        public PatientWindow(User loggedPatient)
        {
            InitializeComponent();
            this.loggedPatient = loggedPatient;
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                new LoginWindow.LoginWindow().ShowDialog();
                //lw.ShowDialog();
            }
        }

        private void Schedule_Button_Click(object sender, RoutedEventArgs e)
        {
            var auto = TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.username);
            Console.WriteLine(auto.ToString());

            new PatientScheduleWindow(this.loggedPatient).ShowDialog();
        }
    }
}