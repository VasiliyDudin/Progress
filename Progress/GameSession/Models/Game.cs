using Contracts.DTO;
using Contracts.Enums;

namespace GameSession.Models
{
    public class Game
    {
        IEnumerable<Gamer> Gamers { get; set; }
        public Guid Uid { get; set; } = Guid.NewGuid();

        public Game(params Gamer[] gamers)
        {
            Gamers = gamers;
            var rand = new Random();
            gamers[rand.Next(1)].IsShooted = true;
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
            return Gamers.Any(g => g.ConnetcionId.Equals(gamerConnectionId));
        }

        public Gamer GetOtherGamer(string shootGamerConnectionId)
        {
            return Gamers.Single(g => !g.ConnetcionId.Equals(shootGamerConnectionId));
        }
        public Gamer GetShooterGamer(string shootGamerConnectionId)
        {
            return Gamers.Single(g => g.ConnetcionId.Equals(shootGamerConnectionId));
        }
        public Gamer GetShooterGamer()
        {
            return Gamers.Single(g => g.IsShooted);
        }
    }

}
