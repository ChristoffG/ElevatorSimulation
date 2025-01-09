/// <summary>
/// Represents the base class for all types of elevators.
/// Provides core functionality for movement, passenger management, and status tracking.
/// </summary>
namespace ElevatorSimulation
{
    public abstract class BaseElevator : IElevatorControl
    {
        public int Id { get; }
        public int CurrentFloor { get; set; }
        public Direction CurrentDirection { get; protected set; }
        public int CurrentPassengers { get; protected set; }
        public int MaxCapacity { get; }
        public ElevatorStatus Status { get; protected set; }
        public ElevatorType Type { get; }

        private readonly SemaphoreSlim movementSemaphore = new SemaphoreSlim(1, 1);
        private readonly SemaphoreSlim passengerSemaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseElevator"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the elevator.</param>
        /// <param name="maxCapacity">The maximum number of passengers the elevator can hold.</param>
        /// <param name="type">The type of elevator.</param>
        protected BaseElevator(int id, int maxCapacity, ElevatorType type)
        {
            Id = id;
            MaxCapacity = maxCapacity;
            Type = type;
            CurrentFloor = 1;
            CurrentDirection = Direction.Idle;
            Status = ElevatorStatus.Available;
        }

        /// <summary>
        /// Moves the elevator to the specified target floor.
        /// </summary>
        /// <param name="targetFloor">The floor to which the elevator should move.</param>
        public virtual async Task MoveToFloor(int targetFloor)
        {
            await movementSemaphore.WaitAsync();
            try
            {
                CurrentDirection = targetFloor > CurrentFloor ? Direction.Up : Direction.Down;

                while (CurrentFloor != targetFloor)
                {
                    await Task.Delay(3000); // Simulate movement time
                    CurrentFloor += CurrentDirection == Direction.Up ? 1 : -1;
                }

                CurrentDirection = Direction.Idle;
            }
            finally
            {
                movementSemaphore.Release();
            }
        }

        /// <summary>
        /// Checks if the elevator can move to the specified floor.
        /// </summary>
        /// <param name="floor">The floor to check.</param>
        /// <returns><c>true</c> if the elevator can move to the floor; otherwise, <c>false</c>.</returns>
        public virtual bool CanMoveTo(int floor) => Status == ElevatorStatus.Available;

        /// <summary>
        /// Adds the specified number of passengers to the elevator.
        /// </summary>
        /// <param name="count">The number of passengers to add.</param>
        /// <returns><c>true</c> if the passengers were successfully added; otherwise, <c>false</c>.</returns>
        public virtual bool AddPassengers(int count)
        {
            passengerSemaphore.Wait();
            try
            {
                if (!HasCapacityFor(count)) return false;
                CurrentPassengers += count;
                return true;
            }
            finally
            {
                passengerSemaphore.Release();
            }
        }

        /// <summary>
        /// Removes the specified number of passengers from the elevator.
        /// </summary>
        /// <param name="count">The number of passengers to remove.</param>
        /// <returns><c>true</c> if the passengers were successfully removed; otherwise, <c>false</c>.</returns>
        public virtual bool RemovePassengers(int count)
        {
            passengerSemaphore.Wait();
            try
            {
                if (CurrentPassengers < count) return false;
                CurrentPassengers -= count;
                return true;
            }
            finally
            {
                passengerSemaphore.Release();
            }
        }

        /// <summary>
        /// Checks if the elevator has enough capacity for the specified number of passengers.
        /// </summary>
        /// <param name="passengers">The number of passengers to check for.</param>
        /// <returns><c>true</c> if the elevator has enough capacity; otherwise, <c>false</c>.</returns>
        public virtual bool HasCapacityFor(int passengers)
            => CurrentPassengers + passengers <= MaxCapacity;

        /// <summary>
        /// Updates the status of the elevator.
        /// </summary>
        /// <param name="status">The new status of the elevator.</param>
        public void SetStatus(ElevatorStatus status)
        {
            Status = status;
        }
    }
}
