using HealthInstitution.Commands.DoctorCommands.ConsumedEquipment;
using HealthInstitution.Core;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.ConsumedEquipment
{
    public class ConsumedEquipmentDialogViewModel : ViewModelBase
    {
        public Room Room { get; set; }
        public int RoomNo { get; set; }

        private ObservableCollection<Equipment> _equipmentComboBoxItems;

        public ObservableCollection<Equipment> EquipmentComboBoxItems
        {
            get
            {
                return _equipmentComboBoxItems;
            }
            set
            {
                _equipmentComboBoxItems = value;
                OnPropertyChanged(nameof(EquipmentComboBoxItems));
            }
        }
        private int _equipmentComboBoxSelectedIndex;

        public int EquipmentComboBoxSelectedIndex
        {
            get
            {
                return _equipmentComboBoxSelectedIndex;
            }
            set
            {
                _equipmentComboBoxSelectedIndex = value;
                OnPropertyChanged(nameof(EquipmentComboBoxSelectedIndex));
            }
        }
        private string _consumedQuantity;

        public string ConsumedQuantity
        {
            get
            {
                return _consumedQuantity;
            }
            set
            {
                _consumedQuantity = value;
                OnPropertyChanged(nameof(ConsumedQuantity));
            }
        }

        public int GetConsumedQuantity()
        {
            return Int32.Parse(ConsumedQuantity);
        }
        public Equipment GetEquipment()
        {
            return EquipmentComboBoxItems[EquipmentComboBoxSelectedIndex];
        }
        public void LoadEquipmentComboBox()
        {
            EquipmentComboBoxItems = new();
            foreach (Equipment equipment in RoomService.GetDynamicEquipment(Room))
            {
                EquipmentComboBoxItems.Add(equipment);
            }
            EquipmentComboBoxSelectedIndex = 0;
        }

        public ICommand SubmitOneConsumedEquipmentCommand { get; }
        public ICommand FinishConsumedEquipmentCommand { get; }
        public ConsumedEquipmentDialogViewModel(Room room)
        {
            Room = room;
            RoomNo = room.Number;
            LoadEquipmentComboBox();
            SubmitOneConsumedEquipmentCommand = new SubmitOneConsumedEquipmentCommand(this);
            FinishConsumedEquipmentCommand = new FinishConsumedEquipmentCommand(this);
        }
    }
}
