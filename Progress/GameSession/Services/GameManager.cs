using Contracts.DTO;
using GameSession.Hubs;
using GameSession.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Collections.Concurrent;

namespace GameSession.Services
{

    public class GameManager
    {
        /// <summary>
        /// текущие игры
        /// </summary>
        private IDictionary<Guid, Game> Games = new Dictionary<Guid, Game>();

        /// <summary>
        /// свободные игроки
        /// </summary>
        private ConcurrentQueue<Gamer> FreeGamers = new ConcurrentQueue<Gamer>();
        private readonly UserStatisticIntegrationOption userStstkSrvOption;
        private readonly GameStatisticBrokerClient statisticBrokerClient;

        private IHubContext<GameHub> HubContext { get; }

        private IConnection _connection;

        public GameManager(
            IHubContext<GameHub> hubContext, 
            GameStatisticBrokerClient statisticBrokerClient)
        {
            new Timer(CreateGames, null, 0, 5 * 1000);
            HubContext = hubContext;
            this.statisticBrokerClient = statisticBrokerClient;
            this.userStstkSrvOption = userStstkSrvOption.Value;
        }

        /// <summary>
        /// регистрация готового игрока
        /// </summary>
        public void RegisterGamer(Gamer gamer)
        {
            FreeGamers.Enqueue(gamer);
        }

        public void AddNewGame(params Gamer[] gamers)
        {
            var game = new Game(HubContext, gamers);
            Games.Add(game.Uid, game);
            game.EndGameSubj.Subscribe(gameUid =>
            {
                Games.Remove(gameUid);
            });
        }


        public async Task EvolveShoot(string shootGamerConnectionId, CoordinateSimple coordinateShoot)
        {
            await Games.Values.Single(g => g.IsExistGamer(shootGamerConnectionId))
                 .EvolveShoot(shootGamerConnectionId, coordinateShoot);
        }

        /// <summary>
        /// удаляем игрока (обрыв связи, и т.д.)
        /// </summary>
        /// <param name="connectionId"></param>
        public void RemoveGamer(string connectionId)
        {
            FreeGamers.SingleOrDefault(g => g.EqualsConnectionId(connectionId))?.SetDisconnected();
            Games.Values.SelectMany(g => g.Gamers).SingleOrDefault(g => g.EqualsConnectionId(connectionId))?.SetDisconnected();
        }

        /// <summary>
        /// в цикле создаёт игры
        /// </summary>
        /// <param name="stateInfo"></param>
        private void CreateGames(Object stateInfo)
        {
            if (FreeGamers.Count < 2)
            {
                return;
            }

            var gamers = new List<Gamer?>
            {
                 GetRanadomGamer(),
                 GetRanadomGamer()
             };

            if (gamers.Any(gamer => gamer == null))
            {
                // возвращаем в очередь
                gamers
                    .Where(g => g != null)
                    .ToList()
                    .ForEach(gamer => { RegisterGamer(gamer!); });
                return;
            }
            AddNewGame(gamers.ToArray()!);
            CreateGames(stateInfo);
        }

        private Gamer? GetRanadomGamer()
        {
            while (FreeGamers.TryDequeue(out var gamer))
            {
                if (gamer.IsDisconnected)
                {
                    return GetRanadomGamer();
                }
                return gamer;
            }
            return null;
        }
    }
}
