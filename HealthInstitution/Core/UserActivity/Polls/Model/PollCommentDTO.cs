using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Model
{
    public class PollCommentDTO
    {
        public string Comment { get; set; }
        public Doctor? ForDoctor { get; set; }

        public PollCommentDTO(string comment, Doctor? forDoctor)
        {
            Comment = comment;
            ForDoctor = forDoctor;
        }
    }
}
