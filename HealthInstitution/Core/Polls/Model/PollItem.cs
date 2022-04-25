using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls.Model
{
    internal class PollItem
    {
        public String name { get; set; }
        public List<Grade> grades { get; set; }

        public PollItem(string name, List<Grade> grades)
        {
            this.name = name;
            this.grades = grades;
        }
    }
}
