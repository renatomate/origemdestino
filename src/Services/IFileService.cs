using OrigemDestino.Core;

namespace OrigemDestino.Services
{
    public interface IFileService
    {
        Graph ReadGraphFromTextFile(string path);
    }
}
