// ConsoleInput.cs
namespace ElevatorSimulation
{
    public class ConsoleInput : IUserInput
    {
        private readonly int inputLine;
        private readonly IBuilding building;
        private readonly object consoleLock = new object();

        public ConsoleInput(int inputLine, IBuilding building)
        {
            this.inputLine = inputLine;
            this.building = building;
        }

        public async Task<IElevatorRequest?> GetNextRequest()
        {
            while (true)
            {
                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, inputLine);
                    Console.Write(new string(' ', Console.WindowWidth)); // Clear the line
                    Console.SetCursorPosition(0, inputLine);
                    Console.Write("Enter command (call <from_floor> <to_floor> <passengers> | exit): ");
                    Console.SetCursorPosition(0, inputLine + 1);
                    Console.Write(new string(' ', Console.WindowWidth)); // Clear the line
                    Console.SetCursorPosition(0, inputLine + 1);
                }

                // Read the entire line without interference
                string input = await Task.Run(() => Console.ReadLine() ?? string.Empty);

                if (string.IsNullOrWhiteSpace(input)) continue;

                var parts = input.Split(' ');
                if (parts[0].ToLower() == "exit")
                    return null;

                if (parts[0].ToLower() == "call" && parts.Length == 4)
                {
                    if (int.TryParse(parts[1], out int fromFloor) &&
                        int.TryParse(parts[2], out int toFloor) &&
                        int.TryParse(parts[3], out int passengers))
                    {
                        if (!building.IsValidFloor(fromFloor) || !building.IsValidFloor(toFloor))
                        {
                            lock (consoleLock)
                            {
                                Console.SetCursorPosition(0, inputLine + 2);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(0, inputLine + 2);
                                Console.WriteLine("Invalid floor number");
                            }
                            continue;
                        }

                        return new ElevatorRequest(fromFloor, toFloor, passengers);
                    }
                }

                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, inputLine + 2);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, inputLine + 2);
                    Console.WriteLine("Invalid input format. Please use: call <from_floor> <to_floor> <passengers>");
                }
            }
        }
    }
}
