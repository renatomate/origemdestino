using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace OrigemDestino.Core
{
    public class Graph
    {
        private IDictionary<int, Node> _nodesDictionary;

        public IList<Node> Nodes { get; set; }

        public Graph()
        {
            _nodesDictionary = new Dictionary<int, Node>();
        }

        public int EdgesCount
        {
            get
            {
                return Nodes.SelectMany(n => n.Neighboors).Count() / 2;
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
                    var neighborsCount = node.Neighboors.Count();

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

        public Dictionary<int, int> GetDistances()
        {
            // lista que armazenará as distâncias encontradas
            var distances = new List<int>();

            var nodesToVisit = new Queue<Node>();

            // pega o primeiro nó do grafo para começar a visita
            var firstNode = Nodes.First();
            nodesToVisit.Enqueue(firstNode);

            var nodeDistances = new Dictionary<int, int>
                {
                    { firstNode.Id, 0 }
                };

            while (nodesToVisit.Any())
            {
                var currentNode = nodesToVisit.Dequeue();

                // percorre todos os vizinhos que ainda não foram visitados
                foreach (var neighboor in currentNode.Neighboors.Where(n => !nodeDistances.ContainsKey(n.Id)))
                {
                    nodesToVisit.Enqueue(neighboor);

                    // a distância até o vizinho é a distância até o nó atual + 1
                    var distance = nodeDistances[currentNode.Id] + 1;
                    nodeDistances.Add(neighboor.Id, distance);
                    distances.Add(distance);
                }
            }

            var distancesDictionary = new Dictionary<int, int>();

            // monta o dicionário que será retornado como resultado do método
            foreach (var distance in distances.Distinct())
            {
                distancesDictionary.Add(distance, distances.Count(d => d == distance) / 2);
            }

            return distancesDictionary;
        }

        public List<Graph> GetComponents()
        {
            // nós que ainda não foram visitados
            var unvisitedNodes = Nodes.ToHashSet();

            // as componentes são uma lista de grafos
            var components = new List<Graph>();

            // enquanto existirem nós não visitados, executa a função de busca para encontrar novas componentes
            while (unvisitedNodes.Any())
            {
                components.Add(new Graph
                {
                    Nodes = DeepFirstSearch(unvisitedNodes.First(), unvisitedNodes)
                });
            }

            // retorna as componentes encontradas
            return components;
        }

        private List<Node> DeepFirstSearch(Node node, HashSet<Node> unvisitedNodes)
        {
            // remove o nó que está sendo visitado da lista de nós não visitados
            unvisitedNodes.Remove(node);

            // cria a lista de nós encontrados, adicionando ele próprio a ela
            var foundNodes = new List<Node> { node };

            // percorre todos os vizinhos que ainda não foram visitados e executa a busca sobre ele
            foreach (var neighboor in node.Neighboors.Where(n => unvisitedNodes.Contains(n)))
            {
                foundNodes.AddRange(DeepFirstSearch(neighboor, unvisitedNodes));
            }

            // retorna os nós encontrados
            return foundNodes;
        }

        public void AddConnection(int fromId, int toId)
        {
            if (Nodes == null)
            {
                Nodes = new List<Node>();
            }

            if (!_nodesDictionary.TryGetValue(fromId, out Node fromNode))
            {
                fromNode = AddNode(fromId);
            }

            if (!_nodesDictionary.TryGetValue(toId, out Node toNode))
            {
                toNode = AddNode(toId);
            }

            fromNode.AddNeighboor(toNode);
            toNode.AddNeighboor(fromNode);
        }

        public Node AddNode(int id)
        {
            var node = new Node
            {
                Id = id
            };

            Nodes.Add(node);
            _nodesDictionary.Add(id, node);

            return node;
        }

        public List<ContagionSimulationMetrics> GetContagiousSimulation(double contagiousProbability, double recoverProbability)
        {
            // Marca todos os nós inicialmente como suscetíveis
            foreach (var node in Nodes)
            {
                node.SicknessType = SicknessType.Susceptible;
            }

            // Sorteia um nó aleatório e o marca como infectado
            var random = new Random();
            var randomNumber = random.Next(0, Nodes.Count());
            Nodes[randomNumber].SicknessType = SicknessType.Infected;

            // Inicia o contador de tempo (passos) em 1
            var steps = 1;

            // Adiciona o primeiro cenário, ou seja, aquele em que há somente um infectado inicial
            var simulationResult = new List<ContagionSimulationMetrics>
            {
                new ContagionSimulationMetrics
                {
                    Step = steps,
                    InfectedCount = Nodes.Count(n => n.SicknessType == SicknessType.Infected),
                    SusceptibleCount = Nodes.Count(n => n.SicknessType == SicknessType.Susceptible),
                    RemovedCount = Nodes.Count(n => n.SicknessType == SicknessType.Removed)
                }
            };

            // Executa enquanto houver pelo menos um infectado no grafo
            while (Nodes.Any(n => n.SicknessType == SicknessType.Infected))
            {
                steps++;

                // Percorre todos os nós infectados
                foreach (var node in Nodes.Where(n => n.SicknessType == SicknessType.Infected))
                {
                    // Sorteia um número entre 0 e 1
                    var x = random.NextDouble();

                    // Se o número sorteado for menor que a probabilidade de recuperação, marca o nó como recuperado
                    if (x <= recoverProbability)
                    {
                        node.SicknessType = SicknessType.Removed;
                    }
                    else // Do contrário, percorre todos os vizinhos
                    {
                        foreach (var neighboor in node.Neighboors.Where(n => n.SicknessType == SicknessType.Susceptible))
                        {
                            // Sorteia um número entre 0 e 1
                            var y = random.NextDouble();

                            // Se o número sorteado for menor que a probabilidade de contágio, marca o nó como infectado
                            if (y <= contagiousProbability)
                            {
                                neighboor.SicknessType = SicknessType.Infected;
                            }
                        }
                    }
                }

                // Adiciona o cenário atual nas métricas
                simulationResult.Add(new ContagionSimulationMetrics
                {
                    Step = steps,
                    InfectedCount = Nodes.Count(n => n.SicknessType == SicknessType.Infected),
                    SusceptibleCount = Nodes.Count(n => n.SicknessType == SicknessType.Susceptible),
                    RemovedCount = Nodes.Count(n => n.SicknessType == SicknessType.Removed)
                });
            }

            // Retorna as métricas encontradas
            return simulationResult;
        }
    }
}
