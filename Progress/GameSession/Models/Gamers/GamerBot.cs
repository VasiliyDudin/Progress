using System.Reactive.Subjects;
using Contracts.DTO;
using Contracts.Enums;

namespace GameSession.Models.Gamers
{
    public class GamerBot : IGamer
    {
        private IList<CoordinateSimple> HistoryShoot = new List<CoordinateSimple>();

        /// <summary>
        /// ID игрока в БД
        /// </summary>
        public long? UserEntityId { get; set; } = null;

        /// <summary>
        /// Ид соединения,он же и главный идентификатор
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// обрыв соединения, передаётся ConnectionId
        /// </summary>
        public Subject<string> DisconnectedSub { get; set; } = new Subject<string>();

        private EGamerStatus Status { get; set; } = EGamerStatus.Unknown;

        /// <summary>
        /// игрок стреляет
        /// </summary>
        public bool IsShooted
        {
            get
            {
                return Status == EGamerStatus.Shooted;
            }
        }

        /// <summary>
        ///  С игроком потеряна связь
        /// </summary>
        public bool IsDisconnected
        {
            get
            {
                return Status == EGamerStatus.Disconnectd;
            }
        }

        private readonly IEnumerable<ShipDto> Ships;

        public delegate Task ShootBot(string shootGamerConnectionId, CoordinateSimple coordinateShoot);
        public event ShootBot ShootBotEvent;

        public GamerBot(IEnumerable<ShipDto> ships)
        {
            ConnectionId = "-111";
            Ships = ships;
        }

        /// <summary>
        /// обработка выстрела по игроку
        /// </summary>
        /// <param name="coordinateShoot"></param>
        /// <returns></returns>
        public (EShootStatus, ShipDto?) EvolveShoot(CoordinateSimple coordinateShoot)
        {
            var shootedShipsStatus = Ships.Select(s => s.ShootValidate(coordinateShoot)).ToList();
            if (Ships.All(s => s.IsKilling()))
            {
                return (EShootStatus.KillingAll, null);
            }

            var status = shootedShipsStatus.SingleOrDefault((status) => !status.Item1.IsMissing());
            return status;
        }

        public void ContinueShoot()
        {
            if (Status == EGamerStatus.Shooted)
            {
                CoordinateSimple newCoordinates = CalculateShootCoordinate();
                ShootBotEvent?.Invoke(this.ConnectionId, newCoordinates);
            }
        }

        /// <summary>
        /// добавить выстрел в историю 
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public IGamer AddHistory(CoordinateSimple history)
        {
            HistoryShoot.Add(history);
            return this;
        }

        /// <summary>
        /// переключить статус выстрела
        /// </summary>
        public void SwitchShoot()
        {
            ChangeStatus(IsShooted ? EGamerStatus.Wait : EGamerStatus.Shooted);

            
        }

        private CoordinateSimple CalculateShootCoordinate()
        {
            var random = new Random();
            var newCoordinates = new CoordinateSimple() { X = random.Next(0, 10), Y = random.Next(0, 10) };

            return newCoordinates;
        }

        public bool EqualsConnectionId(string connectionId) { return ConnectionId.Equals(connectionId); }

        public void SetDisconnected()
        {
            ChangeStatus(EGamerStatus.Disconnectd);
            DisconnectedSub.OnNext(ConnectionId);
            DisconnectedSub.OnCompleted();
        }

        public void ChangeStatus(EGamerStatus newStatus)
        {
            Status = newStatus;

            if (Status == EGamerStatus.Shooted)
            {
                CoordinateSimple newCoordinates = CalculateShootCoordinate();
                ShootBotEvent?.Invoke(this.ConnectionId, newCoordinates);
            }
        }

        public bool IsWinner()
        {
            return Status == EGamerStatus.Winner;
        }

        public void SetUserEntityId(long id)
        {
            UserEntityId = id;
        }
    }
}
