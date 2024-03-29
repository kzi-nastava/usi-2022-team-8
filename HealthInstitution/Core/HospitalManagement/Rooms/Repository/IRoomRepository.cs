﻿using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms.Repository
{
    public interface IRoomRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Room> GetAll();
        public Dictionary<int, Room> GetAllById();
        public Room GetById(int id);
        public Room AddRoom(Room room);
        public void Update(int id, Room byRoom);
        public void Delete(int id);
        public void AddToRoom(int id, Equipment equipment);
        public List<Room> GetActive();
        public List<Room> GetNotRenovating();
        public List<Equipment> GetDynamicEquipment(Room room);
        public Room? GetFromString(string? roomFromForm);
        public bool RoomNumberIsTaken(int number);
        public int FindIndexWithRoomNumber(int number);
    }
}
