using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrigemDestino.Services;

namespace OrigemDestino.Controllers.Api
{
    [Route("api/simulation")]
    [ApiController]
    public class ContagionSimulationController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ContagionSimulationController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var graph = _fileService.ReadGraphFromTextFile("Data/cenario3.txt");

            var metrics = graph.GetContagiousSimulation(0.3, 0.7);

            return Ok(metrics);
        }

    }
}