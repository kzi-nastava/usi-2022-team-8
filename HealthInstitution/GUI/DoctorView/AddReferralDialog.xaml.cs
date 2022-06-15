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
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Doctors;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddReferralDialog.xaml
    /// </summary>
    public partial class AddReferralDialog : Window
    {
        private Patient _patient;
        private Doctor _doctor;
        IDoctorService _doctorService;
        IReferralService _referralService;
        IMedicalRecordService _medicalRecordService;
        public AddReferralDialog(IDoctorService doctorService,
            IReferralService referralService, IMedicalRecordService medicalRecordService)
        {
            _doctorService = doctorService;
            _referralService = referralService;
            _medicalRecordService = medicalRecordService;
            InitializeComponent();
            Load();
        }
        public void SetReferralFields(Patient patient, Doctor doctor)
        { 
            _patient = patient;
            _doctor = doctor;
        }
        public void Load()
        {
            doctorRadioButton.IsChecked = true;
            doctorComboBox.IsEnabled = true;
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
            List<Doctor> doctors = _doctorService.GetAll();
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

        private ReferralDTO CreateReferralDTOWithDoctor()
        {
            Doctor refferedDoctor = (Doctor)doctorComboBox.SelectedItem;
            return new ReferralDTO(ReferralType.SpecificDoctor, _doctor, refferedDoctor, null);
        }

        private ReferralDTO CreateReferralDTOWithSpecialty()
        {
            SpecialtyType specialtyType = (SpecialtyType)specialtyComboBox.SelectedIndex;
            return new ReferralDTO(ReferralType.Specialty, _doctor, null, specialtyType);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ReferralDTO referralDTO;
            if ((bool)doctorRadioButton.IsChecked)
            {
                referralDTO = CreateReferralDTOWithDoctor();
            } else
            {
                referralDTO = CreateReferralDTOWithSpecialty();
            }
            Referral referral = _referralService.Add(referralDTO);
            _medicalRecordService.AddReferral(_patient, referral);
            System.Windows.MessageBox.Show("You have created the referral!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
