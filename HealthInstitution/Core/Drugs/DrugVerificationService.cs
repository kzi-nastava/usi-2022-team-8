using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    class DrugVerificationService
    {
        private DrugRepository _drugRepository = DrugRepository.GetInstance();
        public void Accept(Drug drug)
        {
            drug.State = DrugState.Accepted;
            _drugRepository.DrugById[drug.Id] = drug;
            _drugRepository.Save();
        }

        public void Reject(Drug drug, string rejectionReason)
        {
            drug.State = DrugState.Rejected;
            drug.RejectionReason = rejectionReason;
            _drugRepository.DrugById[drug.Id] = drug;
            _drugRepository.Save();
        }
    }
}
