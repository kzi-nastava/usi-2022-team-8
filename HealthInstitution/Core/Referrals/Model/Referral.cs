using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Referrals.Model;

public class Referral
{
    public int id { get; set; }
    public ReferralType type { get; set; }
    public Doctor prescribedBy { get; set; }
    public Doctor referredDoctor { get; set; }
    public SpecialtyType referredSpecialty { get; set; }

    public Referral(int id, ReferralType type, Doctor prescribedBy, Doctor referredDoctor, SpecialtyType referredSpecialty)
    {
        this.id = id;
        this.type = type;
        this.prescribedBy = prescribedBy;
        this.referredDoctor = referredDoctor;
        this.referredSpecialty = referredSpecialty;
    }
}

public enum ReferralType
{
    SpecificDoctor,
    Specialty
}