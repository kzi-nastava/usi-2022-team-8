using HealthInstitution.Core.Drugs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    public interface IDrugVerificationService
    {
        public void Accept(Drug drug);
        public void Reject(Drug drug, string rejectionReason);
        public string ReasonForRejection(Drug drug);
    }
}
