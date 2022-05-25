using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.DoctorRatings.Model
{
    internal class DoctorRating
    {
        public string Username { get; set; }
        public List<int> Ratings { get; set; }

        public double GetAverage()
        {
            return Ratings.Average();
        }

        public void AddRating(int score)
        {
            Ratings.Add(score);
            DoctorRepository.GetInstance().ChangeRating(Username, GetAverage());
        }
    }
}