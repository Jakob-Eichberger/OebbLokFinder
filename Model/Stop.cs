using Newtonsoft.Json;

namespace Model
{
    public class Stop
    {
        public int Id { get; set; }

        public string TrainNumber { get; set; } = "";

        public Vehicle Vehicle { get; set; }

        public int VehicleId { get; set; }

        public Station Station { get; set; }

        public int StationId { get; set; }

        public DateTime? Arrival { get; set; } = DateTime.MinValue;

        public DateTime Departure { get; set; } = DateTime.MinValue;
    }

    public class Station
    {
        public int Id { get; set; }

        public string StationName { get; set; }

        public string StationNameAbbr { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public List<Stop> Stops { get; } = new();
    }

}
