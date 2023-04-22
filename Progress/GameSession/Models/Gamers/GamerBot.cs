using System.Reactive.Subjects;
using Contracts.DTO;
using Contracts.Enums;

namespace GameSession.Models.Gamers
{
    public class GamerBot : IGamer
    {
        public bool IsShooted { get; }
        public string ConnectionId { get; set; }
        public long? UserEntityId { get; set; }
        public Subject<string> DisconnectedSub { get; set; }

        private readonly IEnumerable<ShipDto> _ships;

        public GamerBot(string connetcionId, IEnumerable<ShipDto> ships)
        {
            ConnectionId = connetcionId;
            _ships = ships;
        }

        public (EShootStatus, ShipDto?) EvolveShoot(CoordinateSimple coordinateShoot)
        {
            throw new NotImplementedException();
        }

        public Gamer AddHistory(CoordinateSimple history)
        {
            throw new NotImplementedException();
        }

        public void SwitchShoot()
        {
            throw new NotImplementedException();
        }

        public void SetDisconnected()
        {
            throw new NotImplementedException();
        }

        public bool EqualsConnectionId(string connectionId)
        {
            throw new NotImplementedException();
        }

        public void ChangeStatus(EGamerStatus newStatus)
        {
            throw new NotImplementedException();
        }

        public bool IsWinner()
        {
            throw new NotImplementedException();
        }

        public void SetUserEntityId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
