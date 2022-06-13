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

    public DoctorRatingsService(IDoctorRatingRepository doctorRatingRepository)
    {
        _doctorRatingRepository = doctorRatingRepository;
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
            DoctorService.AssignScorebyId(rating.Username, rating.GetAverage());
        }
    }
}