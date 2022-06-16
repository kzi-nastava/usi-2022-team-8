using HealthInstitution.Commands.DoctorCommands.DrugVerification;
using HealthInstitution.Core;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.ViewModels.ModelViewModels.DrugVerification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.DrugVerification
{
    public class DrugsVerificationTableViewModel : ViewModelBase
    {
        public List<Drug> Drugs;

        private ObservableCollection<DrugViewModel> _drugsVM;

        public ObservableCollection<DrugViewModel> DrugsVM
        {
            get
            {
                return _drugsVM;
            }
            set
            {
                _drugsVM = value;
                OnPropertyChanged(nameof(DrugsVM));
            }
        }

        private int _selectedDrugIndex;

        public int SelectedDrugIndex
        {
            get
            {
                return _selectedDrugIndex;
            }
            set
            {
                _selectedDrugIndex = value;
                OnPropertyChanged(nameof(SelectedDrugIndex));
            }
        }

        public void RefreshGrid()
        {
            _drugsVM.Clear();
            Drugs.Clear();
            foreach (Drug drug in _drugService.GetAllCreated())
            {
                Drugs.Add(drug);
                _drugsVM.Add(new DrugViewModel(drug));
            }
        }

        public Drug GetSelectedDrug()
        {
            return Drugs[_selectedDrugIndex];
        }

        public ICommand AcceptDrugCommand { get; }
        public ICommand RejectDrugCommand { get; }
        IDrugService _drugService;
        IDrugVerificationService _drugVerificationService;
        public DrugsVerificationTableViewModel(IDrugService drugService, IDrugVerificationService drugVerificationService)
        {
            _drugService = drugService;
            _drugVerificationService = drugVerificationService;
            AcceptDrugCommand = new AcceptDrugCommand(this, drugVerificationService);
            RejectDrugCommand = new RejectDrugCommand(this);
            _drugsVM = new();
            Drugs = new();
            RefreshGrid();
        }
    }
}
