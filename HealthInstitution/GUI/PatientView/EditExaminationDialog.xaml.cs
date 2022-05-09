using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;
using System.Windows.Controls;

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

    private void save_Click(object sender, RoutedEventArgs e)
    {
        string formatDate = datePicker.SelectedDate.ToString();
        //formatDate = formatDate;

        DateTime.TryParse(formatDate, out var dateTime);
        dateTime = dateTime.AddHours(_hours);
        dateTime = dateTime.AddMinutes(_minutes);
        try
        {
            if (_selectedExamination.Appointment.AddDays(-2) < DateTime.Today)
            {
                Examination newExamination = ExaminationRepository.GetInstance().GenerateRequestExamination(_selectedExamination, _loggedPatient.Username, _doctorUsername, dateTime);
                ScheduleEditRequestFileRepository.GetInstance().AddEditRequest(newExamination);
            }
            else
            {
                ExaminationRepository.GetInstance().EditExamination(_selectedExamination, _loggedPatient.Username, _doctorUsername, dateTime);
                ExaminationDoctorRepository.GetInstance().Save();
                ExaminationRepository.GetInstance().Save();
            }

            this.Close();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void doctorComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var doctorComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> doctors = new List<String>();

        foreach (User user in UserRepository.GetInstance().GetAll())
        {
            if (user.Type == UserType.Doctor)
                doctors.Add(user.Username);
        }

        doctorComboBox.ItemsSource = doctors;
        doctorComboBox.SelectedItem = _selectedExamination.Doctor.Username;
        doctorComboBox.Items.Refresh();
    }

    private void hourComboBox_Loaded(object sender, RoutedEventArgs e)
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

    private void minuteComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> minutes = new List<String>();
        minutes.Add("00");
        minutes.Add("15");
        minutes.Add("30");
        minutes.Add("45");
        minuteComboBox.ItemsSource = minutes;

        if (_selectedExamination.Appointment.Minute == 0) minuteComboBox.SelectedIndex = 0;
        if (_selectedExamination.Appointment.Minute == 15) minuteComboBox.SelectedIndex = 1;
        if (_selectedExamination.Appointment.Minute == 30) minuteComboBox.SelectedIndex = 2;
        if (_selectedExamination.Appointment.Minute == 45) minuteComboBox.SelectedIndex = 3;
        minuteComboBox.SelectedIndex = _selectedExamination.Appointment.Minute / 15;
    }

    private void doctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var doctorComboBox = sender as System.Windows.Controls.ComboBox;
        this._doctorUsername = doctorComboBox.SelectedValue as string;
    }

    private void hourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        int h = hourComboBox.SelectedIndex;
        this._hours = h + 9;
    }

    private void minuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        int m = minuteComboBox.SelectedIndex;
        this._minutes = m * 15;
    }
}