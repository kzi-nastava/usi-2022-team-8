using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms;

namespace HealthInstitution.Core.Scheduling;

public class RecommendedSchedulingService : IRecommendedSchedulingService
{
    IMedicalRecordService _medicalRecordService;
    IPatientExaminationAvailabilityService patientExaminationAvailabilityService;
    IDoctorExaminationAvailabilityService _doctorExaminationAvailabilityService;
    ISchedulingService _schedulingService;
    IExaminationService _examinationService;

    public RecommendedSchedulingService(IMedicalRecordService medicalRecordService, IPatientExaminationAvailabilityService patientExaminationAvailabilityService,
        IDoctorExaminationAvailabilityService doctorExaminationAvailabilityService, ISchedulingService schedulingService, IExaminationService examinationService)
    {
        _medicalRecordService = medicalRecordService;
        this.patientExaminationAvailabilityService = patientExaminationAvailabilityService;
        _doctorExaminationAvailabilityService = doctorExaminationAvailabilityService;
        _schedulingService = schedulingService;
        _examinationService = examinationService;
    }

    private ExaminationDTO FindFit(ExaminationDTO examinationDTO, FindFitDTO findFitDTO)
    {
        bool found = false;
        while (findFitDTO.Fit <= findFitDTO.End)
        {
            try
            {
                Room room = _schedulingService.FindAvailableExaminationRoom(findFitDTO.Fit);
                examinationDTO.Appointment = findFitDTO.Fit;
                examinationDTO.Room = room;
                patientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
                _doctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
                found = true;
                break;
            }
            catch
            {
                findFitDTO.Fit = IncrementFit(findFitDTO.Fit, findFitDTO.MaxHour, findFitDTO.MaxMinutes, findFitDTO.MinHour, findFitDTO.MinMinutes);
            }
        }
        if (found)
            return examinationDTO;
        else
            return null;
    }

    public bool FindFirstFit(RecommendedSchedulingDTOs firstFitDTO)
    {
        bool found = false;
        DateTime fit = GenerateFitDateTime(firstFitDTO.MinHour, firstFitDTO.MinMinutes);
        Doctor doctor = DoctorRepository.GetInstance().GetById(firstFitDTO.DoctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(firstFitDTO.PatientUsername);
        var medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
        ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);
        FindFitDTO findFitDTO = new FindFitDTO(fit, firstFitDTO.End, firstFitDTO.MinHour, firstFitDTO.MinMinutes, firstFitDTO.MaxHour, firstFitDTO.MaxMinutes);
        ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
        if (firstFit is not null)
        {
            found = true;
            _examinationService.Add(examinationDTO);
            MessageBox.Show("Examination scheduled for: " + fit.ToString());
        }
        return found;
    }

    public DateTime GenerateFitDateTime(int minHour, int minMinutes)
    {
        DateTime fit = DateTime.Today.AddDays(1);
        fit = fit.AddHours(minHour);
        fit = fit.AddMinutes(minMinutes);
        return fit;
    }

    public List<Examination> FindClosestFit(ClosestFitDTO closestFitDTO)
    {
        Doctor pickedDoctor = DoctorRepository.GetInstance().GetById(closestFitDTO.DoctorUsername);
        Patient patient = PatientRepository.GetInstance().GetByUsername(closestFitDTO.PatientUsername);
        var medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
        List<Examination> suggestions = new List<Examination>();
        List<Doctor> viableDoctors = new List<Doctor>();

        if (closestFitDTO.DoctorPriority)
        {
            closestFitDTO.MaxHour = 22;
            closestFitDTO.MaxMinutes = 45;
            viableDoctors.Add(pickedDoctor);
            closestFitDTO.End = closestFitDTO.End.AddHours(-closestFitDTO.End.Hour + 22);
            closestFitDTO.End = closestFitDTO.End.AddMinutes(-closestFitDTO.End.Minute + 45);
        }
        else
        {
            viableDoctors = DoctorRepository.GetInstance().GetAll();
            viableDoctors.Remove(pickedDoctor);
        }

        foreach (Doctor doctor in viableDoctors)
        {
            DateTime fit = GenerateFitDateTime(closestFitDTO.MinHour, closestFitDTO.MinMinutes);
            ExaminationDTO examinationDTO = new ExaminationDTO(fit, null, doctor, medicalRecord);

            if (suggestions.Count == 3) break;
            while (fit <= closestFitDTO.End)
            {
                if (suggestions.Count == 3) break;
                FindFitDTO findFitDTO = new FindFitDTO(fit, closestFitDTO.End, closestFitDTO.MaxHour, closestFitDTO.MinMinutes, closestFitDTO.MaxHour, closestFitDTO.MaxMinutes);
                ExaminationDTO firstFit = FindFit(examinationDTO, findFitDTO);
                if (firstFit is not null)
                {
                    suggestions.Add(new Examination(firstFit));
                    fit = firstFit.Appointment;
                    fit = IncrementFit(fit, closestFitDTO.MaxHour, closestFitDTO.MaxMinutes, closestFitDTO.MinHour, closestFitDTO.MinMinutes);
                }
            }
        }
        return suggestions;
    }

    public DateTime IncrementFit(DateTime fit, int maxHour, int maxMinutes, int minHour, int minMinutes)
    {
        fit = fit.AddMinutes(15);

        if ((fit.Hour > maxHour) || (fit.Hour == maxHour && fit.Minute > maxMinutes))
        {
            fit = fit.AddDays(1);
            fit = fit.AddHours(minHour - fit.Hour);
            fit = fit.AddMinutes(minMinutes - fit.Minute);
        }
        return fit;
    }
}