using HealthInstitution.Core.PrescriptionNotifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.PrescriptionNotifications.Repository;

public interface IPrescriptionNotificationSettingsRepository
{
    public void LoadFromFile();

    public void Save();
    public List<PrescriptionNotificationSettings> GetAll();
    public PrescriptionNotificationSettings GetById(int id);

    public void Add(PrescriptionNotificationSettings recepieNotificationSettings);

    public void Delete(int id);

    public void Update(int id, PrescriptionNotificationSettings settings);
}