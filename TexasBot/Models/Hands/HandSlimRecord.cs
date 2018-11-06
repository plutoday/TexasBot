using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Tools;

namespace TexasBot.Models.Hands
{
    public class HandSlimRecord
    {
        public string HandString { get; set; }

        public int Score { get; set; }

        public HandSlimRecord() { }

        public HandSlimRecord(HandOf5 hand)
        {
            Score = hand.Score;
            HandString = Utils.GetStringForCards(hand.Cards);
        }
    }
}
