using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.Core.Polls.Model
{
    internal class Grade
    {
        public String Comment { get; set; }
        public int Rate { get; set; }

        public Grade(string comment, int rate)
        {
            this.Comment = comment;
            this.Rate = rate;
        }
    }
}
