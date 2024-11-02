using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistrationProgram
{
    public class CityService
    {
        private AppDbContext context;

        public CityService(AppDbContext context)
        {
            this.context = context;
        }

        public int GetNumberOfDifferentCities()
        {
            return context.Students.Select(s => s.City).Distinct().Count();
        }

        public string GetClosestCityName(string name)
        {
            var items = context.Cities.ToList();

            var closestMatch = items
                .OrderByDescending(city => StringMetrics.JaroWinklerSimilarity(name, city.Name))
                .FirstOrDefault();

            return closestMatch?.Name;
        }

        public City GetClosestCity(string name)
        {
            var items = context.Cities.ToList();

            var closestMatch = items
                .OrderByDescending(city => StringMetrics.JaroWinklerSimilarity(name, city.Name))
                .FirstOrDefault();

            return closestMatch;
        }

    }
}
