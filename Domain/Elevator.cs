namespace ElevatorSimulation.Domain
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; }
        public bool IsMoving { get; set; }
        public string Direction { get; set; }
        public int PassengerCount { get; set; }
        public int MaxCapacity { get; set; }

        public Elevator(int id, int maxCapacity)
        {
            Id = id;
            MaxCapacity = maxCapacity;
            CurrentFloor = 0; // Assuming ground floor as the initial floor
            IsMoving = false;
            Direction = "Stationary";
            PassengerCount = 0;
        }

        
    }
}
