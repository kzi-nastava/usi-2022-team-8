using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands;

public class ManuallScheduleCommand : CommandBase
{
    private Patient _loggedPatient;
    private IExaminationService _examinationService;
    private IScheduleEditRequestsService _scheduleEditRequestsService;
    private ITrollCounterService _trollCounterService;
    public ManuallScheduleCommand(Patient loggedPatient, IExaminationService examinationService, IScheduleEditRequestsService scheduleEditRequestsService, ITrollCounterService trollCounterService)
    {
        _loggedPatient = loggedPatient;
        _examinationService = examinationService;
        _scheduleEditRequestsService = scheduleEditRequestsService;
        _trollCounterService = trollCounterService;
    }

    public override void Execute(object? parameter)
    {
        var window = DIContainer.GetService<PatientScheduleWindow>();
        window.SetLoggedPatient(_loggedPatient);
        window.DataContext = new PatientScheduleWindowViewModel(_loggedPatient, _examinationService, _scheduleEditRequestsService, _trollCounterService);
        window.ShowDialog();
    }
}