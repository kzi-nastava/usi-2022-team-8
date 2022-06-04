using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;


namespace HealthInstitution.Core.Polls.Model
{
    public class Poll
    {
        public List<PollItem> Items { get; set; }

        public Poll(List<PollItem> items)
        {
            this.Items = items;
        }
    }
}
