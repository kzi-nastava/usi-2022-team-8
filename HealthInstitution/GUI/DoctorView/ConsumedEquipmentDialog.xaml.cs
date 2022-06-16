using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.ConsumedEquipment;
using System.Windows;
using System.Windows.Controls;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for ConsumedEquipmentDialog.xaml
    /// </summary>
    public partial class ConsumedEquipmentDialog : Window
    {
        public ConsumedEquipmentDialog(Room room)
        {
            InitializeComponent();
            DataContext = new ConsumedEquipmentDialogViewModel(room);
        }
    }
}