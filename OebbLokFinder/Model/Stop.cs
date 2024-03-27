using Newtonsoft.Json;

namespace OebbLokFinder.Model
{
    public class Stop
    {
        public int Id { get; set; }

        public Rollingstock Rollingstock { get; set; }

        public int RollingstockId { get; set; }

        public Station Station { get; set; }

        public DateTime? Arrival { get; set; } = DateTime.MinValue;

        public DateTime Departure { get; set; } = DateTime.MinValue;

        public DateTime Fetched { get; set; } = DateTime.Now;
    }

}
