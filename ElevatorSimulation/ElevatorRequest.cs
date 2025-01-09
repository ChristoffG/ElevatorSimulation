// ElevatorRequest.cs
namespace ElevatorSimulation
{
    public class ElevatorRequest : IElevatorRequest
    {
        public int FromFloor { get; }
        public int ToFloor { get; }
        public int Passengers { get; }

        public ElevatorRequest(int fromFloor, int toFloor, int passengers)
        {
            FromFloor = fromFloor;
            ToFloor = toFloor;
            Passengers = passengers;
        }
    }
}
