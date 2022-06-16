using HealthInstitution.Commands.PatientCommands.DoctorPickCommands;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

public class DoctorPickViewModel : ViewModelBase
{
    public Patient LoggedPatient;

    private string _searchTerm = "";

    public string SearchTerm
    {
        get
        {
            return _searchTerm;
        }
        set
        {
            _searchTerm = value;
            OnPropertyChanged(nameof(SearchTerm));
        }
    }

    private int _selectedDoctorIndex;

    public int SelectedDoctorIndex
    {
        get
        {
            return _selectedDoctorIndex;
        }
        set
        {
            _selectedDoctorIndex = value;
            OnPropertyChanged(nameof(SelectedDoctorIndex));
        }
    }

    public List<Doctor> Doctors;

    private System.Collections.ObjectModel.ObservableCollection<DoctorViewModel> _doctorVMs;

    public System.Collections.ObjectModel.ObservableCollection<DoctorViewModel> DoctorGridItems
    {
        get
        {
            return _doctorVMs;
        }
        set
        {
            _doctorVMs = value;
            OnPropertyChanged(nameof(DoctorGridItems));
        }
    }

    public void LoadRows()
    {
        DoctorGridItems.Clear();

        foreach (Doctor doctor in Doctors)
            DoctorGridItems.Add(new DoctorViewModel(doctor));
    }

    public ICommand NameSort { get; }
    public ICommand SurnameSort { get; }
    public ICommand SpecialitySort { get; }
    public ICommand RatingSort { get; }
    public ICommand NameSearch { get; }
    public ICommand SurnameSearch { get; }
    public ICommand SpecialitySearch { get; }
    public ICommand ScheduleCommand { get; }

    public DoctorPickViewModel(Patient loggedPatient)
    {
        LoggedPatient = loggedPatient;
        NameSort = new NameSortCommand(this);
        SurnameSort = new SurnameSortCommand(this);
        SpecialitySort = new SpecialitySortCommand(this);
        RatingSort = new RatingSortCommand(this);
        NameSearch = new NameSearchCommand(this);
        ScheduleCommand = new ScheduleCommand(this);
        SurnameSearch = new SurnameSearchCommand(this);
        SpecialitySearch = new SpecialitySearchCommand(this);
        Doctors = DoctorService.GetAll();
        _doctorVMs = new();
        LoadRows();
    }
}