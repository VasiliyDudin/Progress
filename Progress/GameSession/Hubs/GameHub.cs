using Contracts.DTO;
using GameSession.Models;
using GameSession.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameSession.Hubs
{
    public class GameHub : Hub
    {
        public GameManager GameManager { get; }

        public GameHub(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("InitConnection", Context.ConnectionId);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            GameManager.RemoveGamer(Context.ConnectionId);
        }

        /// <summary>
        /// старт игры
        /// </summary>
        public async Task GameStart(MessageDto<IEnumerable<ShipDto>> msg)
        {

            await Answer(new MessageDto<bool>(msg.Uid, true));
            GameManager.RegisterGamer(new Gamer(Context.ConnectionId, msg.Payload));
        }

        /// <summary>
        /// выстрел игрока
        /// </summary>
        public async Task Shoot(MessageDto<CoordinateSimple> msg)
        {
            await GameManager.EvolveShoot(Context.ConnectionId, msg.Payload);
        }

        /// <summary>
        /// выставляем сопоставление connetionId Id пользователю
        /// </summary>
        public Task SetUserId(MessageDto<long> msg)
        {
            GameManager.SetGamerEntityId(Context.ConnectionId, msg.Payload);
            return Task.CompletedTask;
        }

        /// <summary>
        /// ответ на запрос
        /// </summary>
        private async Task Answer<T>(MessageDto<T> msg)
        {
            await Clients.Caller.SendAsync("Answer", msg);
        }
    }
}
