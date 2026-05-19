using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modules.Trips.Domain.Entities;

namespace Modules.Trips.Application.Services
{
    public class TripPlanGenerator
    {
        private List<Place> _candidates = [];
        private Dictionary<int, int> _categoryLimits = [];
        private double _totalTime;
        private List<List<Place>> _results = [];

        public List<List<Place>> GeneratePlans(
            List<Place> candidates,
            Dictionary<int, int> categoryLimits,
            double totalTime)
        {
            _candidates = candidates;
            _categoryLimits = categoryLimits;
            _totalTime = totalTime;
            _results = [];

            Backtrack(
                startIndex: 0,
                currentPlan: [],
                currentTime: 0,
                categoryUsage: []);

            return _results;
        }

        private void Backtrack(
            int startIndex,
            List<Place> currentPlan,
            double currentTime,
            Dictionary<int, int> categoryUsage)
        {
            if (currentPlan.Count > 0)
            {
                _results.Add([.. currentPlan]);
            }

            for (int i = startIndex; i < _candidates.Count; i++)
            {
                var place = _candidates[i];
                var visitTime = place.AverageVisitTime ?? 60;

                // double travelTime = 0;

                // if (currentPlan.Count > 0)
                // {
                //     var lastPlace = currentPlan.Last();

                //     THIS is the routing engine call
                //     travelTime = _routingService.GetTravelTime(
                //         lastPlace.Latitude,
                //         lastPlace.Longitude,
                //         place.Latitude,
                //         place.Longitude);
                // }

                if (!CanAddPlace(place, visitTime, currentTime, categoryUsage))
                    continue;

                AddPlace(place, visitTime, currentPlan, categoryUsage, ref currentTime);

                Backtrack(i + 1, currentPlan, currentTime, categoryUsage);

                RemovePlace(place, visitTime, currentPlan, categoryUsage, ref currentTime);
            }
        }

        private bool CanAddPlace(
            Place place,
            double visitTime,
            double currentTime,
            Dictionary<int, int> categoryUsage)
        {
            if (currentTime + visitTime > _totalTime)
                return false;

            if (_categoryLimits.TryGetValue(place.CategoryId, out int maxLimit))
            {
                if (categoryUsage.GetValueOrDefault(place.CategoryId) >= maxLimit)
                    return false;
            }

            return true;
        }

        private static void AddPlace(
            Place place,
            double visitTime,
            List<Place> currentPlan,
            Dictionary<int, int> categoryUsage,
            ref double currentTime)
        {
            currentPlan.Add(place);
            currentTime += visitTime;

            categoryUsage[place.CategoryId] =
                categoryUsage.GetValueOrDefault(place.CategoryId) + 1;
        }

        private static void RemovePlace(
            Place place,
            double visitTime,
            List<Place> currentPlan,
            Dictionary<int, int> categoryUsage,
            ref double currentTime)
        {
            currentPlan.RemoveAt(currentPlan.Count - 1);
            currentTime -= visitTime;
            categoryUsage[place.CategoryId]--;
        }
    }
}