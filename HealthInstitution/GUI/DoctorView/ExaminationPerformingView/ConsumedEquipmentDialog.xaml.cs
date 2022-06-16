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
        IRoomService _roomService;
        IEquipmentService _equipmentService;
        Room _selectedRoom;
        public ConsumedEquipmentDialog(IRoomService roomService, IEquipmentService equipmentService)
        {
            InitializeComponent();
            _roomService = roomService;
            _equipmentService = equipmentService;
        }

        public void SetSelectedRoom(Room room)
        {
            _selectedRoom = room;
            DataContext = new ConsumedEquipmentDialogViewModel(room,_roomService,_equipmentService);
        }
    }
}