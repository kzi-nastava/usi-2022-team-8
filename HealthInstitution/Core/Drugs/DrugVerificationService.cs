using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    internal static class DrugVerificationService
    {
        private static DrugRepository s_drugRepository = DrugRepository.GetInstance();

        public static void Accept(Drug drug)
        {
            s_drugRepository.Accept(drug);
        }
        public static void Reject(Drug drug, string rejectionReason)
        {
            s_drugRepository.Reject(drug, rejectionReason);
        }
        public static string ReasonForRejection(Drug drug)
        {
            return drug.RejectionReason;
        }
    }
}
