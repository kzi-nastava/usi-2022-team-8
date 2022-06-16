using HealthInstitution.Commands.DoctorCommands.Referrals;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Referrals
{
    public class AddReferralDialogViewModel : ViewModelBase
    {
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        private object _choice;

        public object Choice
        {
            get
            {
                return _choice;
            }
            set
            {
                _choice = value;
                OnPropertyChanged(nameof(Choice));
            }
        }

        public int GetChosenType()
        {
            return Convert.ToInt32(Choice as string);
        }

        private ObservableCollection<Doctor> _doctorComboBoxItems;

        public ObservableCollection<Doctor> DoctorComboBoxItems
        {
            get
            {
                return _doctorComboBoxItems;
            }
            set
            {
                _doctorComboBoxItems = value;
                OnPropertyChanged(nameof(DoctorComboBoxItems));
            }
        }
        private int _doctorCombBoxSelectedIndex;

        public int DoctorComboBoxSelectedIndex
        {
            get
            {
                return _doctorCombBoxSelectedIndex;
            }
            set
            {
                _doctorCombBoxSelectedIndex = value;
                OnPropertyChanged(nameof(DoctorComboBoxSelectedIndex));
            }
        }

        private ObservableCollection<string> _specialtyComboBoxItems;

        public ObservableCollection<string> SpecialtyComboBoxItems
        {
            get
            {
                return _specialtyComboBoxItems;
            }
            set
            {
                _specialtyComboBoxItems = value;
                OnPropertyChanged(nameof(SpecialtyComboBoxItems));
            }
        }
        private int _specialtyCombBoxSelectedIndex;

        public int SpecialtyComboBoxSelectedIndex
        {
            get
            {
                return _specialtyCombBoxSelectedIndex;
            }
            set
            {
                _specialtyCombBoxSelectedIndex = value;
                OnPropertyChanged(nameof(SpecialtyComboBoxSelectedIndex));
            }
        }


        public Doctor GetDoctor()
        {
            return DoctorComboBoxItems[DoctorComboBoxSelectedIndex];
        }

        public SpecialtyType GetSpecialtyType()
        {
            return (SpecialtyType)SpecialtyComboBoxSelectedIndex;
        }

        private void LoadDoctorComboBox()
        {
            DoctorComboBoxItems = new();
            foreach (Doctor doctor in DoctorService.GetAll())
            {
                DoctorComboBoxItems.Add(doctor);
            }
            DoctorComboBoxSelectedIndex = 0;
        }
        private void LoadSpecialtyComboBox()
        {
            SpecialtyComboBoxItems.Add("GeneralPractitioner");
            SpecialtyComboBoxItems.Add("Surgeon");
            SpecialtyComboBoxItems.Add("Radiologist");
            SpecialtyComboBoxItems.Add("Pediatrician");
            SpecialtyComboBoxSelectedIndex = 0;
        }

        public ICommand CreateReferralCommand { get; }

        public AddReferralDialogViewModel(Patient patient, Doctor doctor)
        {
            Patient = patient;
            Doctor = doctor;
            LoadDoctorComboBox();
            LoadSpecialtyComboBox();
            CreateReferralCommand = new CreateReferralCommand(this);
        }
    }
}
