using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RecommededDTO;

public class FirstFitDTO
{
    public int minHour { get; set; }
    public int minMinutes { get; set; }
    public DateTime end { get; set; }
    public int maxHour { get; set; }
    public int maxMinutes { get; set; }
    public int maxWorkingHour { get; set; }
    public string patientUsername { get; set; }
    public string doctorUsername { get; set; }

    public FirstFitDTO(int minHour,
    int minMinutes,
     DateTime end,
     int maxHour,
     int maxMinutes,
     int maxWorkingHour,
    string patientUsername,
    string doctorUsername)
    {
        this.minHour = minHour;
        this.minMinutes = minMinutes;
        this.end = end;
        this.maxHour = maxHour;
        this.maxMinutes = maxMinutes;
        this.maxWorkingHour = maxWorkingHour;
        this.patientUsername = patientUsername;
        this.doctorUsername = doctorUsername;
    }
}

public class FindFitDTO
{
    public DateTime fit { get; set; }
    public DateTime end { get; set; }
    public int minHour { get; set; }
    public int minMinutes { get; set; }
    public int maxHour { get; set; }
    public int maxMinutes { get; set; }

    public FindFitDTO(DateTime fit,
     DateTime end,
     int minHour,
     int minMinutes, int maxHour,
    int maxMinutes)
    {
        this.fit = fit;
        this.end = end;
        this.minHour = minHour;
        this.minMinutes = minMinutes;
        this.maxHour = maxHour;
        this.maxMinutes = maxMinutes;
    }
}

public class ClosestFitDTO
{
    public ClosestFitDTO(int minHour, int minMinutes, DateTime end, int maxHour, int maxMinutes, int maxWorkingHour, string patientUsername, string doctorUsername, bool doctorPriority)
    {
        this.minHour = minHour;
        this.minMinutes = minMinutes;
        this.end = end;
        this.maxHour = maxHour;
        this.maxMinutes = maxMinutes;
        this.maxWorkingHour = maxWorkingHour;
        this.patientUsername = patientUsername;
        this.doctorUsername = doctorUsername;
        this.doctorPriority = doctorPriority;
    }

    public int minHour { get; set; }
    public int minMinutes { get; set; }
    public DateTime end { get; set; }
    public int maxHour { get; set; }
    public int maxMinutes { get; set; }
    public int maxWorkingHour { get; set; }
    public string patientUsername { get; set; }
    public string doctorUsername { get; set; }
    public bool doctorPriority { get; set; }
}