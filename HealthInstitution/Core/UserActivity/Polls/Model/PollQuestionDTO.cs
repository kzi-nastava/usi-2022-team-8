using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Model
{
    public class PollQuestionDTO
    {
        public string Question { get; set; }
        public Doctor? ForDoctor { get; set; }
        public List<int> Grades { get; set; }

        public PollQuestionDTO(string question, Doctor? forDoctor, List<int> grades)
        {
            Question = question;
            ForDoctor = forDoctor;
            Grades = grades;
        }
    }
}
