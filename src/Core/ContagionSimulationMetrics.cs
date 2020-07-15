using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrigemDestino.Core
{
    public class ContagionSimulationMetrics
    {
        public int Step { get; set; }
        public int InfectedCount { get; set; }
        public int SusceptibleCount { get; set; }
        public int RemovedCount { get; set; }
    }
}
