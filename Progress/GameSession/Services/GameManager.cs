using Contracts.DTO;
using GameSession.Models;

namespace GameSession.Services
{

    public class GameManager
    {
        IList<Game> Games = new List<Game>();

        public Game AddNewGame(Gamer gamer1, Gamer gamer2)
        {
            var game = new Game(gamer1, gamer2);
            Games.Add(game);
            return game;
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
                TargetGamerConnectionId = otherGamer.ConnetcionId,
                GameUid = game.Uid,
                NextGamerShooterConnectionId = game.GetShooterGamer().ConnetcionId,
                Coordinate = coordinateShoot
            };
        }
    }
}
