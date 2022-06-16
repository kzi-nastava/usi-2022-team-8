using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Model;
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

namespace HealthInstitution.GUI.ManagerView.PollView
{
    /// <summary>
    /// Interaction logic for HospitalPollWindow.xaml
    /// </summary>
    public partial class HospitalPollWindow : Window
    {
        IPollService _pollService;
        public HospitalPollWindow(IPollService pollService)
        {
            _pollService = pollService;
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            List<TableItemPoll> questions = _pollService.GetHospitalPollByQuestions();
            pollDataGrid.Items.Clear();
            pollDataGrid.ItemsSource = questions;

            List<PollComment> comments = _pollService.GetHospitalComments();
            commentDataGrid.Items.Clear();
            commentDataGrid.ItemsSource = comments;
        }
    }
}
