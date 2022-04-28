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
    /// Interaction logic for CrudbPatients.xaml
    /// </summary>
    public partial class CrudbPatients : Window
    {
        public CrudbPatients()
        {
            InitializeComponent();
        }
        private void createPatient_click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
            this.Close();
        }

        private void updatePatient_click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
            this.Close();
        }

        private void deletePatient_click(object sender, RoutedEventArgs e)
        {
            ExaminationRequestsReview examinationRequestsReview = new ExaminationRequestsReview();
            examinationRequestsReview.ShowDialog();
            this.Close();
        }

        private void blockPatient_click(object sender, RoutedEventArgs e)
        {
            ExaminationRequestsReview examinationRequestsReview = new ExaminationRequestsReview();
            examinationRequestsReview.ShowDialog();
            this.Close();
        }
    }
}
