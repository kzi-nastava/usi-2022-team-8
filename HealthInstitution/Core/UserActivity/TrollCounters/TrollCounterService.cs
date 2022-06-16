using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.TrollCounters.Model;
using HealthInstitution.Core.TrollCounters.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Core.TrollCounters
{
    public class TrollCounterService : ITrollCounterService
    {
        ITrollCounterFileRepository _trollCounterFileRepository;
        public TrollCounterService(ITrollCounterFileRepository trollCounterFileRepository)
        {
            _trollCounterFileRepository = trollCounterFileRepository;
        }
        public TrollCounter GetById(string id)
        {
            return _trollCounterFileRepository.GetById(id);
        }
        public void Add(string username)
        {
            TrollCounter trollCounter = new TrollCounter(username);
            _trollCounterFileRepository.Add(trollCounter);
        }
        public void Delete(string id)
        {
            TrollCounter trollCounter = GetById(id);
            if (trollCounter != null)
            {
                _trollCounterFileRepository.Delete(trollCounter);
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
                BlockPatient(username);
                Environment.Exit(0);
            }
        }
        public void BlockPatient(string username)
        {
            DIContainer.DIContainer.GetService<IPatientService>().ChangeBlockedStatus(username);
        }
        public void CheckCreateTroll(string username)
        {
            _trollCounterFileRepository.CheckCreateTroll(username);
        }
        public void CheckEditDeleteTroll(string username)
        {
            _trollCounterFileRepository.CheckEditDeleteTroll(username);
        }
        public void AppendEditDeleteDates(string username)
        {
            _trollCounterFileRepository.AppendEditDeleteDates(username);
        }
        public void AppendCreateDates(string username)
        {
            _trollCounterFileRepository.AppendCreateDates(username);
        }
    }
}
