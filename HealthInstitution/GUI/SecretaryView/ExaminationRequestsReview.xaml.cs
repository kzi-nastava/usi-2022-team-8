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
            loadRows();
        }
        private void loadRows()
        {
            dataGrid.Items.Clear();
            List<ScheduleEditRequest> scheduleEditRequests = ScheduleEditRequestFileRepository.GetInstance().Requests;
            foreach (ScheduleEditRequest scheduleEditRequest in scheduleEditRequests)
            {
                dataGrid.Items.Add(scheduleEditRequest);
            }
            dataGrid.Items.Refresh();
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;  
            if(selectedRequest!=null)
            {
                ScheduleEditRequestFileRepository scheduleEditRequestRepository = ScheduleEditRequestFileRepository.GetInstance();
                scheduleEditRequestRepository.AcceptScheduleEditRequests(selectedRequest.Id);
            }
            loadRows();
        }

        private void reject_Click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                ScheduleEditRequestFileRepository scheduleEditRequestRepository = ScheduleEditRequestFileRepository.GetInstance();
                scheduleEditRequestRepository.RejectScheduleEditRequests(selectedRequest.Id);
            }
            loadRows();
        }
    }
}
