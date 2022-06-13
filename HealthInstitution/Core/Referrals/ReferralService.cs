using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals
{
    public class ReferralService : IReferralService
    {
        IReferralRepository _referralRepository;
        public ReferralService(IReferralRepository referralRepository)
        {
            _referralRepository = referralRepository;
        }

        public List<Referral> GetAll()
        {
            return _referralRepository.GetAll();
        }

        public Referral Add(ReferralDTO referralDTO)
        {
            Referral referral = new Referral(referralDTO);
            return _referralRepository.Add(referral);
        }

        public void Update(int id, ReferralDTO referralDTO)
        {
            Referral referral = new Referral(referralDTO);
            _referralRepository.Update(id, referral);
        }

        public void Delete(int id)
        {
            _referralRepository.Delete(id);
        }
    }
}
