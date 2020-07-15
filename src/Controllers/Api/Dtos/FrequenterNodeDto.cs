using System.Collections.Generic;

namespace src.Controllers.Api.Dtos
{
    public class FrequenterNodeDto
    {
        public int FrequenterId { get; set; }
        public List<int> Neighbors { get; set; }
    }
}