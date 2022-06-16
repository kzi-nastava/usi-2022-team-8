using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.PrescriptionNotifications.Repository;

public class PrescriptionNotificationSettingsRepository : IPrescriptionNotificationSettingsRepository
{
    private String _fileName = @"..\..\..\Data\JSON\recepieNotificationSettings.json";
    public List<PrescriptionNotificationSettings> Settings { get; set; }
    public Dictionary<int, PrescriptionNotificationSettings> SettingsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public PrescriptionNotificationSettingsRepository()
    {
        this.Settings = new List<PrescriptionNotificationSettings>();
        this.SettingsById = new Dictionary<int, PrescriptionNotificationSettings>();
        this.LoadFromFile();
    }

    public void LoadFromFile()
    {
        var settings = JsonSerializer.Deserialize<List<PrescriptionNotificationSettings>>(File.ReadAllText(@"..\..\..\Data\JSON\recepieNotificationSettings.json"), _options);
        foreach (PrescriptionNotificationSettings setting in settings)
        {
            this.Settings.Add(setting);
            this.SettingsById.Add(setting.Id, setting);
        }
    }
    public List<PrescriptionNotificationSettings> GetAll()
    {
        return Settings;
    }

    public Dictionary<int, PrescriptionNotificationSettings> GetAllById()
    {
        return this.SettingsById;
    }
    public void Save()
    {
        var allRatings = JsonSerializer.Serialize(this.Settings, _options);
        File.WriteAllText(this._fileName, allRatings);
    }

    public PrescriptionNotificationSettings GetById(int id)
    {
        return this.SettingsById[id];
    }

    public void Update(int id, PrescriptionNotificationSettings settings)
    {
        if (SettingsById.ContainsKey(id))
        {
            var current = SettingsById[id];
            Settings.Remove(current);
            Settings.Add(settings);
            SettingsById[id] = settings;
            Save();
        }
        else
        {
            Add(settings);
        }
    }

    public void Add(PrescriptionNotificationSettings recepieNotificationSettings)
    {
        this.Settings.Add(recepieNotificationSettings);
        this.SettingsById.Add(recepieNotificationSettings.Id, recepieNotificationSettings);
        Save();
    }

    public void Delete(int id)
    {
        PrescriptionNotificationSettings setting = SettingsById[id];
        if (setting != null)
        {
            this.SettingsById.Remove(setting.Id);
            this.Settings.Remove(setting);
            Save();
        }
    }
}