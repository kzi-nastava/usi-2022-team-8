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
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for MedicalRecordView.xaml
    /// </summary>
    public partial class MedicalRecordView : Window
    {
        private User _loggedPatient;
        private List<Examination> _currentExaminations;

        public MedicalRecordView(User loggedPatient)
        {
            InitializeComponent();
            _loggedPatient = loggedPatient;
            loadAllRows();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DoctorButton_Click(object sender, RoutedEventArgs e)
        {
            _currentExaminations = _currentExaminations.OrderBy(o => o.Doctor.Username).ToList();
            loadRows(_currentExaminations);
        }

        private void DateButton_Click(object sender, RoutedEventArgs e)
        {
            _currentExaminations = _currentExaminations.OrderBy(o => o.Appointment).ToList();
            loadRows(_currentExaminations);
        }

        private void SpecializationButton_Click(object sender, RoutedEventArgs e)
        {
            _currentExaminations = _currentExaminations.OrderBy(o => o.Doctor.Specialty).ToList();
            loadRows(_currentExaminations);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void loadAllRows()
        {
            var allRows = ExaminationRepository.GetInstance().GetCompletedByPatient(_loggedPatient.Username);
            loadRows(allRows);
        }

        private void loadRows(List<Examination> examinations)
        {
            _currentExaminations = examinations;
            dataGrid.Items.Clear();
            foreach (Examination examination in examinations)
            {
                if (examination.MedicalRecord.Patient.Username.Equals(_loggedPatient.Username))
                    dataGrid.Items.Add(examination);
            }
            dataGrid.Items.Refresh();
        }
    }
}