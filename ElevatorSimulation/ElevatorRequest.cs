// ElevatorRequest.cs
namespace ElevatorSimulation
{
    /// <summary>
    /// Represents a request to an elevator with the details of the origin floor, destination floor, and number of passengers.
    /// </summary>
    /// <summary>
    /// Initializes a new instance of the <see cref="ElevatorRequest"/> class with the specified floor and passenger details.
    /// </summary>
    /// <param name="fromFloor">The floor from which the elevator request originates.</param>
    /// <param name="toFloor">The destination floor of the elevator request.</param>
    /// <param name="passengers">The number of passengers making the request.</param>
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
