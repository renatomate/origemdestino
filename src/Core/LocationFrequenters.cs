namespace OrigemDestino.Core
{
    public class LocationFrequenter
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int FrequenterId { get; set; }
        public Frequenter Frequenter { get; set; }
    }
}