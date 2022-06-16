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
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.GUIViewModels.Polls;

namespace HealthInstitution.GUI.PatientView.Polls;

/// <summary>
/// Interaction logic for DoctorPollDialog.xaml
/// </summary>
public partial class DoctorPollDialog : Window
{
    private Doctor _doctor;
    private IPollService _pollService;

    public DoctorPollDialog(IPollService pollService)
    {
        InitializeComponent();
        _pollService = pollService;
    }

    public void SetRatedDoctor(Doctor doctor)
    {
        _doctor = doctor;
        DataContext = new DoctorPollViewModel(this, doctor, _pollService);
    }
}