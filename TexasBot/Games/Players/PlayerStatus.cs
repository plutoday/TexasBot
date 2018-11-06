using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Models;

namespace TexasBot.Games.Players
{
    public class PlayerStatus
    {
        public int Chips { get; set; }
        public string PlayerName { get; set; }

        public PlayerStatus(int chips, string playerName)
        {
            Chips = chips;
            PlayerName = playerName;
        }
    }
}
