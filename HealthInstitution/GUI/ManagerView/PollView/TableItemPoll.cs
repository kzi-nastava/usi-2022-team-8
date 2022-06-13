using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.GUI.ManagerView.PollView
{
    public class TableItemPoll
    {
        public string Question { get; set; }
        public double Avg { get; set; }
        public int NumOf1s { get; set; }
        public int NumOf2s { get; set; }
        public int NumOf3s { get; set; }
        public int NumOf4s { get; set; }
        public int NumOf5s { get; set; }

        public TableItemPoll(string question, double avg, int numOf1s, int numOf2s, int numOf3s, int numOf4s, int numOf5s)
        {
            Question = question;
            Avg = avg;
            NumOf1s = numOf1s;
            NumOf2s = numOf2s;
            NumOf3s = numOf3s;
            NumOf4s = numOf4s;
            NumOf5s = numOf5s;
        }

        public TableItemPoll(string question, double avg, Dictionary<int, int> occurrenceByGrade)
        {
            Question = question;
            Avg = avg;
            NumOf1s = occurrenceByGrade[1];
            NumOf2s = occurrenceByGrade[2];
            NumOf3s = occurrenceByGrade[3];
            NumOf4s = occurrenceByGrade[4];
            NumOf5s = occurrenceByGrade[5];
        }

        
    }
}
