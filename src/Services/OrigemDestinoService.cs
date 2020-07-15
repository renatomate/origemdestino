using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrigemDestino.Core;
using OrigemDestino.Persistence;

namespace OrigemDestino.Services
{
    public class OrigemDestinoService : IOrigemDestinoService
    {
        private readonly IOrigemDestinoRepository _origemDestinoRepository;
        private readonly ODContext _context;

        public OrigemDestinoService(IOrigemDestinoRepository origemDestinoRepository,
        ODContext context)
        {
            _origemDestinoRepository = origemDestinoRepository;
            _context = context;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            var locations = _context.Locations
            .Include(l => l.LocationFrequenters)
            .ThenInclude(lf => lf.Frequenter)
            .ToList();

            if (locations == null || !locations.Any())
            {
                MapFromDbf();
                locations = _context.Locations.Include(l => l.LocationFrequenters).ToList();
            }

            return locations;
        }

        public Graph GetGenericGraph()
        {
            var graph = new Graph { Nodes = new List<Node>() };

            var locations = GetAllLocations();

            var frequenters = locations.SelectMany(l => l.LocationFrequenters).Select(lf => lf.Frequenter).Distinct();

            var nodesDictionary = new Dictionary<int, Node>();

            foreach (var frequenter in frequenters)
            {
                if (!nodesDictionary.TryGetValue(frequenter.Id, out Node node))
                {
                    node = new Node
                    {
                        Id = frequenter.Id,
                        Neighboors = new List<Node>()
                    };

                    nodesDictionary.Add(frequenter.Id, node);
                }

                var neighboorsFrequenters = frequenter.LocationFrequenters
                .Select(lf => lf.Location)
                .SelectMany(l => l.LocationFrequenters)
                .Select(lf => lf.Frequenter)
                .Where(f => f.Id != frequenter.Id);

                foreach (var neighboor in neighboorsFrequenters)
                {
                    if (!nodesDictionary.TryGetValue(neighboor.Id, out Node neighboorNode))
                    {
                        neighboorNode = new Node
                        {
                            Id = neighboor.Id,
                            Neighboors = new List<Node>()
                        };

                        nodesDictionary.Add(neighboorNode.Id, neighboorNode);
                    }

                    node.Neighboors.Add(neighboorNode);
                }

                graph.Nodes.Add(node);
            }

            return graph;
        }

        public FrequenterGraph GetGraph()
        {
            var locations = GetAllLocations();
            var nodes = new List<FrequenterNode>();

            foreach (var frequenter in locations.SelectMany(l => l.LocationFrequenters).Select(lf => lf.Frequenter).Distinct())
            {
                nodes.Add(new FrequenterNode
                {
                    Frequenter = frequenter,
                    Neighbors = frequenter.LocationFrequenters
                    .Select(lf => lf.Location)
                    .SelectMany(l => l.LocationFrequenters)
                    .Select(lf => lf.Frequenter)
                    .Where(f => f.Id != frequenter.Id)
                    .Distinct()
                    .ToList()
                });
            }

            var graph = new FrequenterGraph
            {
                Nodes = nodes
            };

            return graph;
        }

        private void MapFromDbf()
        {
            var locations = _origemDestinoRepository.GetAllFromDbf();

            _context.Locations.AddRange(locations);
            _context.SaveChanges();
        }
    }
}