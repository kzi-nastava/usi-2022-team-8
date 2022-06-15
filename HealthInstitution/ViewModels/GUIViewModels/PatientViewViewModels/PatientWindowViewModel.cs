using HealthInstitution.Commands;
using HealthInstitution.Commands.PatientCommands.PatientWindowCommands;
using HealthInstitution.Core;
using HealthInstitution.Core.MVVMNavigation;
using HealthInstitution.Core.SystemUsers.Patients.Model;
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
    public PatientWindowViewModel(Patient loggedPatient, Window thisWindow)
    {
        RateHospital = new RateHospitalCommand();
        MedicalRecordView = new MedicalRecordViewCommand(loggedPatient);
        ManuallySchedule = new ManuallScheduleCommand(loggedPatient);
        RecommendedSchedule = new RecommendedScheduleCommand(loggedPatient);
        PickDoctor = new PickDoctorCommand(loggedPatient);
        PrescriptionNotificationSettings = new PrescriptionNotificationSettingsCommand(loggedPatient.Username);
        Logout = new LogoutCommand(thisWindow);
        PatientNotificationCommand = new PatientNotificationCommand(loggedPatient);
    }

    public ICommand RateHospital { get; }
    public ICommand MedicalRecordView { get; }
    public ICommand ManuallySchedule { get; }
    public ICommand RecommendedSchedule { get; }
    public ICommand PickDoctor { get; }
    public ICommand PrescriptionNotificationSettings { get; }
    public ICommand Logout { get; }
    public ICommand PatientNotificationCommand { get; }
}