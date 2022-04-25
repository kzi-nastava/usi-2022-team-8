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
        public String comment { get; set; }
        public int rate { get; set; }

        public Grade(string comment, int rate)
        {
            this.comment = comment;
            this.rate = rate;
        }
    }
}
