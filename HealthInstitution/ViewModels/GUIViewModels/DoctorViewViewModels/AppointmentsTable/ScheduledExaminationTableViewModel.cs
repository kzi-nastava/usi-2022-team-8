using HealthInstitution.Commands.DoctorCommands.ExaminationPerforming;
using HealthInstitution.Commands.DoctorCommands.Timetable;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable
{
    public class ScheduledExaminationTableViewModel : ViewModelBase
    {
        public Doctor LoggedDoctor;

        private DateTime _selectedDateTime = DateTime.Now;

        public DateTime SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }

        private object _appointmentChoice = "0";

        public object AppointmentChoice
        {
            get
            {
                return _appointmentChoice;
            }
            set
            {
                _appointmentChoice = value;
                OnPropertyChanged(nameof(AppointmentChoice));
            }
        }

        public int GetAppointmentChoice()
        {
            return Convert.ToInt32(AppointmentChoice as string);
        }

        private object _datesChoice = "1";

        public object DatesChoice
        {
            get
            {
                return _datesChoice;
            }
            set
            {
                _datesChoice = value;
                OnPropertyChanged(nameof(DatesChoice));
            }
        }

        public List<Examination> Examinations;

        private int _selectedExaminationIndex;

        public int SelectedExaminationIndex
        {
            get
            {
                return _selectedExaminationIndex;
            }
            set
            {
                _selectedExaminationIndex = value;
                OnPropertyChanged(nameof(SelectedExaminationIndex));
            }
        }

        private ObservableCollection<ExaminationViewModel> _examinationsVM;

        public ObservableCollection<ExaminationViewModel> ExaminationsVM
        {
            get
            {
                return _examinationsVM;
            }
            set
            {
                _examinationsVM = value;
                OnPropertyChanged(nameof(ExaminationsVM));
            }
        }

        public void RefreshGrid()
        {
            _examinationsVM.Clear();
            Examinations.Clear();
            List<Examination> selectedExaminations;
            List<Examination> scheduledExaminations = TimetableService.GetScheduledExaminations(LoggedDoctor);
            if (DatesChoice == "0")
            {
                selectedExaminations = TimetableService.GetExaminationsInThreeDays(scheduledExaminations);
            }
            else
            {
                DateTime date = SelectedDateTime.Date;
                selectedExaminations = TimetableService.GetExaminationsByDate(scheduledExaminations, date);
            }
            foreach (var examination in selectedExaminations)
            {
                Examinations.Add(examination);
                _examinationsVM.Add(new ExaminationViewModel(examination));
            }
        }

        public Examination GetSelectedExamination()
        {
            return Examinations[_selectedExaminationIndex];
        }

        public MedicalRecord GetSelectedMedicalRecord()
        {
            return GetSelectedExamination().MedicalRecord;
        }

        public ICommand ShowDataGridCommand { get; }
        public ICommand ShowMedicalRecordCommand { get; }
        public ICommand StartExaminationCommand { get; }

        public ScheduledExaminationTableViewModel(Doctor loggedDoctor)
        {
            LoggedDoctor = loggedDoctor;
            ShowDataGridCommand = new ShowDataGridCommand(this);
            ShowMedicalRecordCommand = new ShowMedicalRecordCommand(this);
            StartExaminationCommand = new StartExaminationCommand(this);
            Examinations = new();
            _examinationsVM = new();
            RefreshGrid();
        }
    }
}