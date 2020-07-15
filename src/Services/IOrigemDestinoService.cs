using System.Collections.Generic;
using OrigemDestino.Core;

namespace OrigemDestino.Services
{
    public interface IOrigemDestinoService
    {
        IEnumerable<Location> GetAllLocations();
        FrequenterGraph GetGraph();
        Graph GetGenericGraph();
    }
}