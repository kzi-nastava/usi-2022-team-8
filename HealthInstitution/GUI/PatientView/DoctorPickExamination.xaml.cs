using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System.Windows;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for DoctorPickExamination.xaml
/// </summary>
public partial class DoctorPickExamination : Window
{
    private Patient _loggedPatient;
    private List<Doctor> _currentDoctors;
    IDoctorService _doctorService;

    public DoctorPickExamination(Patient loggedPatinent, IDoctorService doctorService)
    {
        InitializeComponent();
        _loggedPatient = loggedPatinent;
        dataGrid.SelectedIndex = 0;
        _doctorService = doctorService;
        _currentDoctors = _doctorService.GetAll();
        LoadRows();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var doctor = dataGrid.SelectedItem as Doctor;
        var dialog = new AddExaminationDialog(_loggedPatient, DIContainer.GetService<IDoctorService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<ISchedulingService>());
        dialog.Show();
        dialog.SetSelectedDoctor(doctor);
    }

    private void NameSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _doctorService.OrderByDoctorName(_currentDoctors);
        LoadRows();
    }

    private void LoadRows()
    {
        dataGrid.Items.Clear();
        List<Doctor> doctors = _currentDoctors;
        foreach (Doctor doctor in doctors)
        {
            dataGrid.Items.Add(doctor);
        }
    }

    private void SurnameSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _doctorService.OrderByDoctorSurname(_currentDoctors);
        LoadRows();
    }

    private void SpecialitySort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _doctorService.OrderByDoctorSpeciality(_currentDoctors);
        LoadRows();
    }

    private void RatingSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _doctorService.OrderByDoctorRating(_currentDoctors);
        LoadRows();
    }

    private void SpecialitySearch_Click(object sender, RoutedEventArgs e)
    {
        string speciality = SearchBox.Text;
        _currentDoctors = _doctorService.SearchBySpeciality(speciality);
        LoadRows();
    }

    private void SurnameSearch_Click(object sender, RoutedEventArgs e)
    {
        string surname = SearchBox.Text;
        _currentDoctors = _doctorService.SearchBySurname(surname);
        LoadRows();
    }

    private void SearchParameter_GotFocus(object sender, RoutedEventArgs e)
    {
        SearchBox.Clear();
    }

    private void NameSearch_Click(object sender, RoutedEventArgs e)
    {
        string name = SearchBox.Text;
        _currentDoctors = _doctorService.SearchByName(name);
        LoadRows();
    }
}