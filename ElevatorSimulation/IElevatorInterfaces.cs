// IElevatorInterfaces.cs
namespace ElevatorSimulation
{
    public interface IElevatorState
    {
        int CurrentFloor { get; }
        Direction CurrentDirection { get; }
        int CurrentPassengers { get; }
        int MaxCapacity { get; }
        ElevatorStatus Status { get; }
        ElevatorType Type { get; }
    }

    public interface IElevatorMovement
    {
        Task MoveToFloor(int targetFloor);
        bool CanMoveTo(int floor);
    }

    public interface IPassengerManagement
    {
        bool AddPassengers(int count);
        bool RemovePassengers(int count);
        bool HasCapacityFor(int passengers);
    }

    public interface IElevatorControl : IElevatorState, IElevatorMovement, IPassengerManagement
    {
        int Id { get; }
        void SetStatus(ElevatorStatus status);
    }

    public interface IElevatorDispatcher
    {
        IElevatorControl GetNearestAvailableElevator(int floor);
    }

    public interface IBuilding
    {
        int NumberOfFloors { get; }
        IEnumerable<IElevatorControl> GetAllElevators();
        bool IsValidFloor(int floor);
    }

    public interface IElevatorRequest
    {
        int FromFloor { get; }
        int ToFloor { get; }
        int Passengers { get; }
    }

    public interface IElevatorDisplay
    {
        void DisplayElevatorStatus(IEnumerable<IElevatorState> elevators);
    }

    public interface IUserInput
    {
        Task<IElevatorRequest?> GetNextRequest();
    }
}
