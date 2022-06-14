using HealthInstitution.Commands;
using HealthInstitution.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels;

public class LoginViewModel : ViewModelBase
{
    public ICommand Login { get; }
    private string _username;

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

    public LoginViewModel()
    {
        Login = new LoginCommand(this);
    }
}