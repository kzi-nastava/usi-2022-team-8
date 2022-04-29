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

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for SecretaryWindow.xaml
    /// </summary>
    public partial class SecretaryWindow : Window
    {
        public SecretaryWindow()
        {
            InitializeComponent();
        }

        private void patients_click(object sender, RoutedEventArgs e)
        {
            CrudbPatients crudbPatientsWindow = new CrudbPatients();
            crudbPatientsWindow.ShowDialog();
            //this.Close();
        }


        private void examinationRequests_click(object sender, RoutedEventArgs e)
        {
            ExaminationRequestsReview examinationRequestsReview = new ExaminationRequestsReview();
            examinationRequestsReview.ShowDialog();
            //this.Close();
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

        /*[STAThread]
        static void Main(string[] args)
        {
            SecretaryWindow secretaryWindow = new SecretaryWindow();
            secretaryWindow.ShowDialog();   
        }*/
    }
}
