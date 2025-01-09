// Building.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// Represents a building that contains a collection of elevators.
    /// Handles elevator management and floor-related operations.
    /// </summary>
    public class Building : IBuilding
    {
        private readonly List<IElevatorControl> elevators;
        public int NumberOfFloors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="totalFloors">The total number of floors in the building.</param>
        /// <param name="elevators">The collection of elevators in the building.</param>
        public Building(int numberOfFloors, IEnumerable<IElevatorControl> elevators)
        {
            NumberOfFloors = numberOfFloors;
            this.elevators = elevators.ToList();
        }

        public IEnumerable<IElevatorControl> GetAllElevators() => elevators;
        public bool IsValidFloor(int floor) => floor >= 1 && floor <= NumberOfFloors;
    }
}
