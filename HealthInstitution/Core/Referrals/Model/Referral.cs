using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Referrals.Model;

public class Referral
{
    public int Id { get; set; }
    public ReferralType type { get; set; }
    public Doctor prescribedBy { get; set; }
    public Doctor referredDoctor { get; set; }
    public SpecialtyType referredSpecialty { get; set; }
}

public enum ReferralType
{
    SpecificDoctor,
    Specialty
}