using HealthInstitution.Core.Examinations.Model;
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
        public ExaminationForm()
        {
            InitializeComponent();
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
            }
        }

        private void medicalRecordButton_click(object sender, RoutedEventArgs e)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            MedicalRecordDialog medicalRecordDialog = new MedicalRecordDialog();
            medicalRecordDialog.ShowDialog();
        }

        /*[STAThread]
        static void Main(string[] args)
        {
            ExaminationForm window = new ExaminationForm();
            window.ShowDialog();
        }*/


    }
}
