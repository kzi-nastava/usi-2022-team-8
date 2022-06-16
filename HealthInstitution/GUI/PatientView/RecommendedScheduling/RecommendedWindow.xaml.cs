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
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecommendedWindow.xaml
/// </summary>
///
public partial class RecommendedWindow : Window
{
    private User _loggedPatient;
    private IRecommendedSchedulingService _recommendedSchedulingService;
    private IDoctorService _doctorService;

    public RecommendedWindow(IRecommendedSchedulingService recommendedSchedulingService, IDoctorService doctorService)
    {
        InitializeComponent();
        _recommendedSchedulingService = recommendedSchedulingService;
        _doctorService = doctorService;
    }

    public void SetLoggedPatient(User patient)
    {
        _loggedPatient = patient;
        DataContext = new RecommendedWindowViewModel(this, patient, _recommendedSchedulingService, _doctorService);
    }
}