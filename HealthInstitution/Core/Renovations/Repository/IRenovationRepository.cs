using HealthInstitution.Core.Renovations.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations.Repository
{
    public interface IRenovationRepository : IRepository<Renovation>
    {
        public Renovation Parse(JToken? renovation);
        public void LoadFromFile();
        public List<dynamic> PrepareForSerialization();
        public void AttachReducedRenovation(List<dynamic> reducedRenovation, Renovation renovation);
        public void Save();
        public List<Renovation> GetAll();
        public Renovation GetById(int id);
        public void AddRenovation(Renovation renovation);
        public void UpdateRenovation(int id, Renovation byRenovation);
        public void UpdateRoomMerger(int id, RoomMerger byRoomMerger);
        public void UpdateRoomSeparation(int id, RoomSeparation byRoomSeparation);
        public void Delete(int id);
    }
}
