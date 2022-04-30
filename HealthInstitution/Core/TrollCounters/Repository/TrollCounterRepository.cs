using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.TrollCounters.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.TrollCounters.Repository;

public class TrollCounterRepository
{
    public String fileName { get; set; }
    public List<TrollCounter> trollCounters { get; set; }
    public Dictionary<String, TrollCounter> trollCountersById { get; set; }

    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private TrollCounterRepository(String fileName)
    {
        this.fileName = fileName;
        this.trollCounters = new List<TrollCounter>();
        this.trollCountersById = new Dictionary<string, TrollCounter>();
        this.LoadCounters();
    }

    private static TrollCounterRepository instance = null;

    public static TrollCounterRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new TrollCounterRepository(@"..\..\..\Data\JSON\trollCounters.json");
            }
            return instance;
        }
    }

    public void LoadCounters()
    {
        var counters = JsonSerializer.Deserialize<List<TrollCounter>>(File.ReadAllText(@"..\..\..\Data\JSON\trollCounters.json"), options);
        foreach (TrollCounter trollCounter in counters)
        {
            this.trollCounters.Add(trollCounter);
            this.trollCountersById.Add(trollCounter.username, trollCounter);
        }
    }

    public void SaveTrollCounters()
    {
        var allTrollCounters = JsonSerializer.Serialize(this.trollCounters, options);
        File.WriteAllText(this.fileName, allTrollCounters);
    }

    public TrollCounter GetTrollCounterById(string id)
    {
        return this.trollCountersById[id];
    }

    public void AddTrollCounter(string username)
    {
        TrollCounter trollCounter = new TrollCounter(username);
        this.trollCounters.Add(trollCounter);
        this.trollCountersById.Add(username, trollCounter);
        SaveTrollCounters();
    }

    public void DeleteTrollCounter(string id)
    {
        TrollCounter trollCounter = trollCountersById[id];
        if (trollCounter != null)
        {
            this.trollCountersById.Remove(trollCounter.username);
            this.trollCounters.Remove(trollCounter);
            SaveTrollCounters();
        }
    }

    public void CheckTroll(string username)
    {
        CheckCreateTroll(username);
        CheckEditDeleteTroll(username);
    }

    public void CheckCreateTroll(string username)
    {
        if (this.trollCountersById[username].createDates.Count() > 8) throw new Exception("Created too many examinations");
    }

    public void CheckEditDeleteTroll(string username)
    {
        if (this.trollCountersById[username].editDeleteDates.Count() >= 5) throw new Exception("Edited too many examinations");
    }
}