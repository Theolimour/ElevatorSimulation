using ElevatorSimulation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorSimulation.Services
{
    public class ElevatorService
    {
        private readonly List<Elevator> _elevators;
        private readonly List<Floor> _floors;
        private readonly Timer _timer;
        private readonly object _lock = new object();

        public ElevatorService(int numberOfElevators, int elevatorCapacity, int numberOfFloors)
        {
            _elevators = new List<Elevator>();
            for (int i = 1; i <= numberOfElevators; i++)
            {
                _elevators.Add(new Elevator(i, elevatorCapacity));
            }

            _floors = new List<Floor>();
            for (int i = 0; i < numberOfFloors; i++)
            {
                _floors.Add(new Floor(i));
            }

            _timer = new Timer(UpdateElevators, null, 0, 1000);
        }

        public List<Elevator> GetElevators()
        {
            lock (_lock)
            {
                return _elevators.ToList();
            }
        }

        public List<Floor> GetFloors()
        {
            lock (_lock)
            {
                return _floors.ToList();
            }
        }

        public async Task MoveElevatorAsync(int id, int targetFloor)
        {
            var elevator = GetElevatorById(id);
            if (elevator == null)
            {
                throw new KeyNotFoundException("Elevator not found.");
            }

            await Task.Run(() =>
            {
                lock (_lock)
                {
                    elevator.MoveToFloor(targetFloor);
                    elevator.RemovePassengers(targetFloor);
                    BoardPassengers(elevator, targetFloor);
                }
            });
        }

        public async Task CallElevatorAsync(int floorNumber)
        {
            var availableElevators = _elevators.Where(e => e.IsAvailable()).ToList();
            if (availableElevators.Count == 0)
            {
                Console.WriteLine("No available elevators at the moment.");
                return;
            }

            var nearestElevator = availableElevators.OrderBy(e => e.GetDistance(floorNumber)).First();
            await MoveElevatorAsync(nearestElevator.Id, floorNumber);
            Console.WriteLine($"Elevator {nearestElevator.Id} is moving to floor {floorNumber}.");
        }

        public async Task RequestElevatorAsync(int floorNumber, int destinationFloor)
        {
            var floor = _floors.FirstOrDefault(f => f.FloorNumber == floorNumber);
            if (floor == null)
            {
                throw new KeyNotFoundException("Floor not found.");
            }

            floor.AddPassenger(new Passenger(new Random().Next(), destinationFloor));
            await CallElevatorAsync(floorNumber);
        }

        private void BoardPassengers(Elevator elevator, int floorNumber)
        {
            var floor = _floors.FirstOrDefault(f => f.FloorNumber == floorNumber);
            if (floor == null) return;

            while (elevator.PassengerCount < elevator.MaxCapacity)
            {
                var passenger = floor.GetNextPassenger();
                if (passenger == null) break;

                if (!elevator.AddPassenger(passenger))
                {
                    floor.AddPassenger(passenger);
                    break;
                }
            }
        }

        private Elevator GetElevatorById(int id)
        {
            return _elevators.FirstOrDefault(e => e.Id == id);
        }

        private void UpdateElevators(object state)
        {
            lock (_lock)
            {
                foreach (var elevator in _elevators)
                {
                    if (elevator.IsMoving)
                    {
                        if (elevator.Direction == "Up")
                        {
                            elevator.CurrentFloor++;
                        }
                        else if (elevator.Direction == "Down")
                        {
                            elevator.CurrentFloor--;
                        }

                        if (elevator.CurrentFloor == 0 || elevator.CurrentFloor == _floors.Count - 1)
                        {
                            elevator.IsMoving = false;
                            elevator.Direction = "Stationary";
                        }
                    }
                }
            }
        }
    }
}
