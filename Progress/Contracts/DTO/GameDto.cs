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
    }

    public class InitGameDto
    {
        public string OtherGamerConnectionId { get; set; }
        public string ShootGamerConnectionId { get; set; }
    }
}
