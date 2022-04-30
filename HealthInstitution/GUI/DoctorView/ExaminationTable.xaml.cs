using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
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
        ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
        DoctorRepository doctorRepository = DoctorRepository.GetInstance();
        ExaminationDoctorRepository examinationDoctorRepository = ExaminationDoctorRepository.GetInstance();
        public Doctor loggedDoctor { get; set; }
        public ExaminationTable(Doctor doctor)
        {
            this.loggedDoctor = doctor;
            InitializeComponent();
            LoadGridRows();
        }
        
        public void LoadGridRows()
        {
            dataGrid.Items.Clear();
            List<Examination> doctorExaminations = this.loggedDoctor.examinations;
            foreach (Examination examination in doctorExaminations)
            {
                dataGrid.Items.Add(examination);
            }
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddExaminationDialog(this.loggedDoctor).ShowDialog();
            LoadGridRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
            editExaminationDialog.ShowDialog();
            LoadGridRows();
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                dataGrid.Items.Remove(selectedExamination);
                examinationRepository.DeleteExamination(selectedExamination.id);
                doctorRepository.DeleteExamination(loggedDoctor, selectedExamination);
                examinationDoctorRepository.SaveToFile();
            }
        }
    }
}
