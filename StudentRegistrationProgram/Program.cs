namespace StudentRegistrationProgram
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var Context = new AppDbContext())
            {
                Context.Database.EnsureCreated();

                var citySeeder = new CitySeeder(Context);
                citySeeder.PopulateCitiesFromJson("Cities.json");

                StudentApp App = new StudentApp(Context);
                App.Run();
            }
        }
    }
}
