using System.Collections.Generic;

namespace OrigemDestino.Core
{
    public class Frequenter
    {
        public int Id { get; set; }
        public virtual ICollection<LocationFrequenter> LocationFrequenters { get; set; }
    }
}