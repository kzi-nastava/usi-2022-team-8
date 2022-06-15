using HealthInstitution.Commands.PatientCommands.PatientWindowCommands;
using HealthInstitution.Commands.PatientCommands.RecommendedSchedulingCommands;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;

public class RecommendedWindowViewModel : ViewModelBase
{
    private string _priority;

    public string Priority
    {
        get
        {
            return _priority;
        }
        set
        {
            _priority = value;
            OnPropertyChanged(nameof(Priority));
        }
    }

    public DateTime GetStartDateTime()
    {
        int minutes = _minuteStartComboBoxSelectedIndex * 15;
        int hours = _hourStartComboBoxSelectedIndex + 9;
        DateTime dateTime = DateTime.Today;
        dateTime = dateTime.AddHours(hours);
        dateTime = dateTime.AddMinutes(minutes);
        dateTime = dateTime.AddDays(1);
        return dateTime;
    }

    public DateTime GetEndDateTime()
    {
        string formatDate = SelectedDateTime.Date.ToString();
        formatDate = formatDate;
        int minutes = _minuteStartComboBoxSelectedIndex * 15;
        int hours = _hourStartComboBoxSelectedIndex + 9;
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

    private int _hourStartComboBoxSelectedIndex;

    public int HourStartComboBoxSelectedIndex
    {
        get
        {
            return _hourStartComboBoxSelectedIndex;
        }
        set
        {
            _hourStartComboBoxSelectedIndex = value;
            OnPropertyChanged(nameof(HourStartComboBoxSelectedIndex));
        }
    }

    private int _minuteStartComboBoxSelectedIndex;

    public int MinuteStartComboBoxSelectedIndex
    {
        get
        {
            return _minuteStartComboBoxSelectedIndex;
        }
        set
        {
            _minuteStartComboBoxSelectedIndex = value;
            OnPropertyChanged(nameof(MinuteStartComboBoxSelectedIndex));
        }
    }

    private int _hourEndComboBoxSelectedIndex;

    public int HourEndComboBoxSelectedIndex
    {
        get
        {
            return _hourEndComboBoxSelectedIndex;
        }
        set
        {
            _hourEndComboBoxSelectedIndex = value;
            OnPropertyChanged(nameof(HourEndComboBoxSelectedIndex));
        }
    }

    private int _minuteEndComboBoxSelectedIndex;

    public int MinuteEndComboBoxSelectedIndex
    {
        get
        {
            return _minuteEndComboBoxSelectedIndex;
        }
        set
        {
            _minuteEndComboBoxSelectedIndex = value;
            OnPropertyChanged(nameof(MinuteEndComboBoxSelectedIndex));
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
    }

    private void LoadMinuteComboBox()
    {
        MinuteComboBoxItems = new();
        for (int i = 0; i <= 45; i += 15)
        {
            MinuteComboBoxItems.Add(i.ToString());
        }
    }

    private void LoadDoctorComboBox()
    {
        DoctorComboBoxItems = new();
        foreach (Doctor user in DoctorService.GetAll())
        {
            DoctorComboBoxItems.Add(user.Username);
        }
    }

    private void LoadComboBoxes()
    {
        LoadDoctorComboBox();
        LoadHourComboBox();
        LoadMinuteComboBox();
    }

    public ICommand ScheduleCommand { get; }
    public User LoggedPatient;

    public RecommendedWindowViewModel(User loggedPatient)
    {
        LoggedPatient = loggedPatient;

        _doctorComboBoxItems = new();
        _hourComboBoxItems = new();
        _minuteComboBoxItems = new();
        ScheduleCommand = new FirstFitScheduleCommand(this);
        LoadComboBoxes();
    }
}