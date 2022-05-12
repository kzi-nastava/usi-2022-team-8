using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Referrals.Repository
{
    internal class ReferralRepository
    {
        private String _fileName;
        private int _maxId;
        public List<Referral> Referrals { get; set; }
        public Dictionary<int, Referral> ReferralById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private ReferralRepository(string fileName) 
        {
            this._maxId = 0;
            this._fileName = fileName;
            this.Referrals = new List<Referral>();
            this.ReferralById = new Dictionary<int, Referral>();
            this.LoadFromFile(); 
        }
        private static ReferralRepository s_instance = null;
        public static ReferralRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new ReferralRepository(@"..\..\..\Data\JSON\referrals.json");
                }
                return s_instance;
            }
        }

        private Referral Parse(JToken? referral)
        {
            Dictionary<string, Doctor> doctorByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
            ReferralType referralType;
            Enum.TryParse(referral["type"].ToString(), out referralType);
            SpecialtyType specialtyType;
            Enum.TryParse(referral["referredSpecialty"].ToString(), out specialtyType);
            return new Referral((int)referral["id"],
                                      referralType,
                                        doctorByUsername[(string)referral["prescribedBy"]],
                                          doctorByUsername[(string)referral["referredDoctor"]],
                                            specialtyType);
        }
        public void LoadFromFile()
        {
            var referrals = JArray.Parse(File.ReadAllText(_fileName));
            //var referrals = JsonSerializer.Deserialize<List<Referral>>(File.ReadAllText(@"..\..\..\Data\JSON\referrals.json"), _options);
            foreach (var referral in referrals)
            {
                Referral loadedReferral = Parse(referral);   
                if (loadedReferral.Id > _maxId)
                {
                    _maxId = loadedReferral.Id;
                }

                this.Referrals.Add(loadedReferral);
                this.ReferralById[loadedReferral.Id] = loadedReferral;
            }
        }
        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedReferrals = new List<dynamic>();
            foreach (var referral in this.Referrals)
            {
                reducedReferrals.Add(new
                {
                    id=referral.Id,
                    type=referral.Type,
                    prescribedBy=referral.PrescribedBy.Username,
                    referredDoctor=referral.ReferredDoctor.Username,
                    referredSpecialty=referral.ReferredSpecialty
                });
            }
            return reducedReferrals;
        }
        public void Save()
        {
            var allReferrals = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allReferrals);
        }

        public List<Referral> GetAll()
        {
            return this.Referrals;
        }

        public Referral GetById(int id)
        {
            if (ReferralById.ContainsKey(id))
                return ReferralById[id];
            return null;
        }

        public Referral Add(ReferralDTO referralDTO)
        {
            this._maxId++;
            int id = this._maxId;
            ReferralType type = referralDTO.Type;
            Doctor prescribedBy = referralDTO.PrescribedBy;
            Doctor? referredDoctor = referralDTO.ReferredDoctor;
            SpecialtyType? referredSpecialty = referralDTO.ReferredSpecialty;

            Referral referral = new Referral(id, type, prescribedBy, referredDoctor, referredSpecialty);
            this.Referrals.Add(referral);
            this.ReferralById[id] = referral;
            Save();

            return referral;
        }

        public void Update(int id, ReferralDTO referralDTO)
        {
            Referral referral = GetById(id);
            referral.PrescribedBy = referralDTO.PrescribedBy;
            referral.ReferredDoctor = referralDTO.ReferredDoctor;
            referral.ReferredSpecialty = referralDTO.ReferredSpecialty;
            ReferralById[id] = referral;
            Save();
        }

        public void Delete(int id)
        {
            Referral referral = GetById(id);
            this.Referrals.Remove(referral);
            this.ReferralById.Remove(referral.Id);
            Save();
        }
    }
}
