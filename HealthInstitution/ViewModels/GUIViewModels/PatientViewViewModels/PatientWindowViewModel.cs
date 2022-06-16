using HealthInstitution.Commands;
using HealthInstitution.Commands.PatientCommands.PatientWindowCommands;
using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MVVMNavigation;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.PatientView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

public class PatientWindowViewModel : ViewModelBase
{
    IPatientService _patientService;
    Patient _loggedPatient;
    public PatientWindowViewModel(Patient loggedPatient, Window thisWindow, IPatientService patientService)
    {   
        _loggedPatient = loggedPatient;
        _patientService = patientService;
        ShowNotificationsDialog();
        RecepieNotificationDialog recepieNotificationDialog = DIContainer.GetService<RecepieNotificationDialog>();
        recepieNotificationDialog.SetLoggedPatient(loggedPatient);
        recepieNotificationDialog.ShowDialog();
        RateHospital = new RateHospitalCommand();
        MedicalRecordView = new MedicalRecordViewCommand(loggedPatient);
        ManuallySchedule = new ManuallScheduleCommand(loggedPatient);
        RecommendedSchedule = new RecommendedScheduleCommand(loggedPatient);
        PickDoctor = new PickDoctorCommand(loggedPatient);
        PrescriptionNotificationSettings = new PrescriptionNotificationSettingsCommand(loggedPatient.Username);
        Logout = new LogoutCommand(thisWindow);
        //PatientNotificationCommand = new PatientNotificationCommand(loggedPatient);
    }

    public ICommand RateHospital { get; }
    public ICommand MedicalRecordView { get; }
    public ICommand ManuallySchedule { get; }
    public ICommand RecommendedSchedule { get; }
    public ICommand PickDoctor { get; }
    public ICommand PrescriptionNotificationSettings { get; }
    public ICommand Logout { get; }
    public ICommand PatientNotificationCommand { get; }
    private void ShowNotificationsDialog()
    {
        if (_patientService.GetActiveAppointmentNotification(_loggedPatient).Count > 0)
        {
            PatientNotificationsDialog patientNotificationsDialog = DIContainer.GetService<PatientNotificationsDialog>();
            patientNotificationsDialog.SetLoggedPatient(_loggedPatient);
            patientNotificationsDialog.ShowDialog();
        }
    }
}