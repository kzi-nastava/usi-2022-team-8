using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddRestRequestDialog.xaml
    /// </summary>
    public partial class AddRestRequestDialog : Window
    {
        Doctor _loggedDoctor;
        IRestRequestService _restRequestService;
        public AddRestRequestDialog(IRestRequestService restRequestService)
        {
            _restRequestService = restRequestService;
            InitializeComponent();
            Load();
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }
        public void Load()
        {
            urgentRadioButton.IsChecked = false;
            notUrgentRadioButton.IsChecked = true;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public RestRequestDTO CreateRestRequestDTOFromInputData()
        {
            DateTime startDate = (DateTime)datePicker.SelectedDate;
            int daysDuration = Int32.Parse(numberOfDaysTextBox.Text);
            String requestReason = requestReasonTextBox.Text;
            bool isUrgent = (bool)urgentRadioButton.IsChecked;
            RestRequestDTO restRequestDTO = new RestRequestDTO(_loggedDoctor, requestReason, startDate, daysDuration, RestRequestState.OnHold, isUrgent, "");
            return restRequestDTO;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var restRequestDTO = CreateRestRequestDTOFromInputData();
                _restRequestService.ApplyForRestRequest(restRequestDTO);
                System.Windows.MessageBox.Show("You have applied for rest days!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UrgentChecked(object sender, RoutedEventArgs e)
        {

        }

        private void NotUrgentChecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
