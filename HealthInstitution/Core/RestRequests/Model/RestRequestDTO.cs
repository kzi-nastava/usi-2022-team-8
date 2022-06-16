using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequests.Model
{ 
    public class RestRequestDTO
    {
        public Doctor Doctor { get; set; }
        public String Reason { get; set; }
        public DateTime StartDate { get; set; }
        public int DaysDuration { get; set; }
        public RestRequestState State { get; set; }
        public bool IsUrgent { get; set; }

        public string RejectionReason { get; set; }

        public RestRequestDTO(Doctor doctor, string reason, DateTime startDate, int daysDuration, RestRequestState state, bool isUrgent, string rejectionReason)
        {
            Doctor = doctor;
            Reason = reason;
            StartDate = startDate;
            DaysDuration = daysDuration;
            State = state;
            IsUrgent = isUrgent;
            RejectionReason = rejectionReason;
        }

    }
}
