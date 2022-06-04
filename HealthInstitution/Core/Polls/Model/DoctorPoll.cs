using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Polls.Model
{
    public class DoctorPoll : Poll
    {
        public String AdditionalComment { get; set; }

        public Doctor Doctor { get; set; }

        public DoctorPoll(List<PollItem> items, string additionalComment, Doctor doctor) : base(items)
        {
            this.AdditionalComment = additionalComment;
            this.Doctor = doctor;
        }
    }
}
