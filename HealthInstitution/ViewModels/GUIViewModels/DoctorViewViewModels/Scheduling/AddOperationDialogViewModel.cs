using HealthInstitution.Commands.DoctorCommands.SchedulingDialogs;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling
{
    public class AddOperationDialogViewModel : ViewModelBase
    {
        public Doctor LoggedDoctor { get; }
        public DateTime GetOperationDateTime()
        {
            string formatDate = SelectedDateTime.Date.ToString();
            formatDate = formatDate;
            int minutes = _minuteComboBoxSelectedIndex * 15;
            int hours = _hourComboBoxSelectedIndex + 9;
            DateTime.TryParse(formatDate, out var dateTime);
            dateTime = dateTime.AddHours(hours);
            dateTime = dateTime.AddMinutes(minutes);
            return dateTime;
        }

        private string _duration;

        public string Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public int GetDuration()
        {
            return Int32.Parse(Duration);
        }

        private DateTime _selectedDateTime = DateTime.Now;

        public DateTime SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }

        private ObservableCollection<string> _hourComboBoxItems;

        public ObservableCollection<string> HourComboBoxItems
        {
            get
            {
                return _hourComboBoxItems;
            }
            set
            {
                _hourComboBoxItems = value;
                OnPropertyChanged(nameof(HourComboBoxItems));
            }
        }

        private ObservableCollection<string> _minuteComboBoxItems;

        public ObservableCollection<string> MinuteComboBoxItems
        {
            get
            {
                return _minuteComboBoxItems;
            }
            set
            {
                _minuteComboBoxItems = value;
                OnPropertyChanged(nameof(MinuteComboBoxItems));
            }
        }

        private ObservableCollection<Patient> _patientComboBoxItems;

        public ObservableCollection<Patient> PatientComboBoxItems
        {
            get
            {
                return _patientComboBoxItems;
            }
            set
            {
                _patientComboBoxItems = value;
                OnPropertyChanged(nameof(PatientComboBoxItems));
            }
        }
        private int _patientCombBoxSelectedIndex;

        public int PatientComboBoxSelectedIndex
        {
            get
            {
                return _patientCombBoxSelectedIndex;
            }
            set
            {
                _patientCombBoxSelectedIndex = value;
                OnPropertyChanged(nameof(PatientComboBoxSelectedIndex));
            }
        }

        private int _hourComboBoxSelectedIndex;

        public int HourComboBoxSelectedIndex
        {
            get
            {
                return _hourComboBoxSelectedIndex;
            }
            set
            {
                _hourComboBoxSelectedIndex = value;
                OnPropertyChanged(nameof(HourComboBoxSelectedIndex));
            }
        }

        private int _minuteComboBoxSelectedIndex;

        public int MinuteComboBoxSelectedIndex
        {
            get
            {
                return _minuteComboBoxSelectedIndex;
            }
            set
            {
                _minuteComboBoxSelectedIndex = value;
                OnPropertyChanged(nameof(MinuteComboBoxSelectedIndex));
            }
        }

        public Patient GetPatient()
        {
            return PatientComboBoxItems[PatientComboBoxSelectedIndex];
        }

        private void LoadHourComboBox()
        {
            HourComboBoxItems = new();
            for (int i = 9; i < 22; i++)
            {
                HourComboBoxItems.Add(i.ToString());
            }
        }

        private void LoadMinuteComboBox()
        {
            MinuteComboBoxItems = new();
            for (int i = 0; i <= 45; i += 15)
            {
                MinuteComboBoxItems.Add(i.ToString());
            }
        }

        private void LoadPatientComboBox()
        {
            PatientComboBoxItems = new();
            foreach (Patient patient in _patientService.GetAll())
            {
                PatientComboBoxItems.Add(patient);
            }
        }

        private void LoadComboBoxes()
        {
            LoadPatientComboBox();
            LoadHourComboBox();
            LoadMinuteComboBox();
        }

        public ICommand CreateOperationCommand { get; }
        IPatientService _patientService;
        IMedicalRecordService _medicalRecordService;
        ISchedulingService schedullingService;
        public AddOperationDialogViewModel(Doctor loggedDoctor, IPatientService patientService, IMedicalRecordService medicalRecordService, ISchedulingService schedulingService)
        {
            LoggedDoctor = loggedDoctor;
            LoadComboBoxes();
            CreateOperationCommand = new AddOperationDialogCommand(this, medicalRecordService, schedulingService);
        }
    }
}
