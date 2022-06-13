using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.DoctorRatings.Repository;
using HealthInstitution.Core.DoctorRatings.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.DoctorRatings;

public class DoctorRatingsService
{
    private static DoctorRatingRepository s_doctorRatingRepository = DoctorRatingRepository.GetInstance();
    public static void Add(string username)
    {
        s_doctorRatingRepository.Add(username);
    }

    public static double GetAverageById(string id)
    {
        return s_doctorRatingRepository.RatingsById[id].GetAverage();
    }

    public static void AssignScores()
    {
        foreach (var rating in s_doctorRatingRepository.GetAll())
        {
            DoctorService.AssignScorebyId(rating.Username, rating.GetAverage());
        }
    }
    public static List<Doctor> LoadSortedDoctors()
    {
        AssignScores();
        return DoctorService.GetDoctorsOrderByRating();
    }
    public static List<Doctor> GetTopRated(int num)
    {
        var sortedDoctors = LoadSortedDoctors();
        var topRatedDoctors = sortedDoctors.Skip(Math.Max(0, sortedDoctors.Count() - num)).ToList();
        topRatedDoctors.Reverse();
        return topRatedDoctors;
    }

    public static List<Doctor> GetWorstRated(int num)
    {
        var sortedDoctors = LoadSortedDoctors();
        var topRatedDoctors = sortedDoctors.Take(num).ToList();
        return topRatedDoctors;
    }

    public static void UpdateScore(string DoctorUsername, int score)
    {
        var doctor = DoctorService.GetById(DoctorUsername);
        var ratings = s_doctorRatingRepository.GetById(DoctorUsername);
        ratings.Scores.Add(score);
        doctor.AvgRating = GetAverageById(DoctorUsername);
        s_doctorRatingRepository.Save();
    }
}