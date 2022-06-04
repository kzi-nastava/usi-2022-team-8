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
        return Scores.Average();
    }

    /*public void AddToScore(int score)
    {
        Scores.Add(score);
        DoctorRepository.GetInstance().ChangeRating(Username, GetAverage());
    }*/

    public DoctorRating(string username)
    {
        Username = username;
        Scores = new List<int>();
    }

    public DoctorRating()
    { }
}