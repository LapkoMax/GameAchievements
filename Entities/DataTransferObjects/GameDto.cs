using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class GameDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public string Genres { get; set; }
    }
}
