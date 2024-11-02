
namespace StudentRegistrationProgram
{
    public class UserInterface
    {
        public Menu Menu { get; set; }
        public InputChecker InputChecker { get; set; }
        public StudentService StudentManager { get; set; }
        public CityService CityManager { get; set; }
        public ActiveSearch ActiveSearch { get; set; }

        public ErrorType error = ErrorType.Default;

        public UserInterface(StudentService studentService, CityService cityService)
        {
            Menu = new Menu(new[] { "Registrera en ny student", "Ändra en student", "Lista alla studenter" });

            StudentManager = studentService;
            CityManager = cityService;

            InputChecker = new InputChecker();

            ActiveSearch = new ActiveSearch();
    }
        public void Start()
        {
            while (true)
            {
                DisplayError(error);

                DrawHeaderBar();

                int choiceAsInt = Menu.DisplayAndAskInt();

                switch (choiceAsInt)
                {
                    case (< 0 ):
                        error = ErrorType.MustBeIntWithinRange;
                        break;
                    case (> 3):
                        error = ErrorType.MustBeIntWithinRange;
                        break;
                    case 0:
                        error = ErrorType.NoError;
                        RegisteringDialog();
                        break;
                    case 1:
                        error = ErrorType.NoError;
                        UpdateStudentDialog();
                        break;
                    case 2:
                        error = ErrorType.NoError;
                        ListStudentsDialog();
                        break;
                    default:
                        error = ErrorType.MustBeInt;
                        continue;
                }
            }
        }

        public void DrawHeaderBar()
        {
            ConsoleHelper.WriteColoredLine(ConsoleColor.White, $"Det finns {StudentManager.GetNumberOfStudents()} studenter, i {CityManager.GetNumberOfDifferentCities()} olika orter.");
            ConsoleHelper.WriteColoredLine(ConsoleColor.Blue, "\nVälj ett alternativ:");
        }

        public void DisplayError(ErrorType error)
        {
            Console.Clear();
            switch (error)
            {
                case ErrorType.MustBeInt:
                    ConsoleHelper.WriteColoredLine(ConsoleColor.Red, "Endast siffror tillåtna.");
                    break;
                case ErrorType.MustBeIntWithinRange:
                    ConsoleHelper.WriteColoredLine(ConsoleColor.Red, "Endast siffror i det givna intervallet tillåtna.");
                    break;
                case ErrorType.StringTooShort:
                    ConsoleHelper.WriteColoredLine(ConsoleColor.Red, "Strängen är för kort.");
                    break;
                case ErrorType.NoError:
                        break;
                default:
                    break;
            }
        }

        public void RegisteringDialog()
        {
            while (true)
            {
                DisplayError(error);

                Console.WriteLine("Du har valt att registrera en ny student.");

                string firstName = ConsoleHelper.Ask("Ange förnamn:");
                error = InputChecker.CheckName(firstName);
                if (error != ErrorType.NoError)
                    continue;

                string lastName = ConsoleHelper.Ask("Ange efternamn:");
                error = InputChecker.CheckName(lastName);
                if (error != ErrorType.NoError)
                    continue;

                // Real-time search/Auto-correction based on city database
                List<City> selectedCities = ActiveSearch.GenericSearch<City>(
                    "Ange ort:",
                    query => new List<City> { CityManager.GetClosestCity(query) },
                    cities => cities.ForEach(city => ConsoleHelper.WriteColoredLine(ConsoleColor.DarkGray, city.Name))
                );

                StudentManager.RegisterStudent(firstName, lastName, selectedCities.Single().Name);

                Console.CursorVisible = true;

                break;
            }
        }


        private void ListStudentsDialog()
        {
            var students = StudentManager.ListStudents();

            Console.WriteLine("\nLista över alla studenter:");

            foreach (var student in students)
            {
                ConsoleHelper.WriteTabbed($"ID: {student.StudentId},", 10);
                ConsoleHelper.WriteTabbed($"Namn: {student.FirstName} {student.LastName},", 32);
                Console.Write($"Ort: {student.City}");
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        

        private void UpdateStudentDialog()
        {
            // Real-time search/Auto-correction based on student database
            List<Student> students = ActiveSearch.GenericSearch<Student>(
                "Ange student:",
                query => StudentManager.GetClosestStudents2(query),
                students => students.ForEach(s => ConsoleHelper.WriteColoredLine(ConsoleColor.DarkGray, $"ID: {s.StudentId}, {s.FirstName} {s.LastName}"))
            );

            Student selectedStudent = new Student();
            int idAsInt = 0;
            string choice;

            if(students.Count > 1)
            {
                choice = ConsoleHelper.Ask("Ange ID:");
                if (int.TryParse(choice, out idAsInt) && StudentManager.StudentExists(idAsInt))
                    selectedStudent = StudentManager.GetStudentFromId(idAsInt);
                else
                    Console.WriteLine("Student med angivet ID hittades inte."); return;

            }
            else
            {
                selectedStudent = students.SingleOrDefault();
            }

            Console.WriteLine($"Nuvarande förnamn: {selectedStudent.FirstName}");
            string newFirstName = ConsoleHelper.Ask("Ange nytt förnamn (lämna tomt för att behålla):");

            Console.WriteLine($"Nuvarande efternamn: {selectedStudent.LastName}");
            string newLastName = ConsoleHelper.Ask("Ange nytt efternamn (lämna tomt för att behålla):");

            Console.WriteLine($"Nuvarande ort: {selectedStudent.City}");
            string newCity = ConsoleHelper.Ask("Ange ny ort (lämna tomt för att behålla):");

            bool updated = StudentManager.UpdateStudent(idAsInt, newFirstName, newLastName, newCity);

            if (updated)
                Console.WriteLine("Studentuppgifter uppdaterade.");
        }
    }
}
