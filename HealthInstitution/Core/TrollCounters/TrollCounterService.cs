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
    public static class TrollCounterService
    {
        static TrollCounterFileRepository s_trollCounterFileRepository = TrollCounterFileRepository.GetInstance();
        public static TrollCounter GetById(string id)
        {
            return s_trollCounterFileRepository.GetById(id);
        }
        public static void Add(string username)
        {
            TrollCounter trollCounter = new TrollCounter(username);
            s_trollCounterFileRepository.Add(trollCounter);
        }
        public static void Delete(string id)
        {
            TrollCounter trollCounter = GetById(id);
            if (trollCounter != null)
            {
                s_trollCounterFileRepository.Delete(trollCounter);
            }
        }
        public static void TrollCheck(string username)
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
        public static void BlockPatient(string username)
        {
            PatientService.ChangeBlockedStatus(username);
        }
        public static void CheckCreateTroll(string username)
        {
            s_trollCounterFileRepository.CheckCreateTroll(username);
        }
        public static void CheckEditDeleteTroll(string username)
        {
            s_trollCounterFileRepository.CheckEditDeleteTroll(username);
        }
        public static void AppendEditDeleteDates(string username)
        {
            s_trollCounterFileRepository.AppendEditDeleteDates(username);
        }
        public static void AppendCreateDates(string username)
        {
            s_trollCounterFileRepository.AppendCreateDates(username);
        }
    }
}
