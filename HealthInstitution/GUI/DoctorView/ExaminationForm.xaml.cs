using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
    /// Interaction logic for ExaminationForm.xaml
    /// </summary>
    public partial class ExaminationForm : Window
    {
        ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
        public ExaminationForm(Doctor loggedDoctor)
        {
            InitializeComponent();
            LoadGridRows(loggedDoctor);
        }
        
        public void LoadGridRows(Doctor loggedDoctor)
        {
            List<Examination> doctorExaminations = loggedDoctor.examinations;
            foreach (Examination examination in doctorExaminations)
            {
                dataGrid.Items.Add(examination);
            }
        }
        private void addButton_click(object sender, RoutedEventArgs e)
        {
            AddExaminationDialog addExaminationDialog = new AddExaminationDialog();
            addExaminationDialog.ShowDialog();
            // TODO: UPDATE TABLE
        }

        private void editButton_click(object sender, RoutedEventArgs e)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
            editExaminationDialog.ShowDialog();

        }

        private void deleteButton_click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                dataGrid.Items.Remove(selectedExamination);
                examinationRepository.DeleteExamination(selectedExamination.id);
                //dodaj isto za doktora.
            }
        }

        /*[STAThread]
        static void Main(string[] args)
        {
            ExaminationForm window = new ExaminationForm();
            window.ShowDialog();
        }*/


    }
}
