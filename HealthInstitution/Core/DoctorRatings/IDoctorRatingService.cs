using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.DoctorRatings;

public interface IDoctorRatingService
{
    public void Add(string username);

    public double GetAverageById(string id);

    public void AssignScores();
}