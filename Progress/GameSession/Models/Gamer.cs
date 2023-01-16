using Contracts.DTO;
using Contracts.Enums;

namespace GameSession.Models
{

    public class Gamer
    {
        IList<CoordinateSimple> HistoryShoot = new List<CoordinateSimple>();
        public string ConnetcionId { get; set; }
        public bool IsShooted { get; set; } = false;

        public IEnumerable<ShipDto> Ships { get; set; }

        public Gamer(string connetcionId, IEnumerable<ShipDto> ships)
        {
            ConnetcionId = connetcionId;
            Ships = ships;
        }

        public EShootStatus EvolveShoot(CoordinateSimple coordinateShoot)
        {
            var shootedShipsStatus = Ships.Select(s => s.ShootValidate(coordinateShoot));
            EShootStatus? status = shootedShipsStatus.SingleOrDefault(s => s != EShootStatus.Miss);
            return status ?? EShootStatus.Miss;

        }

        public Gamer AddHistory(CoordinateSimple history)
        {
            HistoryShoot.Add(history);
            return this;
        }
    }

}
