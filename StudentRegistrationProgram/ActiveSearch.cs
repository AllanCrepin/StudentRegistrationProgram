

namespace StudentRegistrationProgram
{
    public class ActiveSearch
    {
            public List<T> GenericSearch<T>(
                string promptMessage,
                Func<string, List<T>> getClosestMatches,
                Action<List<T>> displayMatches)
            {
                string query = "";
                List<T> closestMatches = new List<T>();

                Console.CursorVisible = false;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(promptMessage);
                    Console.WriteLine(query);

                    if (closestMatches.Count > 1)
                    {
                        displayMatches(closestMatches);
                        ConsoleHelper.WriteColoredLine(ConsoleColor.DarkGray, "[Enter] för att bekräfta eller ange ID.");
                    }
                    else
                    {
                        displayMatches(closestMatches);
                        ConsoleHelper.WriteColoredLine(ConsoleColor.DarkGray, "[Enter] för att bekräfta.");
                    }

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                        return closestMatches;

                    if (key.Key == ConsoleKey.Backspace && query.Length > 0)
                        query = query.Substring(0, query.Length - 1);
                    else if (key.Key != ConsoleKey.Backspace)
                        query += key.KeyChar.ToString();

                    closestMatches = getClosestMatches(query);
                }
            }

        
    }
}
