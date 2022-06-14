using HealthInstitution.Core.DoctorRatings.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;

namespace HealthInstitution.Core.DoctorRatings.Repository;

public class DoctorRatingRepository : IDoctorRatingRepository
{
    private String _fileName;
    public List<DoctorRating> Ratings { get; set; }
    public Dictionary<String, DoctorRating> RatingsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private DoctorRatingRepository(String fileName)
    {
        this._fileName = fileName;
        this.Ratings = new List<DoctorRating>();
        this.RatingsById = new Dictionary<String, DoctorRating>();
        this.LoadFromFile();
    }

    private static DoctorRatingRepository s_instance = null;

    public static DoctorRatingRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new DoctorRatingRepository(@"..\..\..\Data\JSON\doctorRatings.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var ratings = JsonSerializer.Deserialize<List<DoctorRating>>(File.ReadAllText(@"..\..\..\Data\JSON\doctorRatings.json"), _options);
        foreach (DoctorRating doctorRating in ratings)
        {
            this.Ratings.Add(doctorRating);
            this.RatingsById.Add(doctorRating.Username, doctorRating);
            //DoctorRepository.GetInstance().ChangeRating(doctorRating.Username, doctorRating.GetAverage());
        }
    }

    public void Save()
    {
        var allRatings = JsonSerializer.Serialize(this.Ratings, _options);
        File.WriteAllText(this._fileName, allRatings);
    }

    public DoctorRating GetById(string id)
    {
        return this.RatingsById[id];
    }

    public void Add(string username)
    {
        DoctorRating doctorRating = new DoctorRating(username);
        this.Ratings.Add(doctorRating);
        this.RatingsById.Add(username, doctorRating);
        Save();
    }

    public void Delete(string id)
    {
        DoctorRating rating = RatingsById[id];
        if (rating != null)
        {
            this.RatingsById.Remove(rating.Username);
            this.Ratings.Remove(rating);
            Save();
        }
    }

    public List<DoctorRating> GetAll()
    {
        return Ratings;
    }

}