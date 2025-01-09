// StandardElevator.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// Represents a standard elevator with basic functionality such as moving between floors,
    /// handling passenger addition and removal, and updating the elevator's current state.
    /// Inherits from <see cref="BaseElevator"/> and provides a specific implementation
    /// for a standard elevator.
    /// </summary>
    public class StandardElevator : BaseElevator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardElevator"/> class.
        /// This constructor sets the elevator's ID and default capacity and type.
        /// </summary>
        /// <param name="id">The unique identifier for the elevator.</param>
        public StandardElevator(int id) : base(id, 10, ElevatorType.Standard) { }
    }
}
