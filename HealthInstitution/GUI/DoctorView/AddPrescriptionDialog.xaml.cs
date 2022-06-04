using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
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
    /// Interaction logic for AddPrescriptionDialog.xaml
    /// </summary>
    public partial class AddPrescriptionDialog : Window
    {
        private MedicalRecord _medicalRecord;
        public AddPrescriptionDialog(MedicalRecord medicalRecord)
        {
            InitializeComponent();
            this._medicalRecord = medicalRecord;
        }

        private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> hours = new List<String>();
            for (int i = 8; i <= 12; i++)
            {
                hours.Add(i.ToString());
            }
            hourComboBox.ItemsSource = hours;
            hourComboBox.SelectedIndex = 0;
        }

        private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> minutes = new List<String>();
            minutes.Add("00");
            minutes.Add("30");
            minuteComboBox.ItemsSource = minutes;
            minuteComboBox.SelectedIndex = 0;
        }

        private void TimeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var timeComboBox = sender as System.Windows.Controls.ComboBox;
            timeComboBox.Items.Add("Not important");
            timeComboBox.Items.Add("Pre meal");
            timeComboBox.Items.Add("During meal");
            timeComboBox.Items.Add("Post meal");
            timeComboBox.SelectedIndex = 0;
            timeComboBox.Items.Refresh();
        }

        private void DrugComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var drugComboBox = sender as System.Windows.Controls.ComboBox;
            List<Drug> drugs = DrugService.GetAll();
            foreach (Drug drug in drugs)
            {
                drugComboBox.Items.Add(drug);
            }
            drugComboBox.SelectedIndex = 0;
            drugComboBox.Items.Refresh();
        }

        private PrescriptionDTO CreatePrescriptionDTOFromInputData()
        {
            Drug drug = (Drug)drugComboBox.SelectedItem;
            if (PrescriptionService.IsPatientAlergic(_medicalRecord,drug.Ingredients))
                throw new Exception("Patient is alergic to drug ingredients");
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            DateTime hourlyRate = DateTime.Now.AddHours(hours).AddMinutes(minutes);
            PrescriptionTime timeOfUse = (PrescriptionTime)timeComboBox.SelectedIndex;
            int dailyDose = Int32.Parse(doseTextBox.Text);
            PrescriptionDTO prescription = new PrescriptionDTO(dailyDose, timeOfUse, drug, hourlyRate);
            return prescription;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrescriptionDTO prescriptionDTO = CreatePrescriptionDTOFromInputData();
                Prescription prescription = PrescriptionService.Add(prescriptionDTO);
                MedicalRecordService.AddPrescription(_medicalRecord.Patient, prescription);
                System.Windows.MessageBox.Show("You have created the prescription!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
