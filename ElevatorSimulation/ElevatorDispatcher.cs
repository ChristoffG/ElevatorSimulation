// ElevatorDispatcher.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// Responsible for managing the dispatching of elevators 
    /// within the building to handle user requests efficiently.
    /// </summary>
    public class ElevatorDispatcher : IElevatorDispatcher
    {
        private readonly IBuilding building;

        /// <summary>
        /// Initializes a new instance of the ElevatorDispatcher class.
        /// </summary>
        /// <param name="building">The building containing the elevators to manage.</param>
        public ElevatorDispatcher(IBuilding building)
        {
            this.building = building;
        }

        public IElevatorControl GetNearestAvailableElevator(int floor)
        {
            return building.GetAllElevators()
            .Where(e => e.Status == ElevatorStatus.Available && e.CanMoveTo(floor))
            .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
            .FirstOrDefault();
        }
    }
}
