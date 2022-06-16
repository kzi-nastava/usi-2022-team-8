using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Model
{
    public class PollQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Doctor? ForDoctor { get; set; }
        public List<int> Grades { get; set; }

        public PollQuestion(int id, string question, Doctor forDoctor, List<int> grades)
        {
            Id = id;
            Question = question;
            ForDoctor = forDoctor;
            Grades = grades;
        }

        public PollQuestion(PollQuestionDTO pollQuestionDTO)
        {
            this.Question = pollQuestionDTO.Question;
            this.ForDoctor = pollQuestionDTO.ForDoctor;
            this.Grades = pollQuestionDTO.Grades;
        }
    }
}
