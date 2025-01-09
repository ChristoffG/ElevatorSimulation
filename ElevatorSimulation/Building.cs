// Building.cs
namespace ElevatorSimulation
{
    public class Building : IBuilding
    {
        private readonly List<IElevatorControl> elevators;
        public int NumberOfFloors { get; }

        public Building(int numberOfFloors, IEnumerable<IElevatorControl> elevators)
        {
            NumberOfFloors = numberOfFloors;
            this.elevators = elevators.ToList();
        }

        public IEnumerable<IElevatorControl> GetAllElevators() => elevators;
        public bool IsValidFloor(int floor) => floor >= 1 && floor <= NumberOfFloors;
    }
}
