using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.Timetable;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for ScheduledExaminationTable.xaml
    /// </summary>
    public partial class ScheduledExaminationTable : Window
    {
        Doctor _loggedDoctor;
        IDoctorTimetableService _timetableService;
        IExaminationService _examinationService;
        public ScheduledExaminationTable(IDoctorTimetableService timetableService, IExaminationService examinationService)
        {
            //this._loggedDoctor = doctor;   
            InitializeComponent();
            _examinationService = examinationService;
            _timetableService = timetableService;
            //examinationRadioButton.IsChecked = true;
            //datePicker.SelectedDate = DateTime.Now;
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            DataContext = new ScheduledExaminationTableViewModel(doctor,_examinationService,_timetableService);
        }
    }
}
