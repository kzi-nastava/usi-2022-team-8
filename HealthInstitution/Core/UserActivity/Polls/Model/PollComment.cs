using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Model
{
    public class PollComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public Doctor? ForDoctor { get; set; }

        public PollComment(int id, string comment, Doctor forDoctor)
        {
            Id = id;
            Comment = comment;
            ForDoctor = forDoctor;
        }

        public PollComment(PollCommentDTO pollCommentDTO)
        {
            this.Comment = pollCommentDTO.Comment;
            this.ForDoctor = pollCommentDTO.ForDoctor;
        }
    }
}
