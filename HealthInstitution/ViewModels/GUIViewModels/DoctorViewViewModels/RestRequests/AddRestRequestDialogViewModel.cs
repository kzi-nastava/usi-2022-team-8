using HealthInstitution.Commands.DoctorCommands.RestRequests;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests
{
    public class AddRestRequestDialogViewModel : ViewModelBase
    {
        private DateTime _selectedDateTime = DateTime.Now;

        public DateTime SelectedDateTime
        {
            get
            {
                return _selectedDateTime;
            }
            set
            {
                _selectedDateTime = value;
                OnPropertyChanged(nameof(SelectedDateTime));
            }
        }
        public DateTime GetStartDate()
        {
            string formatDate = SelectedDateTime.Date.ToString();
            formatDate = formatDate;
            DateTime.TryParse(formatDate, out var dateTime);
            return dateTime;
        }

        private object _urgencyChoice = "0";

        public object UrgencyChoice
        {
            get
            {
                return _urgencyChoice;
            }
            set
            {
                _urgencyChoice = value;
                OnPropertyChanged(nameof(UrgencyChoice));
            }
        }

        public bool GetUrgencyChoice()
        {
            return Convert.ToInt32(UrgencyChoice as string) == 0;
        }

        private string _daysDuration;

        public string DaysDuration
        {
            get
            {
                return _daysDuration;
            }
            set
            {
                _daysDuration = value;
                OnPropertyChanged(nameof(DaysDuration));
            }
        }

        public int GetDaysDuration()
        {
            return Int32.Parse(DaysDuration);
        }

        private string _reason;

        public string Reason
        {
            get
            {
                return _reason;
            }
            set
            {
                _reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }

        public string GetReason()
        {
            return Reason;
        }

        public ICommand CreateRestRequestCommand { get; }

        public AddRestRequestDialogViewModel(Doctor doctor)
        {
            CreateRestRequestCommand = new AddRestRequestDialogCommand(this, doctor);
        }
    }
}
