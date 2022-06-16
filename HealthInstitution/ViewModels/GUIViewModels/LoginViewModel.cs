using HealthInstitution.Commands;
using HealthInstitution.Core;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels;

public class LoginViewModel : ViewModelBase
{
    public ICommand Login { get; }
    public Window ThisWindow;
    private string _username;

    IUserService _userService;
    ITrollCounterService _trollCounterService;
    IPatientService _patientService;
    IDoctorService _doctorService;
    IPrescriptionNotificationService _prescriptionNotificationService;
    IDoctorRatingsService _doctorRatingsService;

    public LoginViewModel(Window loginWindow, IUserService userService, ITrollCounterService trollCounterService, IPatientService patientService, IDoctorService doctorService, IPrescriptionNotificationService prescriptionNotificationService, IDoctorRatingsService doctorRatingsService)
    {
        this.ThisWindow = loginWindow;
        Login = new LoginCommand(this, userService, trollCounterService, patientService, doctorService, prescriptionNotificationService, doctorRatingsService);
        _userService = userService;
        _trollCounterService = trollCounterService;
        _patientService = patientService;
        _doctorService = doctorService;
        _prescriptionNotificationService = prescriptionNotificationService;
        _doctorRatingsService = doctorRatingsService;
    }

    public string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    private SecureString _password;

    public SecureString Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    
}