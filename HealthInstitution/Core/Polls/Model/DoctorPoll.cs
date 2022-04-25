using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Polls.Model
{
    internal class DoctorPoll : Poll
    {
        public String additionalComment { get; set; }

        public Doctor doctor { get; set; }

        public DoctorPoll(List<PollItem> items, string additionalComment, Doctor doctor) : base(items)
        {
            this.additionalComment = additionalComment;
            this.doctor = doctor;
        }
    }
}
