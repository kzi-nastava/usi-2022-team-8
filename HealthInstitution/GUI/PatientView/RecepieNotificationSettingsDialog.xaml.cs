using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Patients;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationSettings.xaml
/// </summary>
public partial class RecepieNotificationSettingsDialog : Window
{
    private int _hours;
    private int _minutes;
    private string _loggedPatinet;
    private List<Prescription> _prescriptions;

    public RecepieNotificationSettingsDialog(string loggedPatient)
    {
        InitializeComponent();
        _loggedPatinet = loggedPatient;
        _prescriptions = MedicalRecordService.GetByPatientUsername(PatientService.GetByUsername(loggedPatient)).Prescriptions;
        LoadRows();
    }

    private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> hours = new List<String>();
        for (int i = 0; i < 23; i++)
        {
            hours.Add(i.ToString());
        }
        hourComboBox.ItemsSource = hours;
        hourComboBox.SelectedIndex = 0;
    }

    private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> minutes = new List<String>();
        for (int i = 0; i < 59; i++)
        {
            minutes.Add(i.ToString());
        }
        minuteComboBox.ItemsSource = minutes;
        minuteComboBox.SelectedIndex = 0;
    }

    private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        int h = hourComboBox.SelectedIndex;
        _hours = h;
    }

    private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        int m = minuteComboBox.SelectedIndex;
        this._minutes = m;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Prescription prescription = _prescriptions[dataGrid.SelectedIndex];
        DateTime before = DateTime.Today;
        before = before.AddMinutes(_minutes).AddHours(_hours);
        PrescriptionNotificationSettings recepieNotificationSettings = new PrescriptionNotificationSettings(before, _loggedPatinet, prescription, DateTime.Now, prescription.Id);
        PrescriptionNotificationService.UpdateSettings(recepieNotificationSettings.Id, recepieNotificationSettings);
        List<DateTime> dateTimes = PrescriptionNotificationService.GenerateDateTimes(recepieNotificationSettings);
        PrescriptionNotificationService.GenerateCronJobs(dateTimes, recepieNotificationSettings, _loggedPatinet);
    }

    private void LoadRows()
    {
        dataGrid.Items.Clear();

        foreach (var prescription in _prescriptions)
        {
            dataGrid.Items.Add(prescription);
        }
        dataGrid.SelectedIndex = 0;
    }
}