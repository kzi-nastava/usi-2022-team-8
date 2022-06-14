using HealthInstitution.Core.RestRequestNotifications;
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
    /// Interaction logic for RestRequestsReview.xaml
    /// </summary>
    public partial class RestRequestsReview : Window
    {
        IRestRequestService _restRequestService;
        public RestRequestsReview(IRestRequestService restRequestService)
        {
            InitializeComponent();
            _restRequestService = restRequestService;
            LoadRows();
        }

        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<RestRequest> activeRestRequests = _restRequestService.GetActive();
            foreach (RestRequest restRequest in activeRestRequests)
            {
                dataGrid.Items.Add(restRequest);
            }
            dataGrid.Items.Refresh();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            RestRequest selectedRequest = (RestRequest)dataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                _restRequestService.AcceptRestRequest(selectedRequest);
            }
            LoadRows();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            RestRequest selectedRequest = (RestRequest)dataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                RestRequestRejectionDialog restRequestRejectionDialog = new RestRequestRejectionDialog(selectedRequest);
                restRequestRejectionDialog.ShowDialog();    
            }
            LoadRows();
        }
    }
}
