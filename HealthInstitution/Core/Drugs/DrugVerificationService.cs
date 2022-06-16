using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    public class DrugVerificationService : IDrugVerificationService
    {
        IDrugRepository _drugRepository;
        public DrugVerificationService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }
        public void Accept(Drug drug)
        {
            _drugRepository.Accept(drug);
        }
        public void Reject(Drug drug, string rejectionReason)
        {
            _drugRepository.Reject(drug, rejectionReason);
        }
        public string ReasonForRejection(Drug drug)
        {
            return drug.RejectionReason;
        }
    }
}
