using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
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
    /// Interaction logic for ExaminationRequestsReview.xaml
    /// </summary>
    public partial class ExaminationRequestsReview : Window
    {
        public ExaminationRequestsReview()
        {
            InitializeComponent();
            LoadGridRows();
        }
        public void LoadGridRows()
        {
            dataGrid.Items.Clear();
            List<ScheduleEditRequest> scheduleEditRequests = ScheduleEditRequestRepository.GetInstance().scheduleEditRequests;
            foreach (ScheduleEditRequest scheduleEditRequest in scheduleEditRequests)
            {
                dataGrid.Items.Add(scheduleEditRequests);
            }
            dataGrid.Items.Refresh();
        }

        private void accept_click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;  
            if(selectedRequest!=null)
            {
                ScheduleEditRequestRepository scheduleEditRequestRepository = ScheduleEditRequestRepository.GetInstance();
                scheduleEditRequestRepository.AcceptScheduleEditRequests(selectedRequest.Id);
            }
            LoadGridRows();
        }

        private void reject_click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                ScheduleEditRequestRepository scheduleEditRequestRepository = ScheduleEditRequestRepository.GetInstance();
                scheduleEditRequestRepository.RejectScheduleEditRequests(selectedRequest.Id);
            }
            LoadGridRows();
        }
    }
}
