using HealthInstitution.Core.DIContainer;
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
    /// Interaction logic for DrugsVerificationTable.xaml
    /// </summary>
    public partial class DrugsVerificationTable : Window
    {
        IDrugVerificationService _drugVerificationService;
        IDrugService _drugService;
        public DrugsVerificationTable(IDrugVerificationService drugVerificationService, IDrugService drugService)
        {
            _drugService = drugService;
            _drugVerificationService = drugVerificationService;
            InitializeComponent();
            DataContext = new DrugsVerificationTableViewModel(_drugService, _drugVerificationService);
        }
    }
}
