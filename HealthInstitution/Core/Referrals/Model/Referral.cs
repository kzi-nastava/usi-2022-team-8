using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Referrals.Model;

public class Referral
{
    public int Id { get; set; }
    //public ReferralType Type { get; set; }
    public Doctor PrescribedBy { get; set; }
    public Doctor ReferredDoctor { get; set; }
   //public SpecialtyType ReferredSpecialty { get; set; }

    public Referral(int id, Doctor prescribedBy, Doctor referredDoctor)
    {
        this.Id = id;
        //this.Type = type;
        this.PrescribedBy = prescribedBy;
        this.ReferredDoctor = referredDoctor;
        //this.ReferredSpecialty = referredSpecialty;
    }
}

/*public enum ReferralType
{
    SpecificDoctor,
    Specialty
}*/