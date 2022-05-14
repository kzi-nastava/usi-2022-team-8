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

public class TrollCounterFileRepository
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

    public void Add(string username)
    {
        TrollCounter trollCounter = new TrollCounter(username);
        this.Counters.Add(trollCounter);
        this.CountersById.Add(username, trollCounter);
        Save();
    }

    public void Delete(string id)
    {
        TrollCounter trollCounter = CountersById[id];
        if (trollCounter != null)
        {
            this.CountersById.Remove(trollCounter.Username);
            this.Counters.Remove(trollCounter);
            Save();
        }
    }

    public void TrollCheck(string username)
    {
        try
        {
            CheckCreateTroll(username);
            CheckEditDeleteTroll(username);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            BlockUser(username);

            PatientRepository.GetInstance().Save();
            Environment.Exit(0);
        }
    }

    private void BlockUser(string username)
    {
        var patientRepository = PatientRepository.GetInstance();
        var patient = patientRepository.GetByUsername(username);
        var userRepository = UserRepository.GetInstance();
        var user = userRepository.GetByUsername(username);
        patient.Blocked = BlockState.BlockedBySystem;
        user.Blocked = BlockState.BlockedBySystem;
        patientRepository.Save();
        userRepository.Save();
    }

    public void CheckCreateTroll(string username)
    {
        if (this.CountersById[username].CreateDates.Count() > 8) throw new Exception("Created too many examinations");
    }

    public void CheckEditDeleteTroll(string username)
    {
        if (this.CountersById[username].EditDeleteDates.Count() >= 5) throw new Exception("Edited too many examinations");
    }
}