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

public static class ExaminationService
{
    private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();

    public static List<Examination> GetAll()
    {
        LinkDoctors();
        return s_examinationRepository.GetAll();
    }

    public static Examination GetById(int id)
    {
        LinkDoctors();
        return s_examinationRepository.GetById(id);
    }

    public static Examination Add(ExaminationDTO examinationDTO)
    {
        Examination examination = new Examination(examinationDTO);
        s_examinationRepository.Add(examination);
        return examination;
    }

    public static void Update(int id, ExaminationDTO examinationDTO)
    {
        examinationDTO.Validate();
        Examination examination = new Examination(examinationDTO);
        DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
        PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
        s_examinationRepository.Update(id, examination);
    }

    public static void Delete(int id)
    {
        s_examinationRepository.Delete(id);
    }

    public static List<Examination> GetByPatient(string patientUsername)
    {
        return s_examinationRepository.GetByPatient(patientUsername);
    }

    public static List<Examination> GetByDoctor(string doctorUsername)
    {
        return s_examinationRepository.GetByDoctor(doctorUsername);
    }

    public static List<Examination> GetCompletedByPatient(string patientUsername)
    {
        ExaminationDoctorRepository.GetInstance().LoadFromFile();
        return s_examinationRepository.GetCompletedByPatient(patientUsername);
    }

    public static List<Examination> GetSearchAnamnesis(string keyword, string patientUsername)
    {
        return s_examinationRepository.GetSeachAnamnesis(keyword, patientUsername);
    }

    public static List<Examination> OrderByDoctorSpeciality(List<Examination> examinations)
    {
        ExaminationDoctorRepository.GetInstance().LoadFromFile();
        return examinations.OrderBy(o => o.Doctor.Specialty).ToList();
    }

    public static List<Examination> OrderByDate(List<Examination> examinations)
    {
        return examinations.OrderBy(o => o.Appointment).ToList();
    }

    public static List<Examination> OrderByDoctor(List<Examination> examinations)
    {
        return examinations.OrderBy(o => o.Doctor.Username).ToList();
    }

    public static ExaminationDTO ParseExaminationToExaminationDTO(Examination examination)
    {
        return new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
    }

    public static bool IsReadyForPerforming(Examination examination)
    {
        return examination.Appointment <= DateTime.Now;
    }

    public static void Complete(Examination examination, string anamnesis)
    {
        examination.Anamnesis = anamnesis;
        examination.Status = ExaminationStatus.Completed;
        s_examinationRepository.Save();
    }

    public static void LinkDoctors()
    {
        ExaminationRepository.GetInstance();
        ExaminationDoctorRepository.GetInstance().LoadFromFile();
    }
}