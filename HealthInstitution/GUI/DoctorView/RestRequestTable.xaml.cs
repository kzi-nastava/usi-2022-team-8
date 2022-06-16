using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
    /// Interaction logic for RestRequestTable.xaml
    /// </summary>
    public partial class RestRequestTable : Window
    {
        Doctor _loggedDoctor;
        IRestRequestService _restRequestService;
        public RestRequestTable(IRestRequestService restRequestService)
        {
            _restRequestService = restRequestService;
            InitializeComponent();
            
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<RestRequest> activeRestRequests = _restRequestService.GetByDoctor(_loggedDoctor.Username);
            foreach (RestRequest restRequest in activeRestRequests)
            {
                dataGrid.Items.Add(restRequest);
            }
            dataGrid.Items.Refresh();
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            AddRestRequestDialog addRestRequestDialog = DIContainer.GetService<AddRestRequestDialog>();
            addRestRequestDialog.SetLoggedDoctor(_loggedDoctor);
            addRestRequestDialog.ShowDialog();
            LoadRows();
        }
    }
}