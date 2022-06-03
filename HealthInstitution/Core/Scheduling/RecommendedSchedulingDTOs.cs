using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling;

public class RecommendedSchedulingDTOs
{
    public int MinHour { get; set; }
    public int MinMinutes { get; set; }
    public DateTime End { get; set; }
    public int MaxHour { get; set; }
    public int MaxMinutes { get; set; }
    public int MaxWorkingHour { get; set; }
    public string PatientUsername { get; set; }
    public string DoctorUsername { get; set; }

    public RecommendedSchedulingDTOs(int minHour,
    int minMinutes,
     DateTime end,
     int maxHour,
     int maxMinutes,
     int maxWorkingHour,
    string patientUsername,
    string doctorUsername)
    {
        MinHour = minHour;
        MinMinutes = minMinutes;
        End = end;
        MaxHour = maxHour;
        MaxMinutes = maxMinutes;
        MaxWorkingHour = maxWorkingHour;
        PatientUsername = patientUsername;
        DoctorUsername = doctorUsername;
        End = End.AddMinutes(maxMinutes);
        End = End.AddHours(maxHour);
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
        Fit = fit;
        End = end;
        MinHour = minHour;
        MinMinutes = minMinutes;
        MaxHour = maxHour;
        MaxMinutes = maxMinutes;
    }
}

public class ClosestFitDTO
{
    public ClosestFitDTO(int minHour, int minMinutes, DateTime end, int maxHour, int maxMinutes, int maxWorkingHour, string patientUsername, string doctorUsername, bool doctorPriority)
    {
        MinHour = minHour;
        MinMinutes = minMinutes;
        End = end;
        MaxHour = maxHour;
        MaxMinutes = maxMinutes;
        MaxWorkingHour = maxWorkingHour;
        PatientUsername = patientUsername;
        DoctorUsername = doctorUsername;
        DoctorPriority = doctorPriority;
        End = End.AddMinutes(maxMinutes);
        End = End.AddHours(maxHour);
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