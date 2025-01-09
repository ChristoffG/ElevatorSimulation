// ConsoleDisplay.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// Provides a console-based implementation for displaying elevator status information.
    /// </summary>
    public class ConsoleDisplay : IElevatorDisplay
    {
        /// <summary>
        /// Specifies the vertical offset for displaying elevator information in the console.
        /// </summary>
        private readonly int displayOffset;
        
        /// <summary>
        /// Ensures thread-safe access to the console during display updates.
        /// </summary>
        private readonly object consoleLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleDisplay"/> class.
        /// </summary>
        /// <param name="displayOffset">The number of lines to offset the display from the top of the console.</param>
        /// <param name="consoleLock">
        /// An optional lock object to synchronize console access. 
        /// If not provided, a new lock object is created.
        /// </param>
        public ConsoleDisplay(int displayOffset = 0, object? consoleLock = null)
        {
            this.displayOffset = displayOffset;
            this.consoleLock = consoleLock ?? new object();
        }

        /// <summary>
        /// Displays the current status of all elevators in the console.
        /// </summary>
        /// <param name="elevators">A collection of elevator states to be displayed.</param>
        public void DisplayElevatorStatus(IEnumerable<IElevatorState> elevators)
        {
            lock (consoleLock)
            {
                var currentPosition = Console.GetCursorPosition();

                Console.SetCursorPosition(0, displayOffset);
                Console.WriteLine("Elevator Status:");
                Console.WriteLine("---------------");

                foreach (var elevator in elevators)
                {
                    if (elevator != null)
                    {
                        Console.WriteLine(
                            $"Elevator {(elevator as IElevatorControl)?.Id} - " +
                            $"Floor: {elevator.CurrentFloor}, " +
                            $"Direction: {elevator.CurrentDirection}, " +
                            $"Passengers: {elevator.CurrentPassengers}/{elevator.MaxCapacity}, " +
                            $"Status: {elevator.Status}");
                    }
                }

                Console.SetCursorPosition(currentPosition.Left, currentPosition.Top);
            }
        }
    }
}
