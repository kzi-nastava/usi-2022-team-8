using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.DoctorRatings.Repository;
using HealthInstitution.Core.DoctorRatings.Model;
using HealthInstitution.Core.SystemUsers.Doctors;

namespace HealthInstitution.Core.DoctorRatings;

public class DoctorRatingsService : IDoctorRatingService
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
}