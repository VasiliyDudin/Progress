using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Contracts.DTO
{
    public class ShipDto
    {
        public IEnumerable<CoordinateSimple> Coordinates { get; set; }

        public EShootStatus ShootValidate(CoordinateSimple coordinate)
        {
            var targetCoordinate = Coordinates.SingleOrDefault(c => c.Equals(coordinate));
            if (targetCoordinate == null)
            {
                return EShootStatus.Miss;
            }
            if (targetCoordinate.IsShooted)
            {
                return EShootStatus.Error;
            }
            targetCoordinate.IsShooted = true;
            if (Coordinates.All(c => c.IsShooted))
            {
                return EShootStatus.Killing;
            }
            return EShootStatus.Miss;
        }
    }


}
