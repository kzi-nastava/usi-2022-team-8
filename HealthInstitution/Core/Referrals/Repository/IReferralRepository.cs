using HealthInstitution.Core.Referrals.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals.Repository
{
    public interface IReferralRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Referral> GetAll();
        public Dictionary<int, Referral> GetAllById();
        public Referral GetById(int id);
        public Referral Add(Referral referral);
        public void Update(int id, Referral referralTemp);
        public void Delete(int id);
    }
}
