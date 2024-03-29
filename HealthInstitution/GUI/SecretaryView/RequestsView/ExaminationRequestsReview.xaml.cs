﻿using HealthInstitution.Core.ScheduleEditRequests;
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
        IScheduleEditRequestsService _scheduleEditRequestsService;

        public ExaminationRequestsReview(IScheduleEditRequestsService scheduleEditRequestsService)
        {
            InitializeComponent();
            _scheduleEditRequestsService = scheduleEditRequestsService;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<ScheduleEditRequest> scheduleEditRequests = _scheduleEditRequestsService.GetAll();
            foreach (ScheduleEditRequest scheduleEditRequest in scheduleEditRequests)
            {
                dataGrid.Items.Add(scheduleEditRequest);
            }
            dataGrid.Items.Refresh();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;  
            if(selectedRequest!=null)
            {
                _scheduleEditRequestsService.AcceptScheduleEditRequests(selectedRequest.Id);
            }
            LoadRows();
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            ScheduleEditRequest selectedRequest = (ScheduleEditRequest)dataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                _scheduleEditRequestsService.RejectScheduleEditRequests(selectedRequest.Id);
            }
            LoadRows();
        }
    }
}
