using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals
{
    public static class ReferralService
    {
        static ReferralRepository s_referralRepository = ReferralRepository.GetInstance();

        public static List<Referral> GetAll()
        {
            return s_referralRepository.GetAll();
        }

        public static Referral Add(ReferralDTO referralDTO)
        {
            Referral referral = new Referral(referralDTO);
            return s_referralRepository.Add(referral);
        }

        public static void Update(int id, ReferralDTO referralDTO)
        {
            Referral referral = new Referral(referralDTO);
            s_referralRepository.Update(id, referral);
        }

        public static void Delete(int id)
        {
            s_referralRepository.Delete(id);
        }
    }
}
