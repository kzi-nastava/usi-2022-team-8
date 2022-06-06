using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IRecommendedSchedulingService
    {
        public bool FindFirstFit(RecommendedSchedulingDTOs firstFitDTO);
        public DateTime GenerateFitDateTime(int minHour, int minMinutes);
        public List<Examination> FindClosestFit(ClosestFitDTO closestFitDTO);
        public DateTime IncrementFit(DateTime fit, int maxHour, int maxMinutes, int minHour, int minMinutes);

    }
}
