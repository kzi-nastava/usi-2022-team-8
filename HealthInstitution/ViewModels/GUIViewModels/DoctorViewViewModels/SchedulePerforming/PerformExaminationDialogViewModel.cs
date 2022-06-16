using HealthInstitution.Commands.DoctorCommands.ExaminationPerforming;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming
{
    internal class PerformExaminationDialogViewModel : ViewModelBase
    {
        public Doctor LoggedDoctor { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        private string _patient;
        public string Patient
        {
            get
            {
                return _patient;
            }
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }
        private string _height;
        public string Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        private string _weight;
        public string Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        private List<string> _allergens;
        public List<string> Allergens
        {
            get
            {
                return _allergens;
            }
            set
            {
                _allergens = value;
                OnPropertyChanged(nameof(Allergens));
            }
        }
        private List<string> _previousIllnesses;
        public List<string> PreviousIllnesses
        {
            get
            {
                return _previousIllnesses;
            }
            set
            {
                _previousIllnesses = value;
                OnPropertyChanged(nameof(PreviousIllnesses));
            }
        }

        private string _illness;
        public string Illness
        {
            get
            {
                return _illness;
            }
            set
            {
                _illness = value;
                OnPropertyChanged(nameof(Illness));
            }
        }

        private string _allergen;
        public string Allergen
        {
            get
            {
                return _allergen;
            }
            set
            {
                _allergen = value;
                OnPropertyChanged(nameof(Allergen));
            }
        }

        private string _anamnesis;
        public string Anamnesis
        {
            get
            {
                return _anamnesis;
            }
            set
            {
                _anamnesis = value;
                OnPropertyChanged(nameof(Anamnesis));
            }
        }

        public ICommand ShowCreatePrescriptionDialogCommand { get; }
        public ICommand ShowCreateReferralDialogCommand { get; }
        public ICommand FinishExaminationCommand { get; }
        public ICommand AddAllergenCommand { get; }
        public ICommand AddIllnessCommand { get; }

        public PerformExaminationDialogViewModel(Examination examination, MedicalRecord medicalRecord)
        {
            LoggedDoctor = examination.Doctor;
            MedicalRecord = medicalRecord;
            ShowCreatePrescriptionDialogCommand = new ShowCreatePrescriptionDialogCommand(medicalRecord);
            ShowCreateReferralDialogCommand = new ShowCreateReferralDialogCommand(LoggedDoctor, medicalRecord);
            AddAllergenCommand = new AddAllergenCommand(this);
            AddIllnessCommand = new AddIllnessCommand(this);
            FinishExaminationCommand = new FinishExaminationCommand(this, examination);
            LoadData();
        }

        public void LoadData() {
            Patient = MedicalRecord.Patient.Name + " " + MedicalRecord.Patient.Surname;
            Height = MedicalRecord.Height.ToString();
            Weight = MedicalRecord.Weight.ToString();
            PreviousIllnesses = MedicalRecord.PreviousIllnesses;
            Allergens = MedicalRecord.Allergens;
        }
    }
}
