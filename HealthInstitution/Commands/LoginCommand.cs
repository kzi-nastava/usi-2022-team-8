using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.ViewModels.GUIViewModels;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands;

public class LoginCommand : CommandBase
{
    private LoginViewModel _loginViewModel;
    private IUserService _userService;
    private IPatientService _patientService;
    private ITrollCounterService _trollCounterService;
    private IDoctorRatingsService _doctorRatingsService;
    private IDoctorService _doctorService;
    IPrescriptionNotificationService _prescriptionNotificationService;
    public LoginCommand(LoginViewModel loginViewModel, IUserService userService, ITrollCounterService trollCounterService, IPatientService patientService, IDoctorService doctorService, IPrescriptionNotificationService prescriptionNotificationService, IDoctorRatingsService doctorRatingsService)
    {
        _loginViewModel = loginViewModel;
        _userService = userService;
        _trollCounterService = trollCounterService;
        _patientService = patientService;
        _doctorService = doctorService;
        _prescriptionNotificationService = prescriptionNotificationService;
        _doctorRatingsService = doctorRatingsService;
    }

    public String ToPlainString(System.Security.SecureString secureStr)
    {
        String plainStr = new System.Net.NetworkCredential(string.Empty, secureStr).Password;
        return plainStr;
    }

    public override void Execute(object? parameter)
    {
        User user = GetUserFromInputData();
        if (_userService.IsUserFound(user, ToPlainString(_loginViewModel.Password)) && !_userService.IsUserBlocked(user))
        {
            switch (user.Type)
            {
                case UserType.Patient:
                    RedirectPatient(user);

                    break;

                case UserType.Doctor:
                    RedirectDoctor();
                    break;

                case UserType.Secretary:
                    RedirectSecretary();

                    break;

                case UserType.Manager:
                    RedirectManager();
                    break;
            }
        }
    }

    private User GetUserFromInputData()
    {
        return _userService.GetByUsername(_loginViewModel.Username);
    }

    private void RedirectPatient(User foundUser)
    {
        _trollCounterService.TrollCheck(foundUser.Username);
        Patient loggedPatient = _patientService.GetByUsername(_loginViewModel.Username);
        _prescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
        _doctorRatingsService.AssignScores();

        var window = DIContainer.GetService<PatientWindow>();
        window.SetLoggedPatient(loggedPatient);
        _loginViewModel.ThisWindow.Close();
        window.ShowDialog();
    }

    private void RedirectDoctor()
    {
        _doctorService.LoadAppointments();
        Doctor loggedDoctor = _doctorService.GetById(_loginViewModel.Username);

        _loginViewModel.ThisWindow.Close();
        var window = DIContainer.GetService<DoctorWindow>();
        window.SetLoggedDoctor(loggedDoctor);
        window.ShowDialog();
    }

    private void RedirectSecretary()
    {
        _loginViewModel.ThisWindow.Close();

        SecretaryWindow secretaryWindow = DIContainer.GetService<SecretaryWindow>();
        secretaryWindow.ShowDialog();
    }

    private void RedirectManager()
    {
        _loginViewModel.ThisWindow.Close();

        ManagerWindow managerWindow = DIContainer.GetService<ManagerWindow>();
        managerWindow.ShowDialog();
    }
}