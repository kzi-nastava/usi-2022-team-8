using HealthInstitution.Core;
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
using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.UserWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands;

public class LoginCommand : CommandBase
{
    private LoginViewModel _loginViewModel;

    public LoginCommand(LoginViewModel loginViewModel)
    {
        _loginViewModel = loginViewModel;
    }

    public String ToPlainString(System.Security.SecureString secureStr)
    {
        String plainStr = new System.Net.NetworkCredential(string.Empty, secureStr).Password;
        return plainStr;
    }

    public override void Execute(object? parameter)
    {
        User user = GetUserFromInputData();
        if (UserService.IsUserFound(user, ToPlainString(_loginViewModel.Password)) && !UserService.IsUserBlocked(user))
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
        return UserService.GetByUsername(_loginViewModel.Username);
    }

    private void RedirectPatient(User foundUser)
    {
        TrollCounterService.TrollCheck(foundUser.Username);
        Patient loggedPatient = PatientService.GetByUsername(_loginViewModel.Username);
        PrescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
        DoctorRatingsService.AssignScores();
        new PatientWindow(loggedPatient)
        {
            DataContext = new PatientWindowViewModel(loggedPatient)
        }.ShowDialog();
    }

    private void RedirectDoctor()
    {
        DoctorService.LoadAppointments();
        Doctor loggedDoctor = DoctorService.GetById(_loginViewModel.Username);
        new DoctorWindow(loggedDoctor).ShowDialog();
    }

    private void RedirectSecretary()
    {
        SecretaryWindow secretaryWindow = new SecretaryWindow();
        secretaryWindow.ShowDialog();
    }

    private void RedirectManager()
    {
        ManagerWindow managerWindow = new ManagerWindow();
        managerWindow.ShowDialog();
    }
}