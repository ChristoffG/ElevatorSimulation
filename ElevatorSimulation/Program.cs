// Program.cs
namespace ElevatorSimulation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Create a shared console lock
            var consoleLock = new object();

            // Create elevators
            var elevators = Enumerable.Range(1, 4)
                .Select(id => new StandardElevator(id))
                .ToList<IElevatorControl>();

            // Setup building and dependencies
            IBuilding building = new Building(20, elevators);
            IElevatorDispatcher dispatcher = new ElevatorDispatcher(building);
            IElevatorDisplay display = new ConsoleDisplay(0, consoleLock);
            IUserInput userInput = new ConsoleInput(elevators.Count + 3, building);

            // Create and start controller
            var controller = new ElevatorController(dispatcher, display, userInput, building);
            await controller.Start();
        }
    }
}
