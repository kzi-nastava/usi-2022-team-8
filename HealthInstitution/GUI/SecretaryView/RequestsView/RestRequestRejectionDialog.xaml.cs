using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
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

namespace HealthInstitution.GUI.SecretaryView.RequestsView
{
    /// <summary>
    /// Interaction logic for RestRequestRejectionDialog.xaml
    /// </summary>
    public partial class RestRequestRejectionDialog : Window
    {
        RestRequest _selectedRestRequest;
        IRestRequestService _restRequestService;
        public RestRequestRejectionDialog(IRestRequestService restRequestService)
        {
            _restRequestService = restRequestService;
            InitializeComponent();
        }

        public void SetSelectedRequest(RestRequest restRequest)
        {
            _selectedRestRequest = restRequest;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            String rejectionReason = commentTextBox.Text;
            if (rejectionReason.Trim() == "")
            {
                System.Windows.MessageBox.Show("You have to write a reason for rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _restRequestService.RejectRestRequest(_selectedRestRequest, rejectionReason);
                System.Windows.MessageBox.Show("Successfull rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
