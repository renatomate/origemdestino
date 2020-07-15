using System.Collections.Generic;

namespace OrigemDestino.Core
{
    public class FrequenterNode
    {
        public Frequenter Frequenter { get; set; }
        public List<Frequenter> Neighbors { get; set; }
        public int Distance { get; set; }
    }
}