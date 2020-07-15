using OrigemDestino.Controllers.Api.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrigemDestino.ViewModels
{
    public class DistancesViewModel
    {
        public List<GraphDistancesDto> Distances { get; set; }
        public int Diameter { get; set; }
        public int AverageDistance { get; set; }
    }
}
