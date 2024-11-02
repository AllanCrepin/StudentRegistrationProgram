namespace StudentRegistrationProgram
{
    public class Menu
    {
        public string[] Options { get; private set; }
        public Menu(string[] options)
        {
            Options = options;
        }
        public void Display()
        {
            for (int i = 0; i < Options.Length; i++)
            {
                ConsoleHelper.WriteColoredLine(ConsoleColor.White, $"[{i}] {Options[i]}");
            }
        }

        public int DisplayAndAskInt()
        {
            Display();

            if (!int.TryParse(ConsoleHelper.AskKey(), out int choiceAsInt))
                return -1;
            else
                return choiceAsInt;
        }

    }
}
