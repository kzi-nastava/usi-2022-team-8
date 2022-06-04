using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Scheduling;

namespace HealthInstitution.GUI.PatientWindows;

/// <summary>
/// Interaction logic for EditExaminationDialog.xaml
/// </summary>
///

public partial class EditExaminationDialog : Window
{
    private int _minutes;
    private int _hours;
    private User _loggedPatient;
    private string _doctorUsername;
    private Examination _selectedExamination;

    public EditExaminationDialog(Examination selectedExamination)
    {
        _selectedExamination = selectedExamination;
        _loggedPatient = selectedExamination.MedicalRecord.Patient;
        InitializeComponent();
    }

    private void GenerateRequest(DateTime dateTime)
    {
        ExaminationDTO examinationDTO = ExaminationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = DoctorService.GetById(_doctorUsername);
        examinationDTO.Appointment = dateTime;
        Examination newExamination = EditSchedulingService.GenerateRequestExamination(_selectedExamination.Id, examinationDTO);
        ScheduleEditRequestService.AddEditRequest(newExamination);
    }

    private void EditNow(DateTime dateTime)
    {
        ExaminationDTO examinationDTO = ExaminationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = DoctorService.GetById(_doctorUsername);
        examinationDTO.Appointment = dateTime;
        EditSchedulingService.EditExamination(_selectedExamination.Id, examinationDTO);
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        string formatDate = datePicker.SelectedDate.ToString();

        DateTime.TryParse(formatDate, out var dateTime);
        dateTime = dateTime.AddHours(_hours);
        dateTime = dateTime.AddMinutes(_minutes);
        try
        {
            if (_selectedExamination.Appointment.AddDays(-2) < DateTime.Today)
            {
                GenerateRequest(dateTime);
            }
            else
            {
                EditNow(dateTime);
            }

            this.Close();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var doctorComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> doctors = new List<String>();

        foreach (User user in DoctorService.GetAll())
        {
            doctors.Add(user.Username);
        }

        doctorComboBox.ItemsSource = doctors;
        doctorComboBox.SelectedItem = _selectedExamination.Doctor.Username;
        doctorComboBox.Items.Refresh();
    }

    private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> hours = new List<String>();
        for (int i = 9; i < 22; i++)
        {
            hours.Add(i.ToString());
        }
        hourComboBox.ItemsSource = hours;
        hourComboBox.SelectedIndex = _selectedExamination.Appointment.Hour - 9;
    }

    private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> minutes = new List<String>();
        minutes.Add("00");
        minutes.Add("15");
        minutes.Add("30");
        minutes.Add("45");
        minuteComboBox.ItemsSource = minutes;

        for (int i = 0; i < 4; i++)
        {
            if (_selectedExamination.Appointment.Minute == i * 15) minuteComboBox.SelectedIndex = i;
        }
        minuteComboBox.SelectedIndex = _selectedExamination.Appointment.Minute / 15;
    }

    private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var doctorComboBox = sender as System.Windows.Controls.ComboBox;
        this._doctorUsername = doctorComboBox.SelectedValue as string;
    }

    private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        int h = hourComboBox.SelectedIndex;
        this._hours = h + 9;
    }

    private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        int m = minuteComboBox.SelectedIndex;
        this._minutes = m * 15;
    }
}