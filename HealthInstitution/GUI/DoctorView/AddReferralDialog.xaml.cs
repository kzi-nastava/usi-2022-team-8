using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Referrals.Model;
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
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddReferralDialog.xaml
    /// </summary>
    public partial class AddReferralDialog : Window
    {
        private Patient _patient;
        private Doctor _doctor;
        private DoctorRepository _doctorRepository = DoctorRepository.GetInstance();
        private MedicalRecordRepository _medicalRecordRepository = MedicalRecordRepository.GetInstance();
        private ReferralRepository _referralRepository = ReferralRepository.GetInstance();  
        public AddReferralDialog(Doctor doctor, Patient patient)
        {
            _patient = patient;
            _doctor = doctor;
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            doctorComboBox.IsEnabled = false;
            specialtyComboBox.IsEnabled = false;
        }

        private void DoctorChecked(object sender, RoutedEventArgs e)
        {
            specialtyComboBox.IsEnabled = false;
            doctorComboBox.IsEnabled = true;
        }

        private void SpecialtyChecked(object sender, RoutedEventArgs e)
        {
            doctorComboBox.IsEnabled = false;
            specialtyComboBox.IsEnabled = true;
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            List<Doctor> doctors = _doctorRepository.GetAll();
            foreach (Doctor doctor in doctors)
            {
                if (_doctor.Username != doctor.Username)
                    doctorComboBox.Items.Add(doctor);
            }
            doctorComboBox.SelectedIndex = 0;
            doctorComboBox.Items.Refresh();
        }

        private void SpecialtyComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var specialtyComboBox = sender as System.Windows.Controls.ComboBox;
            specialtyComboBox.Items.Add("GeneralPractitioner");
            specialtyComboBox.Items.Add("Surgeon");
            specialtyComboBox.Items.Add("Radiologist");
            specialtyComboBox.Items.Add("Pediatrician");
            specialtyComboBox.SelectedIndex = 0;
            specialtyComboBox.Items.Refresh();
        }

        private void CreateWithDoctor()
        {
            this.Close();
            Doctor refferedDoctor = (Doctor)doctorComboBox.SelectedItem;
            ReferralDTO referralDTO = new ReferralDTO(ReferralType.SpecificDoctor, _doctor, refferedDoctor, null);
            Referral referral = _referralRepository.Add(referralDTO);
            _medicalRecordRepository.AddReferral(_patient, referral);
            System.Windows.MessageBox.Show("You have created the referral!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateWithSpecialty()
        {
            this.Close();
            SpecialtyType specialtyType = (SpecialtyType)specialtyComboBox.SelectedIndex;
            ReferralDTO referralDTO = new ReferralDTO(ReferralType.Specialty, _doctor, null, specialtyType);
            Referral referral = _referralRepository.Add(referralDTO);
            _medicalRecordRepository.AddReferral(_patient, referral);
            System.Windows.MessageBox.Show("You have created the referral!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)doctorRadioButton.IsChecked)
            {
                CreateWithDoctor();
            }
            else if ((bool)specialtyRadioButton.IsChecked)
            {
                CreateWithSpecialty();
            }
            else
            {
                System.Windows.MessageBox.Show("Select one of two options!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
