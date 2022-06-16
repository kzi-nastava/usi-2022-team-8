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
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for ClosestFit.xaml
/// </summary>
public partial class ClosestFit : Window
{
    private List<Examination> _suggestions;
    IExaminationService _examinationService;

    public ClosestFit(IExaminationService examinationService)
    {
        InitializeComponent();
        _examinationService = examinationService;

    }
    public void SetSuggestions(List<Examination> suggestions)
    {
        _suggestions = suggestions;
        DataContext = new ClosestFitViewModel(suggestions, _examinationService);
    }
}