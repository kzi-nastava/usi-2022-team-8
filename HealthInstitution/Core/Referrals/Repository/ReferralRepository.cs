using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Referrals.Repository
{
    internal class ReferralRepository
    {
        public string fileName { get; set; }
        private int maxId;
        public List<Referral> referrals { get; set; }
        public Dictionary<int, Referral> referralById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private ReferralRepository(string fileName) 
        {
            this.maxId = 0;
            this.fileName = fileName;
            this.referrals = new List<Referral>();
            this.referralById = new Dictionary<int, Referral>();
            this.LoadReferrals(); 
        }
        private static ReferralRepository instance = null;
        public static ReferralRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new ReferralRepository(@"..\..\..\Data\JSON\referrals.json");
                }
                return instance;
            }
        }
        public void LoadReferrals()
        {
            Dictionary<string, Doctor> doctorByUsername = DoctorRepository.GetInstance().doctorsByUsername;
            var referrals = JArray.Parse(File.ReadAllText(fileName));
            //var referrals = JsonSerializer.Deserialize<List<Referral>>(File.ReadAllText(@"..\..\..\Data\JSON\referrals.json"), options);
            foreach (var referral in referrals)
            {
                ReferralType referralType;
                Enum.TryParse(referral["type"].ToString(), out referralType);
                SpecialtyType specialtyType;
                Enum.TryParse(referral["referredSpecialty"].ToString(), out specialtyType);
                Referral referralTemp = new Referral((int)referral["id"],
                                                       referralType,
                                                       doctorByUsername[(string)referral["prescribedBy"]],
                                                       doctorByUsername[(string)referral["referredDoctor"]],
                                                       specialtyType);
                if (referralTemp.id > maxId)
                {
                    maxId = referralTemp.id;
                }

                this.referrals.Add(referralTemp);
                this.referralById[referralTemp.id] = referralTemp;
            }
        }
        public List<dynamic> ShortenReferral()
        {
            List<dynamic> reducedReferrals = new List<dynamic>();
            foreach (var referral in this.referrals)
            {
                reducedReferrals.Add(new
                {
                    id=referral.id,
                    type=referral.type,
                    prescribedBy=referral.prescribedBy.username,
                    referredDoctor=referral.referredDoctor.username,
                    referredSpecialty=referral.referredSpecialty
                });
            }
            return reducedReferrals;
        }
        public void SaveReferrals()
        {

            var allReferrals = JsonSerializer.Serialize(ShortenReferral(), options);
            File.WriteAllText(this.fileName, allReferrals);
        }

        public List<Referral> GetReferrals()
        {
            return this.referrals;
        }

        public Referral GetReferralById(int id)
        {
            if (referralById.ContainsKey(id))
                return referralById[id];
            return null;
        }

        public void AddReferral(ReferralType type, Doctor prescribedBy, Doctor referredDoctor, SpecialtyType referredSpecialty)
        {
            this.maxId++;
            int id = this.maxId;
            Referral referral = new Referral(id, type, prescribedBy, referredDoctor, referredSpecialty);
            this.referrals.Add(referral);
            this.referralById[id] = referral;
            SaveReferrals();
        }

        public void UpdateReferral(int id, ReferralType type, Doctor prescribedBy, Doctor referredDoctor, SpecialtyType referredSpecialty)
        {
            Referral referral = GetReferralById(id);
            referral.prescribedBy = prescribedBy;
            referral.referredDoctor = referredDoctor;
            referral.referredSpecialty = referredSpecialty;
            referralById[id] = referral;
            SaveReferrals();
        }

        public void DeleteReferral(int id)
        {
            Referral referral = GetReferralById(id);
            this.referrals.Remove(referral);
            this.referralById.Remove(referral.id);
            SaveReferrals();
        }
    }
}
