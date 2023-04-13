using Contracts.DTO;
using GameSession.Hubs;
using GameSession.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameSession.Services
{

    public class GameManager
    {
        private Timer timer;
        private Random random = new Random();

        /// <summary>
        /// текущие игры
        /// </summary>
        private IList<Game> Games = new List<Game>();

        /// <summary>
        /// свободные игроки
        /// </summary>
        private ConcurrentQueue<Gamer> FreeGamers = new ConcurrentQueue<Gamer>();

        public IHubContext<GameHub> HubContext { get; }

        public GameManager(IHubContext<GameHub> hubContext)
        {
            timer = new Timer(CreateGames, null, 0, 5 * 1000);
            HubContext = hubContext;
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
            Games.Add(game);
        }

        public ShootResultDto EvolveShoot(string shootGamerConnectionId, CoordinateSimple coordinateShoot)
        {
            var game = Games.Single(g => g.IsExistGamer(shootGamerConnectionId));
            var status = game.SendShoot(shootGamerConnectionId, coordinateShoot);
            var otherGamer = game.GetOtherGamer(shootGamerConnectionId);
            return new ShootResultDto
            {
                ShootStatus = status,
                SourceGamerConnectionId = shootGamerConnectionId,
                TargetGamerConnectionId = otherGamer.ConnectionId,
                GameUid = game.Uid,
                NextGamerShooterConnectionId = game.GetShooterGamer().ConnectionId,
                Coordinate = coordinateShoot
            };
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
                 GetRnadomGamer(),
                 GetRnadomGamer()
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

        private Gamer? GetRnadomGamer()
        {
            while (FreeGamers.TryDequeue(out var gamer))
                return gamer;
            return null;
        }
    }
}
