using Microsoft.AspNetCore.Mvc;
using OrigemDestino.Services;
using System.Linq;

namespace src.Controllers
{
    public class HistogramController : Controller
    {
        private readonly IOrigemDestinoService _origemDestinoService;
        private readonly IFileService _fileService;

        public HistogramController(IOrigemDestinoService origemDestinoService,
            IFileService fileService)
        {
            _origemDestinoService = origemDestinoService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Graph()
        {
            return View();
        }

        public IActionResult Components()
        {
            var graph = _fileService.ReadGraphFromTextFile("Data/cenario3.txt");
            var components = graph.GetComponents();          

            return View(components);
        }

        public IActionResult Distances()
        {
            return View();
        }
    }
}