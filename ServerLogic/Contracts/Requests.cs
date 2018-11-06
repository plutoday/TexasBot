using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Contracts
{
    public class StartNewRoundRequest
    {
        public List<Player> Players { get; set; }
        public int HeroIndex { get; set; }
        public int ButtonIndex { get; set; }
        public int BigBlindSize { get; set; }
        public int SmallBlindSize { get; set; }
    }

    public class NotifyHeroHolesRequest
    {
        public Guid RoundId { get; set; }
        public List<Card> Holes { get; set; }
    }

    public class NotifyFlopsRequest
    {
        public List<Card> Flops { get; set; }

        public Guid RoundId { get; set; }
    }

    public class NotifyTurnRequest
    {
        public Card Turn { get; set; }
        public Guid RoundId { get; set; }        
    }

    public class NotifyRiverRequest
    {
        public Card River { get; set; }
        public Guid RoundId { get; set; }
    }

    public class NotifyDecisionRequest
    {
        public Decision Decision { get; set; }
        public string PlayerName { get; set; }
        public Guid RoundId { get; set; }
    }

    public class Player
    {
        public string Name { get; set; }
        public decimal? StackSize { get; set; }  
        public bool SittingOut { get; set; }      
    }
}
