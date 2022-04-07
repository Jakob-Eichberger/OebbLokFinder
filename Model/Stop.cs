using Newtonsoft.Json;

namespace Model
{
    public class Stop
    {
        public int Id { get; set; }

        public Vehicle Vehicle { get; set; }

        public int VehicleId { get; set; }

        public Station Station { get; set; }

        public DateTime? Arrival { get; set; } = DateTime.MinValue;

        public DateTime Departure { get; set; } = DateTime.MinValue;

        public DateTime Fetched { get; set; } = DateTime.Now;
    }

}
