namespace StudentRegistrationProgram
{
    public static class ConsoleHelper
    {
        public static void WriteColoredLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteTabbed(string text, int number)
        {
            Console.Write(text + new string(' ',  Math.Max(number - text.Length, 0)));
        }


        public static string Ask(string askMessage)
        {
            Console.WriteLine(askMessage);

            return Console.ReadLine();

        }

        public static string AskKey()
        {
            return (Console.ReadKey(true)).KeyChar.ToString();

        }

        public static string AskKey(string message)
        {
            Console.WriteLine(message);
            return (Console.ReadKey(true)).KeyChar.ToString();
        }
    }
}
