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

    public DoctorPickExamination(Patient loggedPatinent)
    {
        InitializeComponent();
        _loggedPatient = loggedPatinent;
        dataGrid.SelectedIndex = 0;
        _currentDoctors = DoctorService.GetAll();
        LoadRows();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var doctor = dataGrid.SelectedItem as Doctor;
        var dialog = new AddExaminationDialog(_loggedPatient);
        dialog.Show();
        dialog.SetSelectedDoctor(doctor);
    }

    private void NameSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = DoctorService.OrderByDoctorName(_currentDoctors);
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
        _currentDoctors = DoctorService.OrderByDoctorSurname(_currentDoctors);
        LoadRows();
    }

    private void SpecialitySort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = DoctorService.OrderByDoctorSpeciality(_currentDoctors);
        LoadRows();
    }

    private void RatingSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = DoctorService.OrderByDoctorSurname(_currentDoctors);
        LoadRows();
    }

    private void SpecialitySearch_Click(object sender, RoutedEventArgs e)
    {
        string speciality = SearchBox.Text;
        _currentDoctors = DoctorService.SearchBySpeciality(speciality);
        LoadRows();
    }

    private void SurnameSearch_Click(object sender, RoutedEventArgs e)
    {
        string surname = SearchBox.Text;
        _currentDoctors = DoctorService.SearchBySurname(surname);
        LoadRows();
    }

    private void NameSearch_Click(object sender, RoutedEventArgs e)
    {
        string name = SearchBox.Text;
        _currentDoctors = DoctorService.SearchByName(name);
        LoadRows();
    }

    private void seachParameter_GotFocus(object sender, RoutedEventArgs e)
    {
        SearchBox.Clear();
    }
}