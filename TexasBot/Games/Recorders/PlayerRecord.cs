using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasBot.Games.Recorders
{
    public class PlayerRecord
    {
        public string Name { get; set; }
        public int ChipsBet { get; set; }
        public bool AllIn { get; set; }
        public bool Folded { get; set; }
        public bool Polled { get; set; }

        public void ResetForNewStage()
        {
            Polled = false;
        }

        public PlayerRecord(string playerName)
        {
            Name = playerName;
            ChipsBet = 0;
            AllIn = false;
            Folded = false;
            Polled = false;
        }
    }
}
