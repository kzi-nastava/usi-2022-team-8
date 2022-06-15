using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
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
    /// Interaction logic for ExaminationTable.xaml
    /// </summary>
    public partial class ExaminationTable : Window
    {
        private Doctor _loggedDoctor;
        IExaminationService _examinationService;
        IDoctorService _doctorService;
        public ExaminationTable(IExaminationService examinationService, IDoctorService doctorService)
        {
            _examinationService = examinationService;
            _doctorService = doctorService;
            InitializeComponent();
            LoadRows();
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Examination> doctorExaminations = _examinationService.GetByDoctor(_loggedDoctor.Username);
            foreach (Examination examination in doctorExaminations)
            {
                dataGrid.Items.Add(examination);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddExaminationDialog(this._loggedDoctor, DIContainer.GetService<IPatientService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<ISchedulingService>()).ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            new EditExaminationDialog(selectedExamination, DIContainer.GetService<IPatientService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<IExaminationService>()).ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected examination?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                dataGrid.Items.Remove(selectedExamination);
                _examinationService.Delete(selectedExamination.Id);
                _doctorService.DeleteExamination(selectedExamination);
            }
        }
    }
}
