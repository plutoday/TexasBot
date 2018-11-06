using System.Collections.Generic;
using Coaching.Postflop.Boards;
using Models;

namespace Coaching.Postflop
{
    public class PostflopStatusSummary
    {
        public List<PostflopPlayerSummary> AliveVillains { get; set; } 
        public PostflopPlayerSummary Me { get; set; }
        public PostflopPlayerSummary Raiser { get; set; }
        public PositionEnum PreflopRaiserPosition { get; set; }
        public int PotSize { get; set; }
        public int BigBlindSize { get; set; }
        public int ChipsToCall { get; set; }
        public bool IsRaised { get; set; }
        public BoardStatus BoardStatus { get; set; }
    }
}
