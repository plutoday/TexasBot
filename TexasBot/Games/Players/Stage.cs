using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Games.Players
{
    public class Stage
    {
        public GameStage GameStage { get; set; }
        public List<PlayerDecision> PlayerDecisions { get; set; }

        public Stage(GameStage stage)
        {
            GameStage = stage;
            PlayerDecisions = new List<PlayerDecision>();
        }
    }
}
