using HealthInstitution.Commands.PatientCommands.Scheduling;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.Scheduling;

public class EditExaminationDialogViewModel : ViewModelBase
{
    public Examination SelectedExamination;

    public DateTime GetExaminationDateTime()
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

    private ObservableCollection<string> _doctorComboBoxItems;

    public ObservableCollection<string> DoctorComboBoxItems
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

    public string GetDoctorUsername()
    {
        return DoctorComboBoxItems[DoctorComboBoxSelectedIndex];
    }

    private void LoadHourComboBox()
    {
        HourComboBoxItems = new();
        for (int i = 9; i < 22; i++)
        {
            HourComboBoxItems.Add(i.ToString());
        }
        HourComboBoxSelectedIndex = SelectedExamination.Appointment.Hour - 9;
    }

    private void LoadMinuteComboBox()
    {
        MinuteComboBoxItems = new();
        for (int i = 0; i <= 45; i += 15)
        {
            MinuteComboBoxItems.Add(i.ToString());
        }
        MinuteComboBoxSelectedIndex = SelectedExamination.Appointment.Minute / 15;
    }

    private void LoadDoctorComboBox()
    {
        int i = 0;
        int idx = 0;
        DoctorComboBoxItems = new();
        foreach (Doctor user in DoctorService.GetAll())
        {
            DoctorComboBoxItems.Add(user.Username);
            if (user.Username == SelectedExamination.Doctor.Username)
            {
                idx = i;
            }
            i++;
        }
        DoctorComboBoxSelectedIndex = idx;
    }

    private void LoadComboBoxes()
    {
        LoadDoctorComboBox();
        LoadHourComboBox();
        LoadMinuteComboBox();
    }

    public ICommand EditExaminationCommand { get; }

    public EditExaminationDialogViewModel(Examination selectedExamination)
    {
        SelectedExamination = selectedExamination;
        LoadComboBoxes();
        _selectedDateTime = selectedExamination.Appointment;
        EditExaminationCommand = new EditExaminationCommand(this, SelectedExamination);
    }
}