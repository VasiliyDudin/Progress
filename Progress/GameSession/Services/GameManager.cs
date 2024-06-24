using Contracts.DTO;
using GameSession.Hubs;
using GameSession.Models;
using GameSession.Models.Gamers;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using View.Models.In;

namespace GameSession.Services
{

    public class GameManager : IGamerManager
    {
        /// <summary>
        /// текущие игры
        /// </summary>
        private IDictionary<Guid, Game> Games = new Dictionary<Guid, Game>();

        /// <summary>
        /// свободные игроки
        /// </summary>
        private ConcurrentQueue<Gamer> FreeGamers = new ConcurrentQueue<Gamer>();
        private readonly GameStatisticBrokerClient statisticBrokerClient;

        private ConcurrentQueue<GamerBot> BotGamers = new ConcurrentQueue<GamerBot>();
        private GamerBot activeBotGamer;

        private IHubContext<GameHub> HubContext { get; }

        private readonly Timer timer;
        public GameManager(
            IHubContext<GameHub> hubContext,
            GameStatisticBrokerClient statisticBrokerClient)
        {
            timer = new Timer(CreateGames, null, 0, 5 * 1000);
            HubContext = hubContext;
            this.statisticBrokerClient = statisticBrokerClient;
        }

        /// <summary>
        /// регистрация готового игрока
        /// </summary>
        public void RegisterGamer(IGamer gamer)
        {
            FreeGamers.Enqueue((Gamer)gamer);
        }

        /// <summary>
        /// регистрация бота-игрока
        /// </summary>
        public void RegisterBotGamer(IGamer gamer)
        {
            BotGamers.Enqueue((GamerBot)gamer);
        }


        public void AddNewGame(params IGamer[] gamers)
        {
            var game = new Game(HubContext, gamers);
            Games.Add(game.Uid, game);
            game.EndGameSubj.Subscribe(payload =>
            {
                Game game = Games[payload.GameUid]!;
                Games.Remove(payload.GameUid);
                SendBrokerWinnerMsg(game.Gamers);
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

        public void SetGamerEntityId(string connectionId, long entityId)
        {
            this.Games.Values.SelectMany(g => g.Gamers)
                .Concat(this.FreeGamers)
                .SingleOrDefault(g => g.EqualsConnectionId(connectionId))
                ?.SetUserEntityId(entityId);
        }

        /// <summary>
        /// Отправка сообщения на очередь в брокер
        /// </summary>
        /// <param name="gamers"></param>
        public void SendBrokerWinnerMsg(IEnumerable<IGamer> gamers)
        {
            foreach (var gamer in gamers)
            {
                if (!gamer.UserEntityId.HasValue)
                {
                    continue;
                }
                statisticBrokerClient.PushMsg(new InUserStatistics()
                {
                    GameCount = 1,
                    LossGames = gamer.IsWinner() ? 0 : 1,
                    UserId = gamer.UserEntityId.Value,
                    WinGames = gamer.IsWinner() ? 1 : 0,
                });
            }
        }

        /// <summary>
        /// в цикле создаёт игры
        /// </summary>
        /// <param name="stateInfo"></param>
        public void CreateGames(Object stateInfo)
        {
            if (BotGamers.Count > 0)
            {
                CreateGamesWithBot(stateInfo);
                CreateGames(stateInfo);
                return;
            }

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

        private void CreateGamesWithBot(Object stateInfo)
        {
            var gamers = new List<IGamer?>
            {
                GetRanadomGamer(),
                GeBotGamer(),
            };

            activeBotGamer = gamers.SingleOrDefault(x => x is GamerBot) as GamerBot;
            if (activeBotGamer != null)
            {
                activeBotGamer.ShootBotEvent += EvolveShoot;
            }

            AddNewGame(gamers.ToArray()!);
        }

        public Gamer? GetRanadomGamer()
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

        public GamerBot? GeBotGamer()
        {
            while (BotGamers.TryDequeue(out var gamer))
            {
                return gamer;
            }
            return null;
        }
    }
}
