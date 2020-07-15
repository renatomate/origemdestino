using OrigemDestino.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrigemDestino.Services
{
    public class FileService : IFileService
    {
        public Graph ReadGraphFromTextFile(string path)
        {
            var lines = File.ReadAllLines(path);

            var graph = new Graph
            {
                Nodes = new List<Node>()
            };

            foreach (var line in lines.Skip(2))
            {
                try
                {
                    var parts = line.Split(' ');

                    var fromId = Convert.ToInt32(parts[0]);
                    var toId = Convert.ToInt32(parts[1]);

                    graph.AddConnection(fromId, toId);
                }
                catch
                {
                }
            }

            return graph;
        }
    }
}
