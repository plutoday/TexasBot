using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;
using TexasBot.Models;

namespace TexasBot.Games.Reporters
{
    public interface IGameReporter
    {
        void ReportHolesDealt(string playerName, List<Card> holes);
        void ReportDecision(PlayerDecision playerDecision);
        void ReportFlopsDealt(List<Card> flops);
        void ReportTurnDealt(Card turn);
        void ReportRiverDealt(Card river);
        void ShowBestHand(string playerName, HandOf5 bestHand);
        void ReportGainAndLoss(string playerName, int chipsGained);
        string Replay();
    }
}
