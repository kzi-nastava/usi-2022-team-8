using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.GUI.ManagerView;
namespace HealthInstitution.Core.Rooms
{
    public class RoomService : IRoomService
    {
        IRoomRepository _roomRepository;
        IEquipmentService _equipmentService;
        IRenovationService _renovationService;
        IEquipmentTransferService _equipmentTransferService;
        ISchedulingService _schedulingService;

        public RoomService(IRoomRepository roomRepository, IEquipmentService equipmentService, IRenovationService renovationService,
            IEquipmentTransferService equipmentTransferService, ISchedulingService schedulingService) {
            _roomRepository = roomRepository;
            _equipmentService = equipmentService;
            _renovationService = renovationService;
            _equipmentTransferService = equipmentTransferService;
            _schedulingService = schedulingService;
        }
        public List<Room> GetAll()
        {
            return _roomRepository.GetAll();
        }

        public Room AddRoom(RoomDTO roomDTO)
        {
            Room room = new Room(roomDTO);
            return _roomRepository.AddRoom(room);
        }

        public void Update(int id, RoomDTO roomDTO)
        {
            Room room = new Room(roomDTO);
            _roomRepository.Update(id, room);
        }

        public void Delete(int id)
        {
            _roomRepository.Delete(id);
        }

        public void WriteIn()
        {
            _roomRepository.Save();
        }
        public bool CheckImportantOccurrenceOfRoom(Room room)
        {
            if (_equipmentTransferService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }

            if (_schedulingService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }

            return false;
        }

        public void MoveRoomToRenovationHistory(Room selectedRoom)
        {
            if (_renovationService.CheckRenovationStatusForHistoryDelete(selectedRoom))
            {
                _roomRepository.Delete(selectedRoom.Id);
            }
        }

        public bool ExistsChangedRoomNumber(int number, Room oldRoom)
        {
            int idx = _roomRepository.FindIndexWithRoomNumber(number);
            if (idx >= 0)
            {
                if (_roomRepository.GetAll()[idx] != oldRoom)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddToRoom(int id, Equipment equipment)
        {
            _roomRepository.AddToRoom(id, equipment);
        }

        public List<Room> GetActive()
        {
            return _roomRepository.GetActive();
        }

        public List<Room> GetNotRenovating()
        {
            return _roomRepository.GetNotRenovating();
        }

        public List<Equipment> GetDynamicEquipment(Room room)
        {
            return _roomRepository.GetDynamicEquipment(room);
        }

        public bool RoomNumberIsTaken(int number)
        {
            return _roomRepository.RoomNumberIsTaken(number);
        }

        public List<Equipment> GetAvailableEquipment(Room room)
        {
            return room.AvailableEquipment;
        }

        public List<TableItemEquipment> GetTableItemEquipments()
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();

            foreach (Room room in _roomRepository.GetActive())
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);
                }
            }
            return items;
        }

        public void UpdateEquipmentQuantity(Room room, Equipment equipment)
        { 
            int index = room.AvailableEquipment.FindIndex(eq => eq.Name == equipment.Name && eq.Type == equipment.Type);
            if (index >= 0)
            {
                room.AvailableEquipment[index].Quantity += equipment.Quantity;
                _equipmentService.Delete(equipment.Id);
            }
            else
            {
                room.AvailableEquipment.Add(equipment);
            }          
        }
        public void RemoveEquipmentFrom(Room room)
        {
            List<Equipment> equipments = room.AvailableEquipment;
            foreach (Equipment equipment in equipments)
            {
                _equipmentService.Delete(equipment.Id);
            }
            equipments.Clear();
        }
        public Room? GetFromString(string? roomFromForm)
        {
            return _roomRepository.GetFromString(roomFromForm);
        }
    }
}
