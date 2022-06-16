using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests;
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
        Doctor _doctor;
        IRestRequestService _restRequestService;
        public AddRestRequestDialog(IRestRequestService restRequestService)
        {
            InitializeComponent();
            _restRequestService = restRequestService;
        }

        public void SetLoggedDoctor(Doctor doctor)
        {
            _doctor = doctor;
            DataContext = new AddRestRequestDialogViewModel(this, doctor,_restRequestService);
        }
    }
}
