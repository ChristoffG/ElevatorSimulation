# Elevator Simulation Project

## Overview
The **Elevator Simulation** is a C#-based console application designed to simulate the behavior and management of elevators within a building. It provides functionalities such as handling elevator requests, managing passenger loads, simulating elevator movement, and displaying real-time elevator status. The project adheres to SOLID principles and emphasizes modularity, extensibility, and testability.

## Features
- **Elevator Management**: Simulates elevators with different statuses, directions, capacities, and types.
- **Building Setup**: Configurable number of floors and elevators.
- **Dispatcher Logic**: Dynamically assigns the nearest available elevator to requests.
- **Passenger Management**: Handles adding and removing passengers while respecting capacity constraints.
- **User Input Handling**: Processes user commands to call elevators or exit the application.
- **Real-Time Display**: Updates elevator statuses dynamically in the console.

## Architecture
The project is built with a clean architecture in mind:
- **Interfaces**: Define contracts for core functionalities (e.g., elevator control, dispatcher logic, and building operations).
- **Abstract Classes**: Provide reusable base implementations for elevators.
- **Concrete Classes**: Implement specific behaviors for different elevator types and components (e.g., `StandardElevator`, `Building`).
- **Separation of Concerns**: The simulation logic, user input, and display are modular and independent.

## Technologies Used
- **Programming Language**: C# (.NET Core 6.0 or higher)
- **Testing Framework**: xUnit
- **Mocking Library**: Moq (for mocking interfaces in unit tests)
- **Development Tools**: Visual Studio Code, .NET CLI

## Setup and Installation
### Prerequisites
1. .NET SDK (version 6.0 or higher)
2. Visual Studio Code (or any C#-supported IDE)
3. Git (optional, for cloning the repository)

### Steps
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd ElevatorSimulation
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run --project ElevatorSimulation
   ```

## Usage
1. The application starts by displaying the status of all elevators.
2. Enter commands to interact with the simulation:
   - **Call an Elevator**: `call <from_floor> <to_floor> <passengers>`
     Example: `call 3 8 5`
   - **Exit the Application**: `exit`
3. The elevators will dynamically respond to user commands, and their statuses will be updated in real time.

## Testing
### Running Unit Tests
The project includes unit tests to ensure the reliability of key components. To run the tests:
1. Navigate to the test project directory:
   ```bash
   cd ElevatorSimulation.Tests
   ```
2. Run the tests:
   ```bash
   dotnet test
   ```

### Mocking with Moq
The `Moq` library is used to mock dependencies in unit tests, ensuring isolation and testing specific behaviors of components.

## Example Test Case
The following unit test checks the `AddPassengers` method:
```csharp
[Fact]
public void AddPassengers_ShouldAdd_WhenCapacityAllows()
{
    // Arrange
    var elevator = new StandardElevator(1);

    // Act
    var result = elevator.AddPassengers(5);

    // Assert
    Assert.True(result);
    Assert.Equal(5, elevator.CurrentPassengers);
}
```

## Future Enhancements
- Implement more elevator types (e.g., freight, glass elevators).
- Add support for maintenance mode and service prioritization.
- Enhance the user interface with graphical elements or a web front-end.
- Integrate with IoT devices for real-world elevator simulation.

## Contribution
1. Fork the repository.
2. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature
   ```
3. Commit your changes and push to your fork.
4. Submit a pull request for review.

## License
This project is licensed under the MIT License. See `LICENSE` for details.

## Contact
For questions or feedback, please contact:
- **Email**: [christoff.gouws@gmail.com]
- **GitHub**: [ChristoffG]

---
Thank you for using the Elevator Simulation Project! We hope you find it useful.

