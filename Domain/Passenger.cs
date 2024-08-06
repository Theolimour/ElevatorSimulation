namespace ElevatorSimulation.Domain
{
    public class Passenger
    {
        public int Id { get; set; }
        public int DestinationFloor { get; set; }

        public Passenger(int id, int destinationFloor)
        {
            Id = id;
            DestinationFloor = destinationFloor;
        }
    }
}
