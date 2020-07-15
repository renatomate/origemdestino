using System.Collections.Generic;

namespace OrigemDestino.Core
{
    public class Node
    {
        public int Id { get; set; }
        public IList<Node> Neighboors { get; set; }
        public SicknessType SicknessType { get; set; }

        public void AddNeighboor(Node neighboor)
        {
            if (Neighboors == null)
            {
                Neighboors = new List<Node>();
            }

            Neighboors.Add(neighboor);
        }
    }
}
