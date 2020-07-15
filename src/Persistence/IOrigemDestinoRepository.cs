using System.Collections.Generic;
using OrigemDestino.Core;

namespace OrigemDestino.Persistence
{
    public interface IOrigemDestinoRepository
    {
        ICollection<Location> GetAllFromDbf();
    }
}