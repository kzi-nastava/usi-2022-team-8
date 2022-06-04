using HealthInstitution.Core.TrollCounters.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace HealthInstitution.Core.TrollCounters.Repository;

public class TrollCounterFileRepository : ITrollCounterFileRepository
{
    private String _fileName;
    public List<TrollCounter> Counters { get; set; }
    public Dictionary<String, TrollCounter> CountersById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private TrollCounterFileRepository(String fileName)
    {
        this._fileName = fileName;
        this.Counters = new List<TrollCounter>();
        this.CountersById = new Dictionary<string, TrollCounter>();
        this.LoadFromFile();
    }

    private static TrollCounterFileRepository s_instance = null;

    public static TrollCounterFileRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new TrollCounterFileRepository(@"..\..\..\Data\JSON\trollCounters.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var counters = JsonSerializer.Deserialize<List<TrollCounter>>(File.ReadAllText(@"..\..\..\Data\JSON\trollCounters.json"), _options);
        foreach (TrollCounter trollCounter in counters)
        {
            this.Counters.Add(trollCounter);
            this.CountersById.Add(trollCounter.Username, trollCounter);
        }
    }

    public void Save()
    {
        var allTrollCounters = JsonSerializer.Serialize(this.Counters, _options);
        File.WriteAllText(this._fileName, allTrollCounters);
    }

    public TrollCounter GetById(string id)
    {
        return this.CountersById[id];
    }

    public void Add(TrollCounter trollCounter)
    {
        Counters.Add(trollCounter);
        CountersById.Add(trollCounter.Username, trollCounter);
        Save();
    }

    public void Delete(TrollCounter trollCounter)
    {
        this.CountersById.Remove(trollCounter.Username);
        this.Counters.Remove(trollCounter);
        Save();
    }

    public void CheckCreateTroll(string username)
    {
        if (CountersById[username].CreateDates.Count() > 8) throw new Exception("Created too many examinations");
    }

    public void CheckEditDeleteTroll(string username)
    {
        if (CountersById[username].EditDeleteDates.Count() >= 5) throw new Exception("Edited too many examinations");
    }
    public void AppendEditDeleteDates(string username)
    {
        GetById(username).AppendEditDeleteDates(DateTime.Today);
        Save();
    }
    public void AppendCreateDates(string username)
    {
        GetById(username).AppendCreateDates(DateTime.Today);
        Save();
    }
}