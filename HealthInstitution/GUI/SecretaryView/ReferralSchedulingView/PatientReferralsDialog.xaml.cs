using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for PatientReferralsDialog.xaml
    /// </summary>
    public partial class PatientReferralsDialog : Window
    {
        MedicalRecord _medicalRecord;
        public PatientReferralsDialog(MedicalRecord medicalRecord)
        {
            _medicalRecord=medicalRecord;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            foreach (Referral referral in _medicalRecord.Referrals)
            {
                if(referral.Active)
                    dataGrid.Items.Add(referral);
            }
            dataGrid.Items.Refresh();
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            Referral selectedReferral = (Referral)dataGrid.SelectedItem;
            if (selectedReferral != null)
            {
                AddExaminationWithReferralDialog addExaminationWithReferralDialog = new AddExaminationWithReferralDialog(selectedReferral,_medicalRecord);
                addExaminationWithReferralDialog.ShowDialog();
                dataGrid.SelectedItem = null;
                LoadRows();

            }
        }
    }
}
