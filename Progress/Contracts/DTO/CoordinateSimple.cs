using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class CoordinateSimple
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsShooted { get; set; } = false;

        public bool Equals(CoordinateSimple coordinate)
        {
            return coordinate.X == X && coordinate.Y == Y;
        }
    }
}
