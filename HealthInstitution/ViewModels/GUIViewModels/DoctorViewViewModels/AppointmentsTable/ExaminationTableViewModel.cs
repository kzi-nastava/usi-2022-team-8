using HealthInstitution.Commands.DoctorCommands.Scheduling;
using HealthInstitution.Commands.DoctorCommands.SchedulingDialogs;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
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
            foreach (Examination examination in _examinationService.GetByDoctor(LoggedDoctor.Username))
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
        IDoctorService _doctorService;
        IExaminationService _examinationService;
        public ExaminationTableViewModel(Doctor loggedDoctor, IDoctorService doctorService, IExaminationService examinationService)
        {
            LoggedDoctor = loggedDoctor;
            _doctorService = doctorService;
            _examinationService = examinationService;
            CreateExaminationCommand = new AddExaminationCommand(this, loggedDoctor);
            EditExaminationCommand = new EditExaminationCommand(this);
            DeleteExaminationCommand = new DeleteExaminationCommand(this, examinationService,doctorService);
            Examinations = new();
            _examinationsVM = new();
            RefreshGrid();
        }
    }
}