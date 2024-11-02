namespace StudentRegistrationProgram
{
    public class StudentApp
    {
        private readonly AppDbContext Context;
        private readonly StudentService studentManager;
        private readonly CityService cityManager;
        public UserInterface Ui { get; set; }
        public StudentApp(AppDbContext context)
        {
            Context = context;

            studentManager = new StudentService(context);
            cityManager = new CityService(context);

            Ui = new UserInterface(studentManager, cityManager);
        }
        public void Run()
        {
            Ui.Start();
        }
    }
}
