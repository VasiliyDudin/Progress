using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Enums
{
    public enum EShootStatus
    {

        /// <summary>
        /// Ошибка
        /// </summary>
        Error = -1,

        Unknown = 0,

        /// <summary>
        /// Промах 
        /// </summary>
        Miss = 1,

        /// <summary>
        /// Попадание в корабль
        /// </summary>
        Hit = 2,

        /// <summary>
        /// Убийство корабля
        /// </summary>
        Killing = 3,

        /// <summary>
        /// Убийство всех кораблей
        /// </summary>
        KillingAll = 4,


    }

    public static class EShootStatusHalper
    {
        public static bool IsMissing(this EShootStatus shootStatus)
        {
            {
                return shootStatus == EShootStatus.Miss;
            }
        }
        public static bool IsHit(this EShootStatus shootStatus)
        {
            {
                return shootStatus == EShootStatus.Hit;
            }
        }
        public static bool IsKilling(this EShootStatus shootStatus)
        {
            {
                return shootStatus == EShootStatus.Killing;
            }
        }
    }
}
