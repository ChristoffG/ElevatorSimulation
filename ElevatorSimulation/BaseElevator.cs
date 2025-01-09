// BaseElevator.cs
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

        protected BaseElevator(int id, int maxCapacity, ElevatorType type)
        {
            Id = id;
            MaxCapacity = maxCapacity;
            Type = type;
            CurrentFloor = 1;
            CurrentDirection = Direction.Idle;
            Status = ElevatorStatus.Available;
        }

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

        public virtual bool CanMoveTo(int floor) => Status == ElevatorStatus.Available;

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

        public virtual bool HasCapacityFor(int passengers)
            => CurrentPassengers + passengers <= MaxCapacity;

        public void SetStatus(ElevatorStatus status)
        {
            Status = status;
        }
    }
}
