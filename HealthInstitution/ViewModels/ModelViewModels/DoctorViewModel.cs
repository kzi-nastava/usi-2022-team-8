using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels;

public class DoctorViewModel : ViewModelBase
{
    private Doctor _doctor;

    public DoctorViewModel(Doctor doctor)
    {
        _doctor = doctor;
    }

    public string Name => _doctor.Name;
    public string Surname => _doctor.Surname;
    public SpecialtyType Speciality => _doctor.Specialty;
    public double AvgRating => _doctor.AvgRating;
}