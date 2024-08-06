using System.Collections.Generic;

namespace ElevatorSimulation.Domain
{
    public class Floor
    {
        public int FloorNumber { get; set; }
        public Queue<Passenger> WaitingPassengers { get; set; }

        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
            WaitingPassengers = new Queue<Passenger>();
        }

        public void AddPassenger(Passenger passenger)
        {
            WaitingPassengers.Enqueue(passenger);
        }

        public Passenger GetNextPassenger()
        {
            return WaitingPassengers.Count > 0 ? WaitingPassengers.Dequeue() : null;
        }
    }
}
