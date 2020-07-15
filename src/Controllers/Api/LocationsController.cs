using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OrigemDestino.Controllers.Api.Dtos;
using OrigemDestino.Services;
using OrigemDestino.ViewModels;
using src.Controllers.Api.Dtos;

namespace OrigemDestino.Controllers.Api
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IOrigemDestinoService _origemDestinoService;
        private readonly IFileService _fileService;

        public LocationsController(IOrigemDestinoService origemDestinoService,
            IFileService fileService)
        {
            _origemDestinoService = origemDestinoService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = _origemDestinoService.GetAllLocations();

            var dto = new List<LocalDto>();

            foreach (var location in locations)
            {
                dto.Add(new LocalDto
                {
                    X = location.X,
                    Y = location.Y,
                    Frequentadores = location.LocationFrequenters.Select(lf => lf.FrequenterId).ToArray()
                });
            }

            return Ok(dto);
        }

        [HttpGet, Route("histogram")]
        public IActionResult GetFrequentersPerLocation()
        {
            var locations = _origemDestinoService.GetAllLocations();

            var locationsPerFrequenter = locations.GroupBy(l => l.LocationFrequenters.Count());

            var histogram = new List<LocationsHistogramDto>();

            foreach (var locationPerFrequenter in locationsPerFrequenter)
            {
                histogram.Add(new LocationsHistogramDto
                {
                    NumOfFrequenters = locationPerFrequenter.Key,
                    NumOfLocations = locationPerFrequenter.Count()
                });
            }

            return Ok(histogram.OrderByDescending(h => h.NumOfLocations));
        }

        [HttpGet, Route("graph")]
        public IActionResult GetGraph()
        {
            var graph = _fileService.ReadGraphFromTextFile("Data/cenario1.txt");

            var dto = new FrequenterGraphDto
            {
                EdgesCount = graph.EdgesCount,
                Distributions = graph.Distributions
            };

            return Ok(dto);
        }

        [HttpGet, Route("distances")]
        public IActionResult GetDistances()
        {
            var graph = _fileService.ReadGraphFromTextFile("Data/cenario3.txt");
            var components = graph.GetComponents();
            var biggestComponent = components.OrderByDescending(x => x.Nodes.Count()).First();
            var distances = biggestComponent.GetDistances();

            var dto = new List<GraphDistancesDto>();

            foreach (var distance in distances)
            {
                dto.Add(new GraphDistancesDto
                {
                    Distance = distance.Key,
                    PairsCount = distance.Value
                });
            }

            var viewModel = new DistancesViewModel
            {
                Distances = dto.OrderBy(d => d.Distance).ToList(),
                Diameter = dto.Max(d => d.Distance),
                AverageDistance = dto.Sum(d => d.Distance * d.PairsCount) / dto.Sum(d => d.PairsCount)
            };

            return Ok(viewModel);
        }
    }
}