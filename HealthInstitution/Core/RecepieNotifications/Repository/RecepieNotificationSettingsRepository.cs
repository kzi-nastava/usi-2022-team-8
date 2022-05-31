using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.RecepieNotifications.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.RecepieNotifications.Repository;

public class RecepieNotificationSettingsRepository
{
    private String _fileName;
    public List<RecepieNotificationSettings> Settings { get; set; }
    public Dictionary<String, RecepieNotificationSettings> SettingsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private RecepieNotificationSettingsRepository(String fileName)
    {
        this._fileName = fileName;
        this.Settings = new List<RecepieNotificationSettings>();
        this.SettingsById = new Dictionary<String, RecepieNotificationSettings>();
        this.LoadFromFile();
    }

    private static RecepieNotificationSettingsRepository s_instance = null;

    public static RecepieNotificationSettingsRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new RecepieNotificationSettingsRepository(@"..\..\..\Data\JSON\recepieNotificationSettings.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var settings = JsonSerializer.Deserialize<List<RecepieNotificationSettings>>(File.ReadAllText(@"..\..\..\Data\JSON\recepieNotificationSettings.json"), _options);
        foreach (RecepieNotificationSettings setting in settings)
        {
            this.Settings.Add(setting);
            this.SettingsById.Add(setting.Id, setting);
        }
    }

    public void Save()
    {
        var allRatings = JsonSerializer.Serialize(this.Settings, _options);
        File.WriteAllText(this._fileName, allRatings);
    }

    public RecepieNotificationSettings GetById(string id)
    {
        return this.SettingsById[id];
    }

    public void Add(RecepieNotificationSettings recepieNotificationSettings)
    {
        this.Settings.Add(recepieNotificationSettings);
        this.SettingsById.Add(recepieNotificationSettings.Id, recepieNotificationSettings);
        Save();
    }

    public void Delete(string id)
    {
        RecepieNotificationSettings setting = SettingsById[id];
        if (setting != null)
        {
            this.SettingsById.Remove(setting.Id);
            this.Settings.Remove(setting);
            Save();
        }
    }

    public RecepieNotificationSettings GetRecepieNotificationSettings(string prescription)
    {
        foreach (var setting in this.Settings)
        {
            if (setting.Id == prescription) return setting;
        }
        return null;
    }
}