using System;
using System.Threading.Tasks;
using ElevatorSimulation.Services;

namespace ElevatorSimulation
{
    class Program
    {
        private static ElevatorService _elevatorService;
        private static bool _running;

        static async Task Main(string[] args)
        {
            _elevatorService = new ElevatorService(numberOfElevators: 3, elevatorCapacity: 10, numberOfFloors: 10);
            _running = true;

            while (_running)
            {
                Console.Clear();
                Console.WriteLine("Elevator Simulation");
                Console.WriteLine("==================");
                Console.WriteLine("1. Call Elevator to Floor");
                Console.WriteLine("2. Request Elevator for Passenger");
                Console.WriteLine("3. Exit");
                Console.WriteLine("4. Show Elevator Status");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CallElevator();
                        break;
                    case "2":
                        await RequestElevator();
                        break;
                    case "3":
                        _running = false;
                        return;
                    case "4":
                        DisplayElevatorStatus();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        static void DisplayElevatorStatus()
        {
            Console.Clear();
            var elevators = _elevatorService.GetElevators();
            foreach (var elevator in elevators)
            {
                Console.WriteLine($"Elevator ID: {elevator.Id}");
                Console.WriteLine($"Current Floor: {elevator.CurrentFloor}");
                Console.WriteLine($"Direction: {elevator.Direction}");
                Console.WriteLine($"Is Moving: {elevator.IsMoving}");
                Console.WriteLine($"Passenger Count: {elevator.PassengerCount}");
                foreach (var passenger in elevator.Passengers)
                {
                    Console.WriteLine($"  - Passenger to Floor: {passenger.DestinationFloor}");
                }
                Console.WriteLine();
            }
        }

        static async Task CallElevator()
        {
            Console.WriteLine("Enter the floor number to call the elevator to:");
            if (int.TryParse(Console.ReadLine(), out int floor))
            {
                await _elevatorService.CallElevatorAsync(floor);
            }
            else
            {
                Console.WriteLine("Invalid floor number.");
            }
        }

        static async Task RequestElevator()
        {
            Console.WriteLine("Enter the current floor number of the passenger:");
            if (int.TryParse(Console.ReadLine(), out int currentFloor))
            {
                Console.WriteLine("Enter the destination floor number of the passenger:");
                if (int.TryParse(Console.ReadLine(), out int destinationFloor))
                {
                    await _elevatorService.RequestElevatorAsync(currentFloor, destinationFloor);
                }
                else
                {
                    Console.WriteLine("Invalid destination floor number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid current floor number.");
            }
        }
    }
}
