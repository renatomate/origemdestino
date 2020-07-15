using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrigemDestino.Core
{
    public class FrequenterGraph
    {
        public List<FrequenterNode> Nodes { get; set; }

        public int EdgesCount
        {
            get
            {
                return Nodes.SelectMany(n => n.Neighbors).Count() / 2;
            }
        }

        private List<KeyValuePair<int, int>> _distributions;

        public List<KeyValuePair<int, int>> Distributions
        {
            get
            {
                if (_distributions != null)
                    return _distributions;

                _distributions = new List<KeyValuePair<int, int>>();

                foreach (var node in Nodes)
                {
                    var neighborsCount = node.Neighbors.Count();

                    if (_distributions.Any(d => d.Key == neighborsCount))
                    {
                        var distribution = _distributions.Single(d => d.Key == neighborsCount);
                        var index = _distributions.IndexOf(distribution);
                        var newDistribution = new KeyValuePair<int, int>(neighborsCount, distribution.Value + 1);
                        _distributions[index] = newDistribution;
                    }
                    else
                    {
                        _distributions.Add(new KeyValuePair<int, int>(neighborsCount, 1));
                    }
                }

                _distributions = _distributions.OrderBy(k => k.Key).ToList();

                return _distributions;
            }
        }

        public List<GraphComponent> GetComponents()
        {
            var components = new List<GraphComponent>();

            var thread = new Thread(() =>
            {
                var visitedNodes = new ConcurrentDictionary<int, bool>(Nodes.ToDictionary(n => n.Frequenter.Id, n => false));

                while (visitedNodes.Any(n => !n.Value))
                {
                    components.Add(new GraphComponent
                    {
                        Nodes = DeepFirstSearch(visitedNodes.First(n => !n.Value).Key, visitedNodes).ToList()
                    });
                }
            }, 8000000);

            thread.Start();
            thread.Join();

            return components.Distinct().ToList();
        }

        public List<GraphComponent> GetComponentsIterative()
        {
            var components = new List<GraphComponent>();

            var unvisitedNodes = Nodes.ToList();

            var nodesToBeVisited = new Stack<FrequenterNode>();            

            while (unvisitedNodes.Any())
            {
                nodesToBeVisited.Push(unvisitedNodes.First());

                var component = new GraphComponent
                {
                    Nodes = new List<FrequenterNode>()
                };

                while (nodesToBeVisited.Any())
                {
                    var currentNode = nodesToBeVisited.Pop();
                    component.Nodes.Add(currentNode);

                    if (unvisitedNodes.Contains(currentNode))
                    {
                        unvisitedNodes.Remove(currentNode);
                    }

                    foreach (var neighboor in currentNode.Neighbors)
                    {
                        var neighboorNode = Nodes.Single(n => n.Frequenter.Id == neighboor.Id);
                        if (unvisitedNodes.Contains(neighboorNode))
                        {
                            nodesToBeVisited.Push(neighboorNode);
                        }
                    }
                }

                components.Add(component);
            }

            return components;
        }

        private IEnumerable<FrequenterNode> DeepFirstSearch(int frequenterId, ConcurrentDictionary<int, bool> visitedNodes)
        {
            visitedNodes[frequenterId] = true;

            var currentNode = Nodes.Single(n => n.Frequenter.Id == frequenterId);
            var nodes = new ConcurrentBag<FrequenterNode> { currentNode };

            var neighboors = currentNode.Neighbors;

            Parallel.ForEach(neighboors.Where(n => !visitedNodes[n.Id]), (neighboor) =>
            {
                foreach (var childNode in DeepFirstSearch(neighboor.Id, visitedNodes))
                {
                    nodes.Add(childNode);
                }
            });

            return nodes;
        }
    }
}