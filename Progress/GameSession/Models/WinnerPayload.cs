namespace GameSession.Models
{
    public class WinnerPayload
    {
        public Guid GameUid { get; set; }
        public Gamer WinnerGamer { get; set; }
        public WinnerPayload(Guid gameUid, Gamer winnerGamer)
        {
            this.GameUid = gameUid;
            this.WinnerGamer = winnerGamer;
        }
    }
}
