using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.DoctorRatings.Model;

public class DoctorRating
{
    public string Username { get; set; }
    public List<int> Scores { get; set; }

    public double GetAverage()
    {
        if (Scores.Count == 0) return 0;
        return Scores.Average();
    }

    public DoctorRating(string username)
    {
        Username = username;
        Scores = new List<int>();
    }

    public DoctorRating()
    { }
}