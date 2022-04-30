using HealthInstitution.Core.TrollCounters.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.TrollCounters.Repository;

public class TrollCounterFileRepository
{
    public String fileName { get; set; }
    public List<TrollCounter> allCounters { get; set; }
    public Dictionary<String, TrollCounter> allCountersById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private TrollCounterFileRepository(String fileName)
    {
        this.fileName = fileName;
        this.allCounters = new List<TrollCounter>();
        this.allCountersById = new Dictionary<string, TrollCounter>();
        this.LoadCounters();
    }

    private static TrollCounterFileRepository instance = null;

    public static TrollCounterFileRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new TrollCounterFileRepository(@"..\..\..\Data\JSON\trollCounters.json");
            }
            return instance;
        }
    }

    public void LoadCounters()
    {
        var counters = JsonSerializer.Deserialize<List<TrollCounter>>(File.ReadAllText(@"..\..\..\Data\JSON\trollCounters.json"), options);
        foreach (TrollCounter trollCounter in counters)
        {
            this.allCounters.Add(trollCounter);
            this.allCountersById.Add(trollCounter.username, trollCounter);
        }
    }

    public void Save()
    {
        var allTrollCounters = JsonSerializer.Serialize(this.allCounters, options);
        File.WriteAllText(this.fileName, allTrollCounters);
    }

    public TrollCounter GetTrollCounterById(string id)
    {
        return this.allCountersById[id];
    }

    public void AddTrollCounter(string username)
    {
        TrollCounter trollCounter = new TrollCounter(username);
        this.allCounters.Add(trollCounter);
        this.allCountersById.Add(username, trollCounter);
        Save();
    }

    public void DeleteTrollCounter(string id)
    {
        TrollCounter trollCounter = allCountersById[id];
        if (trollCounter != null)
        {
            this.allCountersById.Remove(trollCounter.username);
            this.allCounters.Remove(trollCounter);
            Save();
        }
    }

    public void TrollCheck(string username)
    {
        CheckCreateTroll(username);
        CheckEditDeleteTroll(username);
    }

    public void CheckCreateTroll(string username)
    {
        if (this.allCountersById[username].createDates.Count() > 8) throw new Exception("Created too many examinations");
    }

    public void CheckEditDeleteTroll(string username)
    {
        if (this.allCountersById[username].editDeleteDates.Count() >= 5) throw new Exception("Edited too many examinations");
    }
}