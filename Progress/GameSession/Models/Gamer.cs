using Contracts.DTO;
using Contracts.Enums;
using System.Reactive.Subjects;

namespace GameSession.Models
{


    /// <summary>
    /// Игрок
    /// </summary>
    public class Gamer
    {
        private IList<CoordinateSimple> HistoryShoot = new List<CoordinateSimple>();

        /// <summary>
        /// ID игрока в БД
        /// </summary>
        public long? UserEntityId = null;

        /// <summary>
        /// Ид соединения,он же и главный идентификатор
        /// </summary>
        public string ConnectionId { get; set; }

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

        /// <summary>
        /// обрыв соединения, передаётся ConnectionId
        /// </summary>
        public Subject<string> DisconnectedSub = new();

        public Gamer(string connetcionId, IEnumerable<ShipDto> ships)
        {
            ConnectionId = connetcionId;
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
            return shootedShipsStatus.SingleOrDefault((status) => !status.Item1.IsMissing());

        }

        /// <summary>
        /// добавить выстрел в историю 
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public Gamer AddHistory(CoordinateSimple history)
        {
            HistoryShoot.Add(history);
            return this;
        }

        /// <summary>
        /// переключить статус выстрела
        /// </summary>
        public void SwitchShoot()
        {
            ChangeStatus(this.IsShooted ? EGamerStatus.Wait : EGamerStatus.Shooted);
        }

        public bool EqualsConnectionId(string connectionId) { return this.ConnectionId.Equals(connectionId); }

        public void SetDisconnected()
        {
            ChangeStatus(EGamerStatus.Disconnectd);
            this.DisconnectedSub.OnNext(this.ConnectionId);
            this.DisconnectedSub.OnCompleted();
        }

        public void ChangeStatus(EGamerStatus newStatus)
        {
            this.Status = newStatus;
        }

        public bool IsWinner()
        {
            return Status == EGamerStatus.Winner;
        }

        public void SetUserEntityId(long id)
        {
            this.UserEntityId = id;
        }
    }

}
