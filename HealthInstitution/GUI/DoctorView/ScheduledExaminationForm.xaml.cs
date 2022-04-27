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
    /// Interaction logic for ScheduledExaminationForm.xaml
    /// </summary>
    public partial class ScheduledExaminationForm : Window
    {
        public ScheduledExaminationForm()
        {
      
            InitializeComponent();
            ExaminationRadioButton.IsChecked = true;
            UpcomingDaysRadioButton.IsChecked = true;
        }

        private void AppointmentChecked(object sender, RoutedEventArgs e)
        {

        }

        private void UpcomingDatesChecked(object sender, RoutedEventArgs e)
        {

        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {

        }

        /*[STAThread]
        static void Main(string[] args)
        {
            ScheduledExaminationForm window = new ScheduledExaminationForm();
            window.ShowDialog();
        }*/

        private void ShowMedicalRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartExamination_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
