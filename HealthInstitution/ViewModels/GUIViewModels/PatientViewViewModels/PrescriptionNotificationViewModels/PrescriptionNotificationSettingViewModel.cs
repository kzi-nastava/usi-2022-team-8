using HealthInstitution.Commands.PatientCommands;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;

public class PrescriptionNotificationSettingViewModel : ViewModelBase
{
    
    IMedicalRecordService _medicalRecordService;
    IPatientService _patientService;
    IPrescriptionNotificationService _prescriptionNotificationService;
    public ICommand SetNotificationTimeCommand { get; }
    public Patient LoggedPatient;
    public PrescriptionNotificationSettingViewModel(Patient loggedPatient, IMedicalRecordService medicalRecordService,
        IPatientService patientService, IPrescriptionNotificationService prescriptionNotificationService)
    {
        LoggedPatient = loggedPatient;
        _patientService = patientService;
        _medicalRecordService = medicalRecordService;
        _prescriptionNotificationService = prescriptionNotificationService;
        PrescritpionVMs = new();
        _hourComboBoxItems = new();
        _minuteComboBoxItems = new();
        _prescriptions = new();
        LoadComboBoxes();
        GridRefresh();
        SetNotificationTimeCommand = new SetPrescriptionNotificationTimeCommand(this,_prescriptionNotificationService);
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

    private int _selectedPrescriptionIndex;

    public int SelectedPrescritpionIndex
    {
        get
        {
            return _selectedPrescriptionIndex;
        }
        set
        {
            _selectedPrescriptionIndex = value;
            OnPropertyChanged(nameof(SelectedPrescritpionIndex));
        }
    }

    private List<Prescription> _prescriptions;

    private ObservableCollection<PrescriptionViewModel> _prescriptionVMs;

    public ObservableCollection<PrescriptionViewModel> PrescritpionVMs
    {
        get
        {
            return _prescriptionVMs;
        }
        set
        {
            _prescriptionVMs = value;
            OnPropertyChanged(nameof(PrescritpionVMs));
        }
    }

    private List<string> _hourComboBoxItems;

    public List<string> HourComboBoxItems
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

    private List<string> _minuteComboBoxItems;

    public List<string> MinuteComboBoxItems
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

    private void MinuteComboBoxLoad()
    {
        for (int i = 0; i < 59; i++)
        {
            _minuteComboBoxItems.Add(i.ToString());
        }
    }

    private void HourComboBoxLoad()
    {
        for (int i = 0; i < 23; i++)
        {
            _hourComboBoxItems.Add(i.ToString());
        }
    }

    private void LoadComboBoxes()
    {
        HourComboBoxLoad();
        MinuteComboBoxLoad();
    }

    

    private void GridRefresh()
    {
        _prescriptions.Clear();
        _prescriptionVMs.Clear();
        _prescriptions = _medicalRecordService.GetByPatientUsername(_patientService.GetByUsername(LoggedPatient.Username)).Prescriptions;

        foreach (var prescription in _prescriptions)
        {
            _prescriptionVMs.Add(new PrescriptionViewModel(prescription));
        }
    }

    

    public Prescription GetSelectedPrescription()
    {
        return _prescriptions[SelectedPrescritpionIndex];
    }

    public DateTime GetbeforeTime()
    {
        return DateTime.Today.AddHours(HourComboBoxSelectedIndex).AddMinutes(MinuteComboBoxSelectedIndex);
    }
}