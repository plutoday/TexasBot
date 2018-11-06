using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace River
{
    public class RiverDecisionContext
    {
        public HoldingHoles HeroHoles { get; set; }
        public int CurrentPotSize { get; set; }
        public int BigBlindSize { get; set; }
        public List<PlayerRoundProfile> Players { get; set; }
        public string HeroName { get; set; }
        public string PreflopRaiserName { get; set; }
        public string FlopRaiserName { get; set; }
        public string TurnRaiserName { get; set; }
        public string RiverRaiserName { get; set; }
        public string HeadsUpVillainName { get; set; }
        public PlayerRoundProfile Hero => Players.First(p => string.Equals(p.Name, HeroName));
        public PlayerRoundProfile PreflopRaiser => Players.First(p => string.Equals(p.Name, PreflopRaiserName));
        public PlayerRoundProfile FlopRaiser => Players.FirstOrDefault(p => string.Equals(p.Name, FlopRaiserName));
        public PlayerRoundProfile TurnRaiser => Players.FirstOrDefault(p => string.Equals(p.Name, TurnRaiserName));
        public PlayerRoundProfile RiverRaiser => Players.FirstOrDefault(p => string.Equals(p.Name, RiverRaiserName));
        public PlayerRoundProfile HeadsUpVillain => Players.First(p => string.Equals(p.Name, HeadsUpVillainName));
        public RiverBoard RiverBoard { get; set; }
        public bool IsRaised { get; set; }

        //todo consider all-in players
        public List<PlayerRoundProfile> AliveVillains => Players.Where(p => p.IsAlive && p.Name != HeroName).ToList();

        public bool IsHeadsUp { get; set; }
    }
}
