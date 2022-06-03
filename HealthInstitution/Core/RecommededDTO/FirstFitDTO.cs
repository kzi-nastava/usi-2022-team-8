using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RecommededDTO;

public class FirstFitDTO
{
    public int MinHour { get; set; }
    public int MinMinutes { get; set; }
    public DateTime End { get; set; }
    public int MaxHour { get; set; }
    public int MaxMinutes { get; set; }
    public int MaxWorkingHour { get; set; }
    public string PatientUsername { get; set; }
    public string DoctorUsername { get; set; }

    public FirstFitDTO(int minHour,
    int minMinutes,
     DateTime end,
     int maxHour,
     int maxMinutes,
     int maxWorkingHour,
    string patientUsername,
    string doctorUsername)
    {
        this.MinHour = minHour;
        this.MinMinutes = minMinutes;
        this.End = end;
        this.MaxHour = maxHour;
        this.MaxMinutes = maxMinutes;
        this.MaxWorkingHour = maxWorkingHour;
        this.PatientUsername = patientUsername;
        this.DoctorUsername = doctorUsername;
        this.End = this.End.AddMinutes(maxMinutes);
        this.End = this.End.AddHours(maxHour);
    }
}

public class FindFitDTO
{
    public DateTime Fit { get; set; }
    public DateTime End { get; set; }
    public int MinHour { get; set; }
    public int MinMinutes { get; set; }
    public int MaxHour { get; set; }
    public int MaxMinutes { get; set; }

    public FindFitDTO(DateTime fit,
     DateTime end,
     int minHour,
     int minMinutes, int maxHour,
    int maxMinutes)
    {
        this.Fit = fit;
        this.End = end;
        this.MinHour = minHour;
        this.MinMinutes = minMinutes;
        this.MaxHour = maxHour;
        this.MaxMinutes = maxMinutes;
    }
}

public class ClosestFitDTO
{
    public ClosestFitDTO(int minHour, int minMinutes, DateTime end, int maxHour, int maxMinutes, int maxWorkingHour, string patientUsername, string doctorUsername, bool doctorPriority)
    {
        this.MinHour = minHour;
        this.MinMinutes = minMinutes;
        this.End = end;
        this.MaxHour = maxHour;
        this.MaxMinutes = maxMinutes;
        this.MaxWorkingHour = maxWorkingHour;
        this.PatientUsername = patientUsername;
        this.DoctorUsername = doctorUsername;
        this.DoctorPriority = doctorPriority;
        this.End = this.End.AddMinutes(maxMinutes);
        this.End = this.End.AddHours(maxHour);
    }

    public int MinHour { get; set; }
    public int MinMinutes { get; set; }
    public DateTime End { get; set; }
    public int MaxHour { get; set; }
    public int MaxMinutes { get; set; }
    public int MaxWorkingHour { get; set; }
    public string PatientUsername { get; set; }
    public string DoctorUsername { get; set; }
    public bool DoctorPriority { get; set; }
}