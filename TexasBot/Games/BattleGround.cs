using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TexasBot.Models;
using TexasBot.Models.HandSummaries;
using TexasBot.Tools;

namespace TexasBot.Games
{
    public class BattleGround
    {
        private List<Card> _flops;

        private Card _turn;
        private Card _river;

        private static readonly List<HolesPairCompareResult> Records
            = JsonConvert.DeserializeObject<List<HolesPairCompareResult>>(File.ReadAllText(@"D:\TexasBot\compareResults.json"));

        public void SetFlops(List<Card> flops)
        {
            if (flops.Count != 3)
            {
                throw new InvalidOperationException();
            }

            _flops = new List<Card>(flops);
        }

        public void SetTurn(Card turn)
        {
            _turn = turn;
        }

        public void SetRiver(Card river)
        {
            _river = river;
        }

        public CompareResult Fight(Fighter host, Fighter guest)
        {
            if (_flops == null)
            {
                return Draw5CardsAndFight(host, guest);
            }

            if (_turn == null)
            {
                return Draw2CardsAndFight(host, guest);
            }

            if (_river == null)
            {
                return Draw1CardAndFight(host, guest);
            }

            return NoDrawFight(host, guest);
        }

        private CompareResult Draw5CardsAndFight(Fighter host, Fighter guest)
        {
            var holes1SummaryString = new HolesSummary(host.Holes).ToString();
            var holes2SummaryString = new HolesSummary(guest.Holes).ToString();

            HolesPairCompareResult record = null;
            try
            {
                record = Records.First(r => (string.Equals(holes1SummaryString, r.Holes1.RecordString)
                && string.Equals(holes2SummaryString, r.Holes2.RecordString)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return record.CompareResult;
        }

        private CompareResult Draw2CardsAndFight(Fighter host, Fighter guest)
        {
            var excluded = new List<Card>();
            excluded.AddRange(host.Holes);
            excluded.AddRange(guest.Holes);
            excluded.AddRange(_flops);

            var allCards = Utils.GenerateAllCards().Except(excluded).ToArray();
            var twoCardsCombinations = Utils.Enumerate(allCards, 0, 2);

            var compareResult = new CompareResult(0, 0, 0);
            foreach (var twoCardsCombination in twoCardsCombinations)
            {
                var communityCards = new List<Card>();
                communityCards.AddRange(_flops);
                communityCards.AddRange(twoCardsCombination);
                var battleSetup = new BattleSetup(communityCards);

                compareResult.Add(FightBattleSetup(host, guest, battleSetup));
            }

            return compareResult;
        }

        private CompareResult Draw1CardAndFight(Fighter host, Fighter guest)
        {
            var excluded = new List<Card>();
            excluded.AddRange(host.Holes);
            excluded.AddRange(guest.Holes);
            excluded.AddRange(_flops);
            excluded.Add(_turn);

            var allCards = Utils.GenerateAllCards().Except(excluded).ToArray();

            var compareResult = new CompareResult(0, 0, 0);
            foreach (var river in allCards)
            {
                var communityCards = new List<Card>();
                communityCards.AddRange(_flops);
                communityCards.Add(_turn);
                communityCards.Add(river);
                var battleSetup = new BattleSetup(communityCards);

                compareResult.Add(FightBattleSetup(host, guest, battleSetup));
            }

            return compareResult;
        }

        private CompareResult NoDrawFight(Fighter host, Fighter guest)
        {
            var communityCards = new List<Card>();
            communityCards.AddRange(_flops);
            communityCards.Add(_turn);
            communityCards.Add(_river);
            var battleSetup = new BattleSetup(communityCards);

            return FightBattleSetup(host, guest, battleSetup);
        }

        private CompareResult FightBattleSetup(Fighter host, Fighter guest, BattleSetup battleSetup)
        {
            var hostCards = new List<Card>();
            hostCards.AddRange(host.Holes);
            hostCards.AddRange(battleSetup.CommunityCards);
            var hostHandOf7 = new HandOf7(hostCards);

            var guestCards = new List<Card>();
            guestCards.AddRange(guest.Holes);
            guestCards.AddRange(battleSetup.CommunityCards);
            var guestHandOf7 = new HandOf7(guestCards);

            var hostBestHandScore = Utils.FindBestHandOf5(hostHandOf7);
            var guestBestHandScore = Utils.FindBestHandOf5(guestHandOf7);

            if (hostBestHandScore > guestBestHandScore)
            {
                return new CompareResult(1, 0, 0);
            }
            else if (hostBestHandScore < guestBestHandScore)
            {
                return new CompareResult(0, 1, 0);
            }
            else
            {
                return new CompareResult(0, 0, 1);
            }
        }
    }
}
