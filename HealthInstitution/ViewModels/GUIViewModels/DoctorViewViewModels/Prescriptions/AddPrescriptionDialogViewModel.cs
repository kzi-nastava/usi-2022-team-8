using HealthInstitution.Commands.DoctorCommands.Prescriptions;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Prescriptions
{
    public class AddPrescriptionDialogViewModel : ViewModelBase
    {
        public MedicalRecord MedicalRecord { get; }
        public DateTime GetHourlyRate()
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

        private ObservableCollection<Drug> _drugComboBoxItems;

        public ObservableCollection<Drug> DrugComboBoxItems
        {
            get
            {
                return _drugComboBoxItems;
            }
            set
            {
                _drugComboBoxItems = value;
                OnPropertyChanged(nameof(DrugComboBoxItems));
            }
        }
        private int _drugCombBoxSelectedIndex;

        public int DrugComboBoxSelectedIndex
        {
            get
            {
                return _drugCombBoxSelectedIndex;
            }
            set
            {
                _drugCombBoxSelectedIndex = value;
                OnPropertyChanged(nameof(DrugComboBoxSelectedIndex));
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


        private ObservableCollection<string> _timeOfUseComboBoxItems;

        public ObservableCollection<string> TimeOfUseComboBoxItems
        {
            get
            {
                return _timeOfUseComboBoxItems;
            }
            set
            {
                _timeOfUseComboBoxItems = value;
                OnPropertyChanged(nameof(TimeOfUseComboBoxItems));
            }
        }

        private int _timeOfUseComboBoxSelectedIndex;

        public int TimeOfUseComboBoxSelectedIndex
        {
            get
            {
                return _timeOfUseComboBoxSelectedIndex;
            }
            set
            {
                _timeOfUseComboBoxSelectedIndex = value;
                OnPropertyChanged(nameof(TimeOfUseComboBoxSelectedIndex));
            }
        }

        private string dailyDose;

        public string DailyDose
        {
            get
            {
                return dailyDose;
            }
            set
            {
                dailyDose = value;
                OnPropertyChanged(nameof(DailyDose));
            }
        }

        public int GetDailyDose()
        {
            return Int32.Parse(DailyDose);
        }

        public Drug GetDrug()
        {
            return DrugComboBoxItems[DrugComboBoxSelectedIndex];
        }

        public PrescriptionTime GetTimeOfUse()
        {
            return (PrescriptionTime)TimeOfUseComboBoxSelectedIndex;
        }

        private void LoadHourComboBox()
        {
            HourComboBoxItems = new();
            for (int i = 9; i <= 12; i++)
            {
                HourComboBoxItems.Add(i.ToString());
            }
            HourComboBoxSelectedIndex = 0;
        }

        private void LoadMinuteComboBox()
        {
            MinuteComboBoxItems = new();
            for (int i = 0; i <= 45; i += 15)
            {
                MinuteComboBoxItems.Add(i.ToString());
            }
            MinuteComboBoxSelectedIndex = 0;
        }
        private void LoadTimeOfUseComboBox()
        {
            TimeOfUseComboBoxItems = new();
            TimeOfUseComboBoxItems.Add("Not important");
            TimeOfUseComboBoxItems.Add("Pre meal");
            TimeOfUseComboBoxItems.Add("During meal");
            TimeOfUseComboBoxItems.Add("Post meal");
            TimeOfUseComboBoxSelectedIndex = 0;
        }

        private void LoadDrugComboBox()
        {
            DrugComboBoxItems = new();
            foreach (Drug drug in _drugService.GetAll())
            {
                DrugComboBoxItems.Add(drug);
            }
            DrugComboBoxSelectedIndex = 0;
        }

        private void LoadComboBoxes()
        {
            LoadDrugComboBox();
            LoadTimeOfUseComboBox();
            LoadHourComboBox();
            LoadMinuteComboBox();
        }

        public ICommand CreatePrescriptionCommand { get; }
        IDrugService _drugService;
        IMedicalRecordService _medicalRecordService;
        IPrescriptionService _prescriptionService;
        public AddPrescriptionDialogViewModel(MedicalRecord medicalRecord, IDrugService drugService, IMedicalRecordService medicalRecordService, IPrescriptionService prescriptionService)
        {
            MedicalRecord = medicalRecord;
            LoadComboBoxes();
            _drugService = drugService;
            _medicalRecordService = medicalRecordService;
            _prescriptionService = prescriptionService;
            CreatePrescriptionCommand = new AddPrescriptionDialogCommand(this, prescriptionService, medicalRecordService);
        }
    }
}