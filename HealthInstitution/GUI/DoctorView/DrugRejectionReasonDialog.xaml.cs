using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.DrugVerification;
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
    /// Interaction logic for DrugRejectionReason.xaml
    /// </summary>
    public partial class DrugRejectionReasonDialog : Window
    {
        IDrugVerificationService _drugVerificationService;
        Drug _drug;
        public DrugRejectionReasonDialog(IDrugVerificationService drugVerificationService)
        {
            InitializeComponent();
            _drugVerificationService = drugVerificationService;
        }

        public void SetDrug(Drug drug)
        {
            _drug = drug;
            DataContext = new RejectionReasonDialogViewModel(drug, _drugVerificationService);
        }
    }
}
