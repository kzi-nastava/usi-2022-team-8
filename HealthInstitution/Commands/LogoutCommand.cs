﻿using HealthInstitution.Core;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.ViewModels.GUIViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands
{
    public class LogoutCommand : CommandBase
    {
        public PatientWindow Window { get; set; }

        public override void Execute(object? parameter)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Window.Close();
                new LoginWindow()
                {
                    DataContext = new LoginViewModel()
                }.ShowDialog();
            }
        }
    }
}