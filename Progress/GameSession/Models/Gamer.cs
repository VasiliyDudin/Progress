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
        /// Ид соединения,он же и главный идентификатор
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// игрок стреляет
        /// </summary>
        public bool IsShooted { get; set; } = false;

        /// <summary>
        ///  С игроком потеряна связь
        /// </summary>
        public bool IsDisconnected { get; set; } = false;

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
        public (EShootStatus, ShipDto) EvolveShoot(CoordinateSimple coordinateShoot)
        {
            var shootedShipsStatus = Ships.Select(s => s.ShootValidate(coordinateShoot)).ToList();
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
        /// переключить флаг выстрела
        /// </summary>
        public void SwitchShoot()
        {
            this.IsShooted = !this.IsShooted;
        }

        public bool EqualsConnectionId(string connectionId) { return this.ConnectionId.Equals(connectionId); }
        public void SetDisconnected()
        {
            this.IsDisconnected = true;
            this.DisconnectedSub.OnNext(this.ConnectionId);
            this.DisconnectedSub.OnCompleted();
        }
    }

}
