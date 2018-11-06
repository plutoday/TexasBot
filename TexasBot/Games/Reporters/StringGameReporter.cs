using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexasBot.Games.Players;
using TexasBot.Models;
using TexasBot.Tools;

namespace TexasBot.Games.Reporters
{
    public class StringGameReporter : IGameReporter
    {
        public Dictionary<string, int> Gains = new Dictionary<string, int>(); 

        private StringBuilder _sb;

        char club = '\u2663';
        char spade = '\u2660';
        char diamond = '\u2666';
        char heart = '\u2665';

        public void ReportHolesDealt(string playerName, List<Card> holes)
        {
            if (_sb == null)
            {
                _sb = new StringBuilder();
            }

            _sb.AppendLine($"{playerName} is dealt {string.Join("", holes.Select(TranslateCardString))}");
        }

        private string TranslateCardString(Card card)
        {
            var cardString = Utils.GetStringForCard(card);
            return cardString.Replace('D', diamond).Replace('S', spade).Replace('H', heart).Replace('C', club);
        }

        public void ReportDecision(PlayerDecision playerDecision)
        {
            _sb.Append($"{playerDecision.PlayerName} {playerDecision.Decision.DecisionEnum} ");
            if (playerDecision.Decision.DecisionEnum == DecisionEnum.Call ||
                playerDecision.Decision.DecisionEnum == DecisionEnum.Raise)
            {
                _sb.AppendLine($"{playerDecision.Decision.Chips}");
            }
            else
            {
                _sb.AppendLine();
            }
        }

        public void ReportFlopsDealt(List<Card> flops)
        {
            _sb.AppendLine($"flops are {string.Join("", flops.Select(TranslateCardString))}");
        }

        public void ReportTurnDealt(Card turn)
        {
            _sb.AppendLine($"turn is {TranslateCardString(turn)}");
        }

        public void ReportRiverDealt(Card river)
        {
            _sb.AppendLine($"river is {TranslateCardString(river)}");
        }

        public void ShowBestHand(string playerName, HandOf5 bestHand)
        {
            _sb.AppendLine($"{playerName} got {bestHand.HandValue.HandEnum}/{string.Join("", bestHand.Cards.Select(TranslateCardString))}/score={bestHand.Score}");
        }

        public void ReportGainAndLoss(string playerName, int chipsGained)
        {
            if (!Gains.ContainsKey(playerName))
            {
                Gains.Add(playerName, 0);
            }

            Gains[playerName] += chipsGained;

            _sb.AppendLine(chipsGained > 0 ? $"{playerName} wins {chipsGained}" : $"{playerName} loses {-chipsGained}");
        }

        public string Replay()
        {
            var replay = _sb.ToString();
            _sb = null;
            return replay;
        }
    }
}
