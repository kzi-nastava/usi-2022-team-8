using HealthInstitution.Commands.DoctorCommands.Scheduling;
using HealthInstitution.Commands.DoctorCommands.SchedulingDialogs;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
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
    public class ExaminationTableViewModel : ViewModelBase
    {
        public Doctor LoggedDoctor;

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
            foreach (Examination examination in ExaminationService.GetByDoctor(LoggedDoctor.Username))
            {
                    Examinations.Add(examination);
                    _examinationsVM.Add(new ExaminationViewModel(examination));
            }
        }

        public Examination GetSelectedExamination()
        {
            return Examinations[_selectedExaminationIndex];
        }

        public ICommand CreateExaminationCommand { get; }
        public ICommand EditExaminationCommand { get; }
        public ICommand DeleteExaminationCommand { get; }

        public ExaminationTableViewModel(Doctor loggedDoctor)
        {
            LoggedDoctor = loggedDoctor;
            CreateExaminationCommand = new AddExaminationCommand(this, loggedDoctor);
            EditExaminationCommand = new EditExaminationCommand(this);
            DeleteExaminationCommand = new DeleteExaminationCommand(this);
            Examinations = new();
            _examinationsVM = new();
            RefreshGrid();
        }
    }
}