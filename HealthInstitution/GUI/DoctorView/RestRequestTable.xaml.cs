using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests;
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
        public RestRequestTable(Doctor doctor)
        {
            InitializeComponent();
            DataContext = new RestRequestTableViewModel(doctor);
        }
    }
}