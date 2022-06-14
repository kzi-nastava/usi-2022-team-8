using HealthInstitution.Core.Referrals.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals
{
    public interface IReferralService
    {
        public List<Referral> GetAll();
        public Referral Add(ReferralDTO referralDTO);
        public void Update(int id, ReferralDTO referralDTO);
        public void Delete(int id);
    }
}
