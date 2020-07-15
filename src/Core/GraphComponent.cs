using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrigemDestino.Core
{
    public class GraphComponent
    {
        public IList<FrequenterNode> Nodes { get; set; }

        public Dictionary<int, int> GetDistances()
        {
            var distances = new List<int>();
            foreach (var node in Nodes)
            {
                node.Distance = 0;
                var nodesToVisit = new Queue<FrequenterNode>();
                var visitedNodes = new List<int>();

                nodesToVisit.Enqueue(node);

                while (nodesToVisit.Any())
                {
                    var visitingNode = nodesToVisit.Dequeue();        
                    visitedNodes.Add(visitingNode.Frequenter.Id);

                    foreach (var neighboor in visitingNode.Neighbors.Where(n => !visitedNodes.Contains(n.Id)))
                    {
                        var neighboorNode = Nodes.Single(n => n.Frequenter.Id == neighboor.Id);
                        nodesToVisit.Enqueue(neighboorNode);
                        neighboorNode.Distance = visitingNode.Distance + 1;
                        distances.Add(neighboorNode.Distance);
                    }
                }
            }

            return distances.GroupBy(d => d).ToDictionary(d => d.Key, d => d.Count()/2);
        }
    }
}
