using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevatorSimulation;
using Moq;
using Xunit;

namespace ElevatorSimulation.Tests
{
    public class BaseElevatorTests
    {
        [Fact]
        public async Task MoveToFloor_ValidFloor_ChangesCurrentFloorAndDirection()
        {
            // Arrange
            var elevator = new StandardElevator(1);

            // Act
            await elevator.MoveToFloor(5);

            // Assert
            Assert.Equal(5, elevator.CurrentFloor);
            Assert.Equal(Direction.Idle, elevator.CurrentDirection);
        }

        [Fact]
        public async Task MoveToFloor_MovingDown_ChangesDirectionToDown()
        {
            // Arrange
            var elevator = new StandardElevator(1) { CurrentFloor = 10 };

            // Act
            await elevator.MoveToFloor(5);

            // Assert
            Assert.Equal(5, elevator.CurrentFloor);
            Assert.Equal(Direction.Idle, elevator.CurrentDirection);
        }

        [Fact]
        public void AddPassengers_WithinCapacity_ReturnsTrue()
        {
            // Arrange
            var elevator = new StandardElevator(1);

            // Act
            var result = elevator.AddPassengers(5);

            // Assert
            Assert.True(result);
            Assert.Equal(5, elevator.CurrentPassengers);
        }

        [Fact]
        public void AddPassengers_ExceedingCapacity_ReturnsFalse()
        {
            // Arrange
            var elevator = new StandardElevator(1);

            // Act
            var result = elevator.AddPassengers(15); // Max capacity is 10

            // Assert
            Assert.False(result);
            Assert.Equal(0, elevator.CurrentPassengers);
        }

        [Fact]
        public void RemovePassengers_ValidCount_ReturnsTrue()
        {
            // Arrange
            var elevator = new StandardElevator(1);
            elevator.AddPassengers(5);

            // Act
            var result = elevator.RemovePassengers(3);

            // Assert
            Assert.True(result);
            Assert.Equal(2, elevator.CurrentPassengers);
        }

        [Fact]
        public void RemovePassengers_TooMany_ReturnsFalse()
        {
            // Arrange
            var elevator = new StandardElevator(1);
            elevator.AddPassengers(5);

            // Act
            var result = elevator.RemovePassengers(10);

            // Assert
            Assert.False(result);
            Assert.Equal(5, elevator.CurrentPassengers);
        }
    }

    public class BuildingTests
    {
        [Fact]
        public void IsValidFloor_ValidFloor_ReturnsTrue()
        {
            // Arrange
            var elevators = new List<IElevatorControl> { new StandardElevator(1) };
            var building = new Building(20, elevators);

            // Act
            var result = building.IsValidFloor(10);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidFloor_InvalidFloor_ReturnsFalse()
        {
            // Arrange
            var elevators = new List<IElevatorControl> { new StandardElevator(1) };
            var building = new Building(20, elevators);

            // Act
            var result = building.IsValidFloor(25);

            // Assert
            Assert.False(result);
        }
    }

    public class ElevatorDispatcherTests
    {
        [Fact]
        public void GetNearestAvailableElevator_ReturnsClosestElevator()
        {
            // Arrange
            var elevator1 = new Mock<IElevatorControl>();
            elevator1.Setup(e => e.Status).Returns(ElevatorStatus.Available);
            elevator1.Setup(e => e.CurrentFloor).Returns(5);
            elevator1.Setup(e => e.CanMoveTo(It.IsAny<int>())).Returns(true);

            var elevator2 = new Mock<IElevatorControl>();
            elevator2.Setup(e => e.Status).Returns(ElevatorStatus.Available);
            elevator2.Setup(e => e.CurrentFloor).Returns(10);
            elevator2.Setup(e => e.CanMoveTo(It.IsAny<int>())).Returns(true);

            var elevators = new List<IElevatorControl> { elevator1.Object, elevator2.Object };
            var building = new Building(20, elevators);
            var dispatcher = new ElevatorDispatcher(building);

            // Act
            var result = dispatcher.GetNearestAvailableElevator(7);

            // Assert
            Assert.Equal(elevator1.Object, result); // Elevator 1 is closer
        }

        [Fact]
        public void GetNearestAvailableElevator_NoElevatorsAvailable_ReturnsNull()
        {
            // Arrange
            var elevator1 = new Mock<IElevatorControl>();
            elevator1.Setup(e => e.Status).Returns(ElevatorStatus.OutOfService);

            var elevators = new List<IElevatorControl> { elevator1.Object };
            var building = new Building(20, elevators);
            var dispatcher = new ElevatorDispatcher(building);

            // Act
            var result = dispatcher.GetNearestAvailableElevator(7);

            // Assert
            Assert.Null(result);
        }
    }
}
