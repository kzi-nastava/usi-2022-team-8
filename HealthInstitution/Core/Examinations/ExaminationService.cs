using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations;

public class ExaminationService : IExaminationService
{
    IExaminationRepository _examinationRepository;
    IDoctorExaminationAvailabilityService _doctorExaminationAvailabilityService;
    IPatientExaminationAvailabilityService _patientExaminationAvailabilityService;
    IExaminationDoctorRepository _examinationDoctorRepository;
    public ExaminationService(IExaminationRepository examinationRepository,
        IDoctorExaminationAvailabilityService doctorExaminationAvailabilityService,
        IPatientExaminationAvailabilityService patientExaminationAvailabilityService,
        IExaminationDoctorRepository examinationDoctorRepository)
    {
        _examinationRepository = examinationRepository;
        _doctorExaminationAvailabilityService = doctorExaminationAvailabilityService;
        _patientExaminationAvailabilityService = patientExaminationAvailabilityService;
        _examinationDoctorRepository = examinationDoctorRepository;
    }
    public int GetMaxId()
    {
        return _examinationRepository.GetMaxId();
    }
    public List<Examination> GetAll()
    {
        return _examinationRepository.GetAll();
    }

    public Examination GetById(int id)
    {
        return _examinationRepository.GetById(id);
    }

    public Examination Add(ExaminationDTO examinationDTO)
    {
        Examination examination = new Examination(examinationDTO);
        _examinationRepository.Add(examination);
        return examination;
    }

    public void Update(int id, ExaminationDTO examinationDTO)
    {
        examinationDTO.Validate();
        Examination examination = new Examination(examinationDTO);
        _doctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
        _patientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
        _examinationRepository.Update(id, examination);
    }

    public void Delete(int id)
    {
        _examinationRepository.Delete(id);
    }

    public List<Examination> GetByPatient(string patientUsername)
    {
        return _examinationRepository.GetByPatient(patientUsername);
    }

    public List<Examination> GetByDoctor(string doctorUsername)
    {
        return _examinationRepository.GetByDoctor(doctorUsername);
    }

    public List<Examination> GetCompletedByPatient(string patientUsername)
    {
        return _examinationRepository.GetCompletedByPatient(patientUsername);
    }

    public List<Examination> GetSearchAnamnesis(string keyword, string patientUsername)
    {
        return _examinationRepository.GetSeachAnamnesis(keyword, patientUsername);
    }

    public List<Examination> OrderByDoctorSpeciality(List<Examination> examinations)
    {
        _examinationDoctorRepository.LoadFromFile();
        return examinations.OrderBy(o => o.Doctor.Specialty).ToList();
    }

    public List<Examination> OrderByDate(List<Examination> examinations)
    {
        return examinations.OrderBy(o => o.Appointment).ToList();
    }

    public List<Examination> OrderByDoctor(List<Examination> examinations)
    {
        return examinations.OrderBy(o => o.Doctor.Username).ToList();
    }

    public ExaminationDTO ParseExaminationToExaminationDTO(Examination examination)
    {
        return new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
    }

    public bool IsReadyForPerforming(Examination examination)
    {
        return examination.Appointment <= DateTime.Now;
    }

    public void Complete(Examination examination, string anamnesis)
    {
        examination.Anamnesis = anamnesis;
        examination.Status = ExaminationStatus.Completed;
        _examinationRepository.Save();
    }
}