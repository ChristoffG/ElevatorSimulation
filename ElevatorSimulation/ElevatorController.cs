using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulation
{
    /// <summary>
/// Handles the overall control of the elevator system, 
/// including processing user requests, dispatching elevators, 
/// and updating the elevator status display.
/// </summary>
public class ElevatorController
{
    private readonly IElevatorDispatcher dispatcher; // Responsible for dispatching the nearest available elevator
    private readonly IElevatorDisplay display; // Displays elevator status to the user
    private readonly IUserInput userInput; // Handles user input for elevator requests
    private readonly IBuilding building; // Represents the building and its elevators
    private readonly CancellationTokenSource cancellationToken; // Token to gracefully cancel ongoing tasks
    private readonly List<Task> activeRequests; // Tracks active elevator request tasks
    private readonly SemaphoreSlim semaphore; // Ensures thread-safe access to shared resources

    /// <summary>
    /// Initializes a new instance of the ElevatorController class.
    /// </summary>
    /// <param name="dispatcher">The elevator dispatcher responsible for assigning elevators to requests.</param>
    /// <param name="display">The display used to show elevator statuses.</param>
    /// <param name="userInput">The user input handler for elevator requests.</param>
    /// <param name="building">The building containing the elevators.</param>
    public ElevatorController(
        IElevatorDispatcher dispatcher,
        IElevatorDisplay display,
        IUserInput userInput,
        IBuilding building)
    {
        this.dispatcher = dispatcher;
        this.display = display;
        this.userInput = userInput;
        this.building = building;
        this.cancellationToken = new CancellationTokenSource();
        this.activeRequests = new List<Task>();
        this.semaphore = new SemaphoreSlim(1, 1);
    }

    /// <summary>
    /// Starts the elevator system, continuously listening for user input
    /// and processing elevator requests until the system is terminated.
    /// </summary>
    public async Task Start()
    {
        // Start a background task to update the elevator display periodically
        _ = UpdateDisplay();

        while (!cancellationToken.Token.IsCancellationRequested)
        {
            // Wait for the next user request
            var request = await userInput.GetNextRequest();

            if (request == null)
            {
                // If the request is null, terminate the system
                cancellationToken.Cancel();
                break;
            }

            // Start processing the request asynchronously
            var requestTask = ProcessRequest(request);
            await AddActiveRequest(requestTask);

            // Clean up completed requests from the activeRequests list
            await CleanupCompletedRequests();
        }

        // Ensure all active requests are completed before exiting
        await Task.WhenAll(activeRequests);
    }

    /// <summary>
    /// Adds a new active request to the list of ongoing requests.
    /// </summary>
    /// <param name="request">The task representing the elevator request.</param>
    private async Task AddActiveRequest(Task request)
    {
        await semaphore.WaitAsync(); // Lock the activeRequests list for thread safety
        try
        {
            activeRequests.Add(request); // Add the request to the active list
        }
        finally
        {
            semaphore.Release(); // Release the lock
        }
    }

    /// <summary>
    /// Removes completed tasks from the activeRequests list to free resources.
    /// </summary>
    private async Task CleanupCompletedRequests()
    {
        await semaphore.WaitAsync(); // Lock the activeRequests list for thread safety
        try
        {
            // Remove tasks that have been completed
            activeRequests.RemoveAll(t => t.IsCompleted);
        }
        finally
        {
            semaphore.Release(); // Release the lock
        }
    }

    /// <summary>
    /// Processes an elevator request by dispatching the nearest available elevator,
    /// moving it to the requested floors, and managing passenger loading/unloading.
    /// </summary>
    /// <param name="request">The elevator request to process.</param>
    private async Task ProcessRequest(IElevatorRequest request)
    {
        // Get the nearest available elevator for the requested floor
        var elevator = dispatcher.GetNearestAvailableElevator(request.FromFloor);

        if (elevator == null)
        {
            Console.WriteLine("No available elevators at the moment.");
            return;
        }

        // Check if the elevator can accommodate the requested number of passengers
        if (!elevator.HasCapacityFor(request.Passengers))
        {
            Console.WriteLine($"Elevator {elevator.Id} cannot accommodate {request.Passengers} passengers. " +
                              $"Maximum capacity is {elevator.MaxCapacity}.");
            return;
        }

        try
        {
            // Move the elevator to the source floor
            await elevator.MoveToFloor(request.FromFloor);
            // Add passengers to the elevator
            elevator.AddPassengers(request.Passengers);
            // Move the elevator to the destination floor
            await elevator.MoveToFloor(request.ToFloor);
            // Remove passengers from the elevator
            elevator.RemovePassengers(request.Passengers);
        }
        catch (Exception ex)
        {
            // Log any errors that occur during request processing
            Console.WriteLine($"Error processing request for elevator {elevator.Id}: {ex.Message}");
        }
    }

    /// <summary>
    /// Periodically updates the display to show the current status of all elevators.
    /// </summary>
    private async Task UpdateDisplay()
    {
        while (!cancellationToken.Token.IsCancellationRequested)
        {
            // Display the current status of all elevators in the building
            display.DisplayElevatorStatus(building.GetAllElevators());
            await Task.Delay(500); // Update every 500 milliseconds
        }
    }
}
}
