using System.Collections.Generic;
using Models;

namespace Preflop
{
    public class PreflopPlayerSummary
    {
        public PositionEnum Position { get; set; }

        public PlayerStatusEnum PlayerStatus { get; set; }
        public List<Decision> Decisions { get; set; }
        public string Tag { get; set; }
        public int StackSize { get; set; }

        public string Name { get; set; }
    }
}
