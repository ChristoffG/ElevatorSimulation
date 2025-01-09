// ConsoleDisplay.cs
namespace ElevatorSimulation
{
    public class ConsoleDisplay : IElevatorDisplay
    {
        private readonly int displayOffset;
        private readonly object consoleLock;

        public ConsoleDisplay(int displayOffset = 0, object? consoleLock = null)
        {
            this.displayOffset = displayOffset;
            this.consoleLock = consoleLock ?? new object();
        }

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
