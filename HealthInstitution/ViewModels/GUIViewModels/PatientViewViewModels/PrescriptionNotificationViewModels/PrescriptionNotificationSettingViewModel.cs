using HealthInstitution.Core;
using HealthInstitution.Core.MedicalRecords;
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
    private int _hourComboBoxSelectedIndex;

    public Patient LoggedPatient;

    public int HourComboBoxSelectedintex
    {
        get
        {
            return _hourComboBoxSelectedIndex;
        }
        set
        {
            _hourComboBoxSelectedIndex = value;
            OnPropertyChanged(nameof(HourComboBoxSelectedintex));
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

    private ICommand SetNotificationTime;

    private void GridRefresh()
    {
        _prescriptions.Clear();
        _prescriptionVMs.Clear();
        _prescriptions = MedicalRecordService.GetByPatientUsername(PatientService.GetByUsername(_loggedPatient.Username)).Prescriptions;

        foreach (var prescription in _prescriptions)
        {
            _prescriptionVMs.Add(new PrescriptionViewModel(prescription));
        }
    }

    public PrescriptionNotificationSettingViewModel(Patient loggedPatient)
    {
        _loggedPatient = loggedPatient;
        PrescritpionVMs = new();
        _hourComboBoxItems = new();
        _minuteComboBoxItems = new();
        LoadComboBoxes();
        GridRefresh();
    }
}