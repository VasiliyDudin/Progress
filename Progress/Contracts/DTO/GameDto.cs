using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class ShootResultDto
    {
        public Guid GameUid { get; set; }
        public EShootStatus ShootStatus { get; set; }
        public string SourceGamerConnectionId { get; set; }
        public string TargetGamerConnectionId { get; set; }
        public string NextGamerShooterConnectionId { get; set; }

        public CoordinateSimple Coordinate { get; set; }
    }

    public class InitGameDto
    {
        /// <summary>
        /// ID всех игроков
        /// </summary>
        public string[] AllGamerIds { get; set; }

        /// <summary>
        ///  ID игрока чей выстрел
        /// </summary>
        public string ShootGamerId { get; set; }
    }
}
