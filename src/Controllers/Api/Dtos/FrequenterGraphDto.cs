using System;
using System.Collections.Generic;

namespace src.Controllers.Api.Dtos
{
    public class FrequenterGraphDto
    {
        public int EdgesCount { get; set; }
        public List<KeyValuePair<int, int>> Distributions { get; set; }
    }
}