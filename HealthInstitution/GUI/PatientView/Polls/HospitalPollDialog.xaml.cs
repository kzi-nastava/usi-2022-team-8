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
using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.Polls;
using HealthInstitution.ViewModels.GUIViewModels.Polls;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for PatientPolllDialog.xaml
/// </summary>
public partial class PatientHospitalPollDialog : Window
{
    IPollService _pollService;
    public PatientHospitalPollDialog(IPollService pollService)
    {
        InitializeComponent();
        _pollService = pollService;
        DataContext = new HospitalPollDialogViewModel(pollService);
    }
}