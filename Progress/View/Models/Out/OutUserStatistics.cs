using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Models.Out
{
    public class OutUserStatistics
    {
        public long UserId { get; set; }

        public Level Level { get; set; }

        public int Rating { get; set; }

        public bool IsPrivileged { get; set; }

        public int GameCount { get; set; }

        public int WinGames { get; set; }
        public int LossGames { get; set; }
    }
}
