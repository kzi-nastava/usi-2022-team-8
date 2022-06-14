using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Examinations.Model;

namespace HealthInstitution.Core.Examinations;

public class ExaminationViewModel : ViewModelBase
{
    private Examination _examination;

    public String RoomNo => _examination.Room.Number.ToString();
    public ExaminationStatus Status => _examination.Status;
    public DateTime Appointment => _examination.Appointment;
    public string Doctor => _examination.Doctor.Name + " " + _examination.Doctor.Surname;
    public string Anamnesis => _examination.Anamnesis;

    public ExaminationViewModel(Examination examination)
    {
        _examination = examination;
    }
}