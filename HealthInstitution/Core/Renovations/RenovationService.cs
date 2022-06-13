using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations
{
    public class RenovationService : IRenovationService
    {
        IRenovationRepository _renovationRepository;
        IRoomService _roomService;
        public RenovationService(IRenovationRepository renovationRepository,
            IRoomService roomService)
        {
            _renovationRepository = renovationRepository;
            _roomService = roomService;
        }
        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public void AddRenovation(RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            _renovationRepository.AddRenovation(renovation);
        }
        public void AddRoomMerger(RoomMergerDTO roomMergerDTO)
        {
            Renovation renovation = new RoomMerger(roomMergerDTO);
            _renovationRepository.AddRenovation(renovation);
        }
        
        public void AddRoomSeparation(RoomSeparationDTO roomSeparationDTO)
        {
            Renovation renovation = new RoomSeparation(roomSeparationDTO);
            _renovationRepository.AddRenovation(renovation);
        }

        public void UpdateRenovation(int id, RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            _renovationRepository.UpdateRenovation(id, renovation);
        }
        public void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO)
        {
            RoomMerger roomMerger = new RoomMerger(roomMergerDTO);
            _renovationRepository.UpdateRoomMerger(id, roomMerger);
        }
        public void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO)
        {
            RoomSeparation roomSeparation = new RoomSeparation(roomSeparationDTO);
            _renovationRepository.UpdateRoomSeparation(id, roomSeparation);
        }
        public void Delete(int id)
        {
            _renovationRepository.Delete(id);
        }

        public void StartRenovation(Room room)
        {
            RoomDTO roomDTO = new RoomDTO(room.Type, room.Number, true);
            _roomService.Update(room.Id, roomDTO);
        }

        public void EndRenovation(Room room)
        {
            RoomDTO roomDTO = new RoomDTO(room.Type, room.Number, false);
            _roomService.Update(room.Id, roomDTO);
        }

        public void StartMerge(Room firstRoom, Room secondRoom, Room mergedRoom)
        {
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, true);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, true);
            _roomService.Update(firstRoom.Id, firstRoomDTO);
            _roomService.Update(secondRoom.Id, secondRoomDTO);
        }

        public void EndMerge(Room firstRoom, Room secondRoom, Room mergedRoom)
        {
            foreach (Equipment equipment in firstRoom.AvailableEquipment)
            {
                mergedRoom.AvailableEquipment.Add(equipment);
            }
            firstRoom.AvailableEquipment.Clear();

            foreach (Equipment equipment in secondRoom.AvailableEquipment)
            {
                _roomService.UpdateEquipmentQuantity(mergedRoom, equipment);
            }
            secondRoom.AvailableEquipment.Clear();

            RoomDTO mergedRoomDTO = new RoomDTO(mergedRoom.Type, mergedRoom.Number, false, true);
            _roomService.Update(mergedRoom.Id, mergedRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, false);
            _roomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, false);
            _roomService.Update(secondRoom.Id, secondRoomDTO);
        }

        public void StartSeparation(Room separationRoom, Room firstRoom, Room secondRoom)
        {
            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, true);
            _roomService.Update(separationRoom.Id, separationRoomDTO);
        }

        public void EndSeparation(Room separationRoom, Room firstRoom, Room secondRoom)
        {
            _roomService.RemoveEquipmentFrom(separationRoom);

            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, false, false);
            _roomService.Update(separationRoom.Id, separationRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, true);
            _roomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, true);
            _roomService.Update(secondRoom.Id, secondRoomDTO);
        }


        public bool CheckRenovationStatusForHistoryDelete(Room room)
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.Room == room)
                {
                    room.IsActive = false;
                    return false;
                }
                if (renovation.IsRoomMerger())
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;
                    if (roomMerger.RoomForMerge == room)
                    {
                        room.IsActive = false;
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckRenovationStatusForRoom(Room room, DateTime date)
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.StartDate > date && renovation.IsRoomSeparation())
                {
                    continue;
                }
                if (renovation.Room == room)
                {
                    return false;
                }

                if (renovation.IsRoomMerger())
                {
                    RoomMerger merger = (RoomMerger)renovation;
                    if (merger.RoomForMerge == room)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }


}
