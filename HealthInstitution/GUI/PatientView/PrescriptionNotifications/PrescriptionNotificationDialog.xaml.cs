using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationDialog.xaml
/// </summary>
public partial class RecepieNotificationDialog : Window
{
    public RecepieNotificationDialog(string _loggedPatient)
    {
        InitializeComponent();
    }
}