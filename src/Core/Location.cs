using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OrigemDestino.Core
{
    public class Location
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public virtual ICollection<LocationFrequenter> LocationFrequenters { get; set; }

        public Location()
        {
            LocationFrequenters = new Collection<LocationFrequenter>();
        }

        public void AddFrequenter(Frequenter frequenter)
        {
            if (!LocationFrequenters.Any(lf => lf.Frequenter == frequenter))
            {
                LocationFrequenters.Add(new LocationFrequenter
                {
                    Location = this,
                    Frequenter = frequenter
                });
            }
        }
    }
}