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

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for ClosestFit.xaml
    /// </summary>
    public partial class ClosestFit : Window
    {
        private List<Examination> _suggestions;
        IExaminationService _examinationService;

        public ClosestFit(List<Examination> suggestions, IExaminationService examinationService)
        {
            InitializeComponent();
            _suggestions = suggestions;
            _examinationService = examinationService;
            LoadRows();
        }

        private void AddButton_click(object sender, RoutedEventArgs e)
        {
            Examination selectedExamination = _suggestions[0];
            if (secondRadioButton.IsChecked == true) selectedExamination = _suggestions[1];
            if (thirdRadioButton.IsChecked == true) selectedExamination = _suggestions[2];
            _examinationService.Add(_examinationService.ParseExaminationToExaminationDTO(selectedExamination));
            this.Close();
        }

        private void LoadRows()
        {
            foreach (Examination examination in _suggestions)
            {
                dataGrid.Items.Add(examination);
            }
            dataGrid.Items.Refresh();
        }
    }
}