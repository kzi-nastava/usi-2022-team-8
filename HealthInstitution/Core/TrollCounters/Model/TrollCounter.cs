using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.TrollCounters.Model;

public class TrollCounter
{
    public string username { get; set; }
    public List<DateTime> createDates { get; set; }
    public List<DateTime> editDeleteDates { get; set; }

    public TrollCounter(string username, List<DateTime> createDates, List<DateTime> editDeleteDates)
    {
        this.username = username;
        this.createDates = createDates;
        this.editDeleteDates = editDeleteDates;
    }

    public TrollCounter(string username)
    {
        this.username = username;
        this.editDeleteDates = new List<DateTime>();
        this.createDates = new List<DateTime>();
    }

    public TrollCounter()
    { }

    private void CheckDates(List<DateTime> dates)
    {
        foreach (DateTime date in dates.ToList())
        {
            if (date.AddDays(30) < DateTime.Today)
                dates.Remove(date);
        }
    }

    public void AppendCreateDates(DateTime date)
    {
        this.CheckDates(this.createDates);
        if (createDates.Count > 9) throw new Exception("Montly quota spent");
        this.createDates.Add(date);
    }

    public void AppendEditDeleteDates(DateTime date)
    {
        this.CheckDates(this.editDeleteDates);
        //if (createDates.Count > 5) throw new Exception("Montly quota spent");
        this.editDeleteDates.Add(date);
    }
}