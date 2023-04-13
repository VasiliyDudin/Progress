using Contracts.DTO;
using Contracts.Enums;
using System;
using Microsoft.AspNetCore.SignalR;
using GameSession.Hubs;
using System.Text.RegularExpressions;

namespace GameSession.Models
{
    public class Game
    {
        public IHubContext<GameHub> HubContext { get; }
        IEnumerable<Gamer> Gamers { get; set; }
        public Guid Uid { get; set; } = Guid.NewGuid();

        private string HubGroupName { get => this.Uid.ToString(); }

        public Game(IHubContext<GameHub> hubContext, params Gamer[] gamers)
        {
            HubContext = hubContext;
            Gamers = gamers;
            var rand = new Random();
            gamers[rand.Next(1)].IsShooted = true;
            InitGameHub();
        }

        async void InitGameHub()
        {
            foreach (var gamer in Gamers)
            {
                await HubContext.Groups.AddToGroupAsync(gamer.ConnectionId, HubGroupName);
            }
            await HubContext.Clients.Group(HubGroupName).SendAsync("StartGame", new InitGameDto()
            {
                AllGamerIds = Gamers.Select(g => g.ConnectionId).ToArray(),
                ShootGamerId = GetShooterGamer().ConnectionId
            });
        }

        public EShootStatus SendShoot(string shootGamerConnectionId, CoordinateSimple coordinateShoot)
        {
            var otherGamer = GetOtherGamer(shootGamerConnectionId);
            var shootGamer = GetShooterGamer(shootGamerConnectionId).AddHistory(coordinateShoot);

            var status = otherGamer.EvolveShoot(coordinateShoot);
            if (status.IsMissing())
            {
                otherGamer.IsShooted = true;
                shootGamer.IsShooted = false;
            }
            return status;
        }


        public bool IsExistGamer(string gamerConnectionId)
        {
            return Gamers.Any(g => g.ConnectionId.Equals(gamerConnectionId));
        }

        public Gamer GetOtherGamer(string shootGamerConnectionId)
        {
            return Gamers.Single(g => !g.ConnectionId.Equals(shootGamerConnectionId));
        }
        public Gamer GetShooterGamer(string shootGamerConnectionId)
        {
            return Gamers.Single(g => g.ConnectionId.Equals(shootGamerConnectionId));
        }
        public Gamer GetShooterGamer()
        {
            return Gamers.Single(g => g.IsShooted);
        }
    }

}
