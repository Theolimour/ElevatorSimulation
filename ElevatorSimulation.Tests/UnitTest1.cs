using ElevatorSimulation.Domain;
using ElevatorSimulation.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ElevatorSimulation.Tests
{
    public class ElevatorServiceTests
    {
        private readonly ElevatorService _elevatorService;

        public ElevatorServiceTests()
        {
            _elevatorService = new ElevatorService(numberOfElevators: 3, elevatorCapacity: 10, numberOfFloors: 10);
        }

        [Fact]
        public void TestElevatorInitialization()
        {
            var elevators = _elevatorService.GetElevators();
            Assert.Equal(3, elevators.Count);
            Assert.All(elevators, e => Assert.Equal(10, e.MaxCapacity));
        }

        [Fact]
        public async Task TestMoveElevator()
        {
            var elevator = _elevatorService.GetElevators().First();
            await _elevatorService.MoveElevatorAsync(elevator.Id, 5);
            Assert.Equal(5, elevator.CurrentFloor);
        }

        [Fact]
        public async Task TestCallElevator()
        {
            await _elevatorService.CallElevatorAsync(5);
            var elevator = _elevatorService.GetElevators().First(e => e.CurrentFloor == 5);
            Assert.NotNull(elevator);
        }

        [Fact]
        public async Task TestRequestElevator()
        {
            await _elevatorService.RequestElevatorAsync(2, 8);
            var elevator = _elevatorService.GetElevators().First(e => e.CurrentFloor == 2);
            Assert.NotNull(elevator);
            Assert.Single(elevator.Passengers);
            Assert.Equal(8, elevator.Passengers.First().DestinationFloor);
        }

        [Fact]
        public void TestElevatorCapacityLimit()
        {
            var elevator = _elevatorService.GetElevators().First();
            for (int i = 0; i < 10; i++)
            {
                elevator.AddPassenger(new Passenger(i, 5));
            }

            var result = elevator.AddPassenger(new Passenger(11, 6));
            Assert.False(result);
            Assert.Equal(10, elevator.PassengerCount);
        }
    }
}
