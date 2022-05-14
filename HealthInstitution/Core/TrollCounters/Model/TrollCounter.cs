using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.TrollCounters.Model;

public class TrollCounter
{
    public string Username { get; set; }
    public List<DateTime> CreateDates { get; set; }
    public List<DateTime> EditDeleteDates { get; set; }

    public TrollCounter(string username, List<DateTime> createDates, List<DateTime> editDeleteDates)
    {
        this.Username = username;
        this.CreateDates = createDates;
        this.EditDeleteDates = editDeleteDates;
    }

    public TrollCounter(string username)
    {
        this.Username = username;
        this.EditDeleteDates = new List<DateTime>();
        this.CreateDates = new List<DateTime>();
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
        this.CheckDates(this.CreateDates);
        if (CreateDates.Count > 9) throw new Exception("Montly quota spent");
        this.CreateDates.Add(date);
    }

    public void AppendEditDeleteDates(DateTime date)
    {
        this.CheckDates(this.EditDeleteDates);

        this.EditDeleteDates.Add(date);
    }
}