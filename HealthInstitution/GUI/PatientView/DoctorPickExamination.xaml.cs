using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

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
        _currentDoctors = DoctorRepository.GetInstance().GetAll();
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
        _currentDoctors = _currentDoctors.OrderBy(o => o.Name).ToList();
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
        _currentDoctors = _currentDoctors.OrderBy(o => o.Surname).ToList();
        LoadRows();
    }

    private void SpecialitySort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _currentDoctors.OrderBy(o => o.Specialty).ToList();
        LoadRows();
    }

    private void RatingSort_Click(object sender, RoutedEventArgs e)
    {
        _currentDoctors = _currentDoctors.OrderBy(o => o.AvgRating).ToList();
        LoadRows();
    }

    private void SpecialitySearch_Click(object sender, RoutedEventArgs e)
    {
        string speciality = SearchBox.Text;
        _currentDoctors = DoctorRepository.GetInstance().SearchDoctorBySpeciality(speciality);
        LoadRows();
    }

    private void SurnameSearch_Click(object sender, RoutedEventArgs e)
    {
        string surname = SearchBox.Text;
        _currentDoctors = DoctorRepository.GetInstance().SearchDoctorBySurname(surname);
        LoadRows();
    }

    private void NameSearch_Click(object sender, RoutedEventArgs e)
    {
        string name = SearchBox.Text;
        _currentDoctors = DoctorRepository.GetInstance().SearchDoctorByName(name);
        LoadRows();
    }

    private void seachParameter_GotFocus(object sender, RoutedEventArgs e)
    {
        SearchBox.Clear();
    }
}