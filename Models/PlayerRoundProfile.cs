using System;
using System.Collections.Generic;
using Models.Ranging;

namespace Models
{
    public class PlayerRoundProfile
    {
        public string Name { get; set; }
        public PositionEnum Position { get; set; }
        public bool InPositionAgainstHero { get; set; }
        public bool IsHero { get; set; }
        public int StackSize { get; set; }

        public PlayerStatusEnum PlayerStatus { get; set; }

        public bool IsAlive => PlayerStatus != PlayerStatusEnum.Folded;

        public PlayerRange PlayerRange { get; set; }

        public PlayerRange StartingRange { get; set; }
        public PlayerRange PreflopRange { get; set; }
        public PlayerRange FlopRange { get; set; }
        public PlayerRange TurnRange { get; set; }
        public PlayerRange RiverRange { get; set; }
        public List<Decision> PreflopDecisions { get; set; }
        public List<Decision> FlopDecisions { get; set; }
        public List<Decision> TurnDecisions { get; set; }
        public List<Decision> RiverDecisions { get; set; }
        public int PreflopBet { get; set; }
        public int FlopBet { get; set; }
        public int TurnBet { get; set; }
        public int RiverBet { get; set; }
        public PlayerStreetStatusEnum PreflopPlayerStatus { get; set; }
        public PlayerStreetStatusEnum FlopPlayerStatus { get; set; }
        public PlayerStreetStatusEnum TurnPlayerStatus { get; set; }
        public PlayerStreetStatusEnum RiverPlayerStatus { get; set; }
        public bool IsPreflopRaiser { get; set; }
        public bool IsFlopRaiser { get; set; }
        public bool IsTurnRaiser { get; set; }
        public bool IsRiverRaiser { get; set; }
    }
}
