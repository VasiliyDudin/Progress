using Contracts.DTO;
using Contracts.Enums;
using GameSession.Models;
using GameSession.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameSession.Hubs
{
    public class GameHub : Hub
    {
        Queue<Gamer> userGameRadyToGameConnectIds = new Queue<Gamer>();

        public GameManager GameManager { get; }

        public GameHub(GameManager gameManager)
        {
            GameManager = gameManager;
        }
        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Responce", $"привет {Context.ConnectionId}!!"); ;
        }

        public async Task GameStart(MessageDto<IEnumerable<ShipDto>> msg)
        {

            await ResponceCaller(new MessageDto<bool>(msg.Uid, true));
            var currentGamer = new Gamer(Context.ConnectionId, msg.Payload);
            var otherGamer = userGameRadyToGameConnectIds.Dequeue();
            /// Начинаем игру
            if (otherGamer != null)
            {
                var game = GameManager.AddNewGame(currentGamer, otherGamer);
                await Groups.AddToGroupAsync(Context.ConnectionId, game.Uid.ToString());
                await Groups.AddToGroupAsync(otherGamer.ConnetcionId, game.Uid.ToString());
                await Clients.Client(otherGamer.ConnetcionId).SendAsync("StartGame", Context.ConnectionId);
                await Clients.Caller.SendAsync("StartGame", otherGamer.ConnetcionId);
            }
            else
            {
                userGameRadyToGameConnectIds.Enqueue(currentGamer);
            }
        }

        public async Task Shoot(MessageDto<CoordinateSimple> msg)
        {
            var result = GameManager.EvolveShoot(Context.ConnectionId, msg.Payload);
            await Clients.Group(result.GameUid.ToString()).SendAsync("ResultShoot", msg.CreateAnswer(result.ShootStatus));
        }

        private async Task ResponceCaller<T>(MessageDto<T> msg)
        {
            await Clients.Caller.SendAsync("Responce", msg);
        }
    }
}
