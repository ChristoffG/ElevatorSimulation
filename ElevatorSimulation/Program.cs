// Program.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// The main entry point for the ElevatorSimulation application.
    /// This class sets up and runs the simulation by initializing necessary components,
    /// handling user input, processing elevator requests, and displaying results.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the application.
        /// This method sets up the building, elevators, dispatcher, and other necessary components
        /// before starting the simulation loop. It handles the elevator request processing and 
        /// manages the display of elevator status in the console.
        /// </summary>
        /// <param name="args">Command-line arguments for application configuration (if any).</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
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
