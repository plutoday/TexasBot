using Models;
using System.Collections.Generic;

namespace Preflop
{
    public class PreflopStatusSummary
    {
        public PreflopGameStatusEnum Status { get; set; }
        public int PotSize { get; set; }

        public PreflopPlayerSummary CurrentRaiser { get; set; }

        /// <summary>
        /// 最后一个raise需要多少才能Call
        /// </summary>
        public int ChipsToCall { get; set; }

        public bool IsRaised { get; set; }

        public int BigBlindSize { get; set; }

        /*
        public PreflopPlayerSummary SmallBlind { get; set; }
        public PreflopPlayerSummary BigBlind { get; set; }
        public PreflopPlayerSummary UnderTheGun { get; set; }
        public PreflopPlayerSummary MiddlePosition { get; set; }
        public PreflopPlayerSummary CuttingOff { get; set; }
        public PreflopPlayerSummary Button { get; set; }
        */

        public PreflopPlayerSummary Me { get; set; }

        public List<PreflopPlayerSummary> Players { get; set; }

        public List<Move> PreflopRaiseMoves { get; set; }

        public PreflopStatusSummary()
        {
            Players = new List<PreflopPlayerSummary>();
            PreflopRaiseMoves = new List<Move>();
        }
    }
}
