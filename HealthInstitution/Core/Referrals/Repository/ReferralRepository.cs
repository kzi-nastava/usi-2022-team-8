using HealthInstitution.Core.Referrals.Model;
using System;
using System.Collections.Generic;
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
        public List<Referral> referrals { get; set; }
        public Dictionary<int, Referral> referralById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private ReferralRepository(string fileName) //singleton
        {
            this.fileName = fileName;
            this.referrals = new List<Referral>();
            this.referralById = new Dictionary<int, Referral>();
            this.LoadReferral();
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
    }
}
