using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentRegistrationProgram
{
    public class CitySeeder
    {
        private AppDbContext context;

        public CitySeeder(AppDbContext context)
        {
            this.context = context;
        }
        public void PopulateCitiesFromJson(string fileName)
        {
            if (context.Cities.Count() == 2113)
            {
                // For performance: if all the cities from the Json file are already here, we don't go further.
                return;
            }

            string filePath = FindFileInDirectoryOrParents(fileName);

            if (filePath == null)
                throw new FileNotFoundException("Hittade inte Json", fileName);

            var jsonData = File.ReadAllText(filePath);
            var citiesFromJson = JsonSerializer.Deserialize<List<List<string>>>(jsonData);

            if (citiesFromJson == null || !citiesFromJson.Any())
                return;

            var cityList = citiesFromJson
                .Select(data => new City
                {
                    Name = data[5],         // Equivalent for "Tätort"
                    Municipality = data[3],  // English for "kommun"
                    County = data[5]       // English for "Län"
                })
                .ToList();

            foreach (var city in cityList)
            {
                if (!context.Cities.Any(c => c.Name == city.Name && c.Municipality == city.Municipality && c.County == city.County))
                {
                    context.Cities.Add(city);
                }
            }

            context.SaveChanges();
        }

        private string FindFileInDirectoryOrParents(string fileName)
        {
            string[] directoriesToCheck = { Directory.GetCurrentDirectory(), "..", "../..", "../../.." };

            foreach (var dir in directoriesToCheck)
            {
                string fullPath = Path.Combine(dir, fileName);
                if (File.Exists(fullPath))
                {
                    return Path.GetFullPath(fullPath);
                }
            }

            return null;
        }
    }
}
