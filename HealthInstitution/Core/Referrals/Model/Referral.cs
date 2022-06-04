using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Referrals.Model;

public class Referral
{
    public int Id { get; set; }
    public ReferralType Type { get; set; }
    public Doctor PrescribedBy { get; set; }

    public bool Active { get; set; }   
    public Doctor? ReferredDoctor { get; set; }
    public SpecialtyType? ReferredSpecialty { get; set; }

    public Referral(int id, ReferralType type, Doctor prescribedBy, Doctor? referredDoctor, SpecialtyType? referredSpecialty, bool active)
    {
        this.Id = id;
        this.Type = type;
        this.PrescribedBy = prescribedBy;
        this.ReferredDoctor = referredDoctor;
        this.Active= active;
        this.ReferredSpecialty = referredSpecialty;
    }

    public Referral(ReferralDTO referralDTO)
    {
        this.Type = referralDTO.Type;
        this.PrescribedBy = referralDTO.PrescribedBy;
        this.ReferredDoctor = referralDTO.ReferredDoctor;
        this.ReferredSpecialty = referralDTO.ReferredSpecialty;
        this.Active = true;
    }
}

public enum ReferralType
{
    SpecificDoctor,
    Specialty
}