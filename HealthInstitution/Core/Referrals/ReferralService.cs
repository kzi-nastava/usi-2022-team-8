using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals
{
    class ReferralService
    {
        private ReferralRepository _referralRepository = ReferralRepository.GetInstance();
        public Referral Add(ReferralDTO referralDTO)
        {
            _referralRepository.maxId++;
            int id = _referralRepository.maxId;
            ReferralType type = referralDTO.Type;
            Doctor prescribedBy = referralDTO.PrescribedBy;
            Doctor? referredDoctor = referralDTO.ReferredDoctor;
            SpecialtyType? referredSpecialty = referralDTO.ReferredSpecialty;

            Referral referral = new Referral(id, type, prescribedBy, referredDoctor, referredSpecialty, true);
            _referralRepository.Referrals.Add(referral);
            _referralRepository.ReferralById[id] = referral;
            _referralRepository.Save();

            return referral;
        }

        public void Update(int id, ReferralDTO referralDTO)
        {
            Referral referral = _referralRepository.GetById(id);
            referral.PrescribedBy = referralDTO.PrescribedBy;
            referral.ReferredDoctor = referralDTO.ReferredDoctor;
            referral.ReferredSpecialty = referralDTO.ReferredSpecialty;
            _referralRepository.ReferralById[id] = referral;
            _referralRepository.Save();
        }

        public void Delete(int id)
        {
            Referral referral = _referralRepository.GetById(id);
            _referralRepository.Referrals.Remove(referral);
            _referralRepository.ReferralById.Remove(referral.Id);
            _referralRepository.Save();
        }
    }
}
