using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Prescriptions;
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
    /// Interaction logic for AddPrescriptionDialog.xaml
    /// </summary>
    public partial class AddPrescriptionDialog : Window
    {
        MedicalRecord _medicalRecord;
        IDrugService _drugService;
        IMedicalRecordService _medicalRecordService;
        IPrescriptionService _prescriptionService;
        public AddPrescriptionDialog(IDrugService drugService,
                                    IMedicalRecordService medicalRecordService, IPrescriptionService prescriptionService)
        {
            InitializeComponent(); 
            _medicalRecordService = medicalRecordService;
            _drugService = drugService;
            _prescriptionService = prescriptionService;
        }

        public void SetMedicalRecord(MedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
            DataContext = new AddPrescriptionDialogViewModel(medicalRecord,_drugService,_medicalRecordService,_prescriptionService);
        }
    }
}
