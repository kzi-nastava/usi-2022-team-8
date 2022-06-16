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

namespace HealthInstitution.GUI.ManagerView.PollView
{
    /// <summary>
    /// Interaction logic for RatedDoctorsWindow.xaml
    /// </summary>
    public partial class RatedDoctorsWindow : Window
    {
        public RatedDoctorsWindow()
        {
            InitializeComponent();
        }
        public void SetDoctors(List<Doctor> doctors)
        {
            List<Doctor> doctorsToShow = doctors.Select(d => { d.AvgRating = Math.Round(d.AvgRating, 2); return d; }).ToList();
            dataGrid.ItemsSource = doctorsToShow;
        }
    }
}
