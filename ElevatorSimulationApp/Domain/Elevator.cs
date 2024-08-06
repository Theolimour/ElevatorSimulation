using System;
using System.Collections.Generic;

namespace ElevatorSimulation.Domain
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; }
        public bool IsMoving { get; set; }
        public string Direction { get; set; }
        public int PassengerCount => Passengers.Count;
        public int MaxCapacity { get; set; }
        public List<Passenger> Passengers { get; set; }

        public Elevator(int id, int maxCapacity)
        {
            Id = id;
            MaxCapacity = maxCapacity;
            CurrentFloor = 0; // Assuming ground floor as the initial floor
            IsMoving = false;
            Direction = "Stationary";
            Passengers = new List<Passenger>();
        }

        public void MoveToFloor(int targetFloor)
        {
            if (CurrentFloor < targetFloor)
            {
                Direction = "Up";
            }
            else if (CurrentFloor > targetFloor)
            {
                Direction = "Down";
            }
            else
            {
                Direction = "Stationary";
            }

            IsMoving = true;
            // Simulate the time taken to move
            System.Threading.Thread.Sleep(Math.Abs(CurrentFloor - targetFloor) * 1000);
            CurrentFloor = targetFloor;
            IsMoving = false;
            Direction = "Stationary";
        }

        public bool IsAvailable()
        {
            return !IsMoving && PassengerCount < MaxCapacity;
        }

        public int GetDistance(int floor)
        {
            return Math.Abs(CurrentFloor - floor);
        }

        public bool AddPassenger(Passenger passenger)
        {
            if (PassengerCount < MaxCapacity)
            {
                Passengers.Add(passenger);
                return true;
            }
            return false;
        }

        public void RemovePassengers(int floor)
        {
            Passengers.RemoveAll(p => p.DestinationFloor == floor);
        }
    }
}
