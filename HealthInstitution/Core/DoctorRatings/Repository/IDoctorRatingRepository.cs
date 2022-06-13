using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.DoctorRatings.Model;

namespace HealthInstitution.Core.DoctorRatings.Repository;

public interface IDoctorRatingRepository
{
    public void LoadFromFile();

    public void Save();

    public DoctorRating GetById(string id);

    public void Add(string username);

    public void Delete(string id);
}