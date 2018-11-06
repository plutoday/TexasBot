using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Coaching.Postflop
{
    public class PostflopPlayerSummary
    {
        public bool IsMe { get; set; }
        public HoldingHoles Holes { get; set; }
        public string Name { get; set; }
        public PositionEnum Position { get; set; }
        public bool IsAlive { get; set; }
        public List<Decision> PreflopDecisions { get; set; }
        public List<Decision> FlopDecisions { get; set; }
        public List<Decision> TurnDecisions { get; set; }
        public List<Decision> RiverDecisions { get; set; } 
        public int ChipsBet { get; set; }
        public int StackSize { get; set; }
        public string Tag { get; set; }

        public InRoundRole InRoundRole { get; set; }
    }

    /// <summary>
    /// Caller/Raiser is regarding the previous round
    /// </summary>
    public enum InRoundRole
    {
        
        OopCaller,
        OopRaiser,
        IpCaller,
        IpRaiser,
    }
}
