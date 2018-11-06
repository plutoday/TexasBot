using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
//using TexasBot.Models;
//using TexasBot.Models.Hands;
//using TexasBot.Models.HandSummaries;
//using TexasBot.Tools;
//using Card = TexasBot.Models.Card;

namespace Unittest
{
    [TestClass]
    public class UnitTest1
    {
        /*
        [TestMethod]
        public void TestMethod1()
        {
            var hands = TexasBot.Tools.Utils.GenerateAllPossibleHands();
            foreach (var handOf5 in hands.Where(h => h.Cards.Count != 5))
            {
                Console.WriteLine(handOf5);
            }
            var handCalculator = new HandCalculator();
            foreach (var handOf5 in hands)
            {
                handOf5.HandValue = handCalculator.CalculateHandValueFor5Cards(handOf5);
            }

            Dictionary<HandEnum, List<HandOf5>> handsDict = new Dictionary<HandEnum, List<HandOf5>>();

            foreach (var hand in hands)
            {
                if (!handsDict.ContainsKey(hand.HandValue.HandEnum))
                {
                    handsDict.Add(hand.HandValue.HandEnum, new List<HandOf5>());
                }

                handsDict[hand.HandValue.HandEnum].Add(hand);
            }

            foreach (var entry in handsDict)
            {
                var line = $"{entry.Key}|{entry.Value.Count}";
                Console.WriteLine(line);
            }

            hands.Sort();
            //hands.Reverse();
            int score = 1;
            hands[0].Score = score;
            for (int i = 0; i < hands.Count - 1; i++)
            {
                if (hands[i].CompareTo(hands[i + 1]) != 0)
                {
                    score++;
                }

                hands[i + 1].Score = score;
            }

            foreach (var handOf5 in hands)
            {
                var line = $"{handOf5.Score}|{handOf5.HandValue.HandEnum}|{handOf5.HandValue.Hand}";
                Console.WriteLine(line);
            }

            var summaries = hands.Select(h => new HandOf5Summary(h)).ToArray();
            Dictionary<string, HandOf5SummarySlimRecord> records = new Dictionary<string, HandOf5SummarySlimRecord>();
            foreach (var summary in summaries)
            {
                var record = new HandOf5SummarySlimRecord(summary.ToString(), summary.Score, 0);
                if (!records.ContainsKey(record.RecordString))
                {
                    records.Add(record.RecordString, record);
                }

                records[record.RecordString].Count++;
            }

            var results = records.Values.ToList();
            results.Sort((r1, r2) => -r1.Score.CompareTo(r2.Score));
            File.WriteAllText("records.json", JsonConvert.SerializeObject(results));
        }
        */

            /*
        [TestMethod]
        public void TestMethod15()
        {
            var allCards = TexasBot.Tools.Utils.GenerateAllCards().ToArray();
            var holesCombinations = TexasBot.Tools.Utils.Enumerate(allCards, 0, 2);



            Dictionary<string, HolesSummarySlimRecord> records = new Dictionary<string, HolesSummarySlimRecord>();
            foreach (var holesCombination in holesCombinations)
            {
                var summary = new HolesSummary(holesCombination);
                var record = new HolesSummarySlimRecord(summary.ToString(), 0);
                if (!records.ContainsKey(record.RecordString))
                {
                    records.Add(record.RecordString, record);
                }

                records[record.RecordString].Count++;
            }

            var results = records.Values.ToList();
            var handSummarySlimRecords = JsonConvert.DeserializeObject<List<HandOf5SummarySlimRecord>>(File.ReadAllText("records.json"));

            foreach (var holesSummarySlimRecord in results)
            {
                var possibleHandSummarySlimRecords =
                    handSummarySlimRecords.Where(r => r.IsPossibleWith(holesSummarySlimRecord));
                holesSummarySlimRecord.PossibleHandRecords = new List<HandOf5SummarySlimRecord>(possibleHandSummarySlimRecords);
            }

            var holesPairCompareResults = new ConcurrentBag<HolesPairCompareResult>();

            Parallel.ForEach(results, holes1 =>
            {
                var compareResults = new List<HolesPairCompareResult>();
                foreach (var holes2 in results)
                {
                    var holes1PossibleHands = handSummarySlimRecords.Where(r => r.IsPossibleWith(holes1))
                        .Where(r => r.IsPossibleWithout(holes2));
                    var holes2PossibleHands = handSummarySlimRecords.Where(r => r.IsPossibleWith(holes2))
                        .Where(r => r.IsPossibleWithout(holes1));
                    var compareResult = CompareTwoGroupsOfHands(holes1PossibleHands, holes2PossibleHands);
                    holesPairCompareResults.Add(new HolesPairCompareResult(holes1, holes2, compareResult));
                    compareResults.Add(new HolesPairCompareResult(holes1, holes2, compareResult));
                }
                File.AppendAllText("cr.json", JsonConvert.SerializeObject(compareResults));
            });

            File.WriteAllText("compareResults.json", JsonConvert.SerializeObject(holesPairCompareResults.ToList()));
        }
        */
        /*
        private CompareResult CompareTwoGroupsOfHands(IEnumerable<HandOf5SummarySlimRecord> group1,
            IEnumerable<HandOf5SummarySlimRecord> group2)
        {
            long win = 0, lose = 0, tie = 0;
            foreach (var hand1 in group1)
            {
                foreach (var hand2 in group2)
                {
                    int count = hand1.Count * hand2.Count;
                    if (hand1.Score > hand2.Score)
                    {
                        win += count;
                    }
                    else if (hand1.Score < hand2.Score)
                    {
                        lose += count;
                    }
                    else
                    {
                        tie += count;
                    }
                }
            }

            return new CompareResult(win, lose, tie);
        }
        */

            /*
        [TestMethod]
        public void TestMethod2()
        {
            var allCards = TexasBot.Tools.Utils.GenerateAllCards().ToArray();
            var holesCombinations = TexasBot.Tools.Utils.Enumerate(allCards, 0, 2);

            var holesRecords = new List<HolesRecord>();

            foreach (var holes in holesCombinations)
            {
                var holesRecord = new HolesRecord(holes);
                var otherCards = allCards.Except(holes).ToArray();
                var opponentHolesCombinations = TexasBot.Tools.Utils.Enumerate(otherCards, 0, 2);

                foreach (var opponentHoles in opponentHolesCombinations)
                {
                    EveluateTwoHolesCombinations(holes.ToList(), opponentHoles.ToList(), holesRecord);
                }

                holesRecords.Add(holesRecord);
            }

            var json = JsonConvert.SerializeObject(holesRecords);
            File.WriteAllText("holesRecords.json", json);
        }

        private void EveluateTwoHolesCombinations(List<Card> myHoles, List<Card> oppHoles, HolesRecord myHolesRecord)
        {
            var otherCards = TexasBot.Tools.Utils.GenerateAllCards().Except(myHoles).Except(oppHoles).ToArray();
            var fourCardsCombinations = TexasBot.Tools.Utils.Enumerate(otherCards, 0, 4);
            foreach (var fourCardsCombination in fourCardsCombinations)
            {
                var mySixCards = new List<Card>();
                mySixCards.AddRange(myHoles);
                mySixCards.AddRange(fourCardsCombination);
                mySixCards = TexasBot.Tools.Utils.SortCards(mySixCards);
                var myHand = new HandOf7(mySixCards);
                var myBestHand = myHand.FindBestHandOf5();
                myBestHand.Score = TexasBot.Tools.Utils.GetScoreFor5Cards(myBestHand);

                var oppSixCards = new List<Card>();
                oppSixCards.AddRange(oppHoles);
                oppSixCards.AddRange(fourCardsCombination);
                oppSixCards = TexasBot.Tools.Utils.SortCards(oppSixCards);
                var oppHand = new HandOf7(oppSixCards);
                var oppBestHand = oppHand.FindBestHandOf5();
                oppBestHand.Score = TexasBot.Tools.Utils.GetScoreFor5Cards(oppBestHand);

                if (myBestHand.Score > oppBestHand.Score)
                {
                    myHolesRecord.WinScenarioCount++;
                }
                else if (myBestHand.Score < oppBestHand.Score)
                {
                    myHolesRecord.LoseScenarioCount++;
                }
                else
                {
                    myHolesRecord.TieScenarioCount++;
                }
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            var allCards = TexasBot.Tools.Utils.GenerateAllCards().ToArray();

            var sevenCardsCombinations = new List<IEnumerable<Card>>();
            foreach (var sevenCardsCombination in TexasBot.Tools.Utils.Enumerate(allCards, 0, 7))
            {
                sevenCardsCombinations.Add(sevenCardsCombination);
            }

            var dict = new Dictionary<string, HandOf7SummarySlimRecord>();

            //Parallel.ForEach(sevenCardsCombinations, sevenCardsCombination =>
            foreach(var sevenCardsCombination in sevenCardsCombinations)
            {
                var handOf7 = new HandOf7(sevenCardsCombination.ToList());
                var handOf7Summary = new HandOf7Summary(handOf7);

                if (!dict.ContainsKey(handOf7Summary.ToString()))
                {
                    var bestHandOf5 = handOf7.FindBestHandOf5();
                    var handOf7SummarySlimRecord = new HandOf7SummarySlimRecord()
                    {
                        HandOf5RecordString = new HandOf5Summary(bestHandOf5).ToString(),
                        RecordString = handOf7Summary.ToString(),
                        Score = bestHandOf5.Score,
                        HandEnum = bestHandOf5.HandValue.HandEnum,
                        Count = 1
                    };

                    dict.Add(handOf7Summary.ToString(), handOf7SummarySlimRecord);
                }
                else
                {
                    dict[handOf7Summary.ToString()].Count++;
                }
            }
            //);

            var json = JsonConvert.SerializeObject(dict.Values.ToList());
            File.WriteAllText("handOf7Records.json", json);
        }*/
    }
}
