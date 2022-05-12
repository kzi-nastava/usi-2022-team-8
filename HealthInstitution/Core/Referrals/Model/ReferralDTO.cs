using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals.Model
{
    public class ReferralDTO
    {
        public ReferralType Type { get; set; }
        public Doctor PrescribedBy { get; set; }
        public Doctor? ReferredDoctor { get; set; }
        public SpecialtyType? ReferredSpecialty { get; set; }

        public ReferralDTO(ReferralType type, Doctor prescribedBy, Doctor? referredDoctor, SpecialtyType? referredSpecialty)
        {
            this.Type = type;
            this.PrescribedBy = prescribedBy;
            this.ReferredDoctor = referredDoctor;
            this.ReferredSpecialty = referredSpecialty;
        }
    }
}
