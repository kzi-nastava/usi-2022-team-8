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

public class DoctorRatingsService : IDoctorRatingsService
{
    IDoctorRatingRepository _doctorRatingRepository;
    IDoctorService _doctorService;

    public DoctorRatingsService(IDoctorRatingRepository doctorRatingRepository, IDoctorService doctorService)
    {
        _doctorRatingRepository = doctorRatingRepository;
        _doctorService = doctorService;
    }
    public void Add(string username)
    {
        _doctorRatingRepository.Add(username);
    }

    public double GetAverageById(string id)
    {
        return _doctorRatingRepository.GetById(id).GetAverage();
    }

    public void AssignScores()
    {
        foreach (var rating in _doctorRatingRepository.GetAll())
        {
            _doctorService.AssignScorebyId(rating.Username, rating.GetAverage());
        }
    }
    public List<Doctor> LoadSortedDoctors()
    {
        AssignScores();
        return _doctorService.GetDoctorsOrderByRating();
    }
    public List<Doctor> GetTopRated(int num)
    {
        var sortedDoctors = LoadSortedDoctors();
        var topRatedDoctors = sortedDoctors.Skip(Math.Max(0, sortedDoctors.Count() - num)).ToList();
        topRatedDoctors.Reverse();
        return topRatedDoctors;
    }

    public List<Doctor> GetWorstRated(int num)
    {
        var sortedDoctors = LoadSortedDoctors();
        var topRatedDoctors = sortedDoctors.Take(num).ToList();
        return topRatedDoctors;
    }

    public void UpdateScore(string DoctorUsername, int score)
    {
        var doctor = _doctorService.GetById(DoctorUsername);
        var ratings = _doctorRatingRepository.GetById(DoctorUsername);
        ratings.Scores.Add(score);
        doctor.AvgRating = GetAverageById(DoctorUsername);
        _doctorRatingRepository.Save();
    }
}