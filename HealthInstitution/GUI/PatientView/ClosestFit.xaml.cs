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
using HealthInstitution.Core.Examinations.Repository;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for ClosestFit.xaml
    /// </summary>
    public partial class ClosestFit : Window
    {
        private List<Examination> _suggestions;

        public ClosestFit(List<Examination> suggestions)
        {
            InitializeComponent();
            _suggestions = suggestions;
            loadRows();
        }

        private void addButton_click(object sender, RoutedEventArgs e)
        {
            var examinationRepository = ExaminationRepository.GetInstance();
            Examination selectedExamination = _suggestions[0];
            if (secondRadioButton.IsChecked == true) selectedExamination = _suggestions[1];
            if (thirdRadioButton.IsChecked == true) selectedExamination = _suggestions[2];
            examinationRepository.Add(selectedExamination);
            this.Close();
        }

        private void loadRows()
        {
            foreach (Examination examination in _suggestions)
            {
                dataGrid.Items.Add(examination);
            }
            dataGrid.Items.Refresh();
        }
    }
}