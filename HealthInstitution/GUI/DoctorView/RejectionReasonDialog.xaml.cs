using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for RejectionReasonDialog.xaml
    /// </summary>
    public partial class RejectionReasonDialog : Window
    {
        private Drug _selectedDrug;
        public RejectionReasonDialog(Drug drug)
        {
            this._selectedDrug = drug;
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            String rejectionReason = commentTextBox.Text;
            if (rejectionReason.Trim() == "")
            {
                System.Windows.MessageBox.Show("You have to write a reason for rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DrugVerificationService.Reject(_selectedDrug, rejectionReason);
                System.Windows.MessageBox.Show("Successfull rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
