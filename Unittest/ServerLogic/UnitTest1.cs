using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using ServerLogic;
using ServerLogic.Contracts;
using Player = ServerLogic.Contracts.Player;

namespace Unittest.ServerLogic
{
    [TestClass]
    public class UnitTest1
    {
        private RoundManager _rm = RoundManager.Instance;
        [TestMethod]
        public void Case1()
        {
            //cc dc g ff ac bc cf df g ac bc
            var roundId = _rm.StartNewRound(GenerateNewRoundRequest(4, 5)).RoundId;
            _rm.NotifyHeroHoles(GenerateHeroHolesRequest(roundId, SuitEnum.Heart, RankEnum.King,
                SuitEnum.Heart, RankEnum.Jack));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Chris", DecisionType.Call, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "David", DecisionType.Call, 0));
            var decision = _rm.GetDecision(roundId);
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Frank", DecisionType.Fold, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Alice", DecisionType.Call, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Bob", DecisionType.Call, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Chris", DecisionType.Fold, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "David", DecisionType.Fold, 0));

            _rm.NotifyFlops(GenerateFlopsRequest(roundId, SuitEnum.Club, RankEnum.Seven,
                SuitEnum.Club, RankEnum.Eight, SuitEnum.Club, RankEnum.Nine));

            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Alice", DecisionType.Check, 0));
            _rm.NotifyDecision(GenerateNotifyDecisionRequest(roundId, "Bob", DecisionType.Check, 0));
            decision = _rm.GetDecision(roundId);
            Assert.IsNotNull(decision);
        }

        [TestMethod]
        public void Test()
        {
            ParseAndValidate(GenerateCase1());
            ParseAndValidate(GenerateCase2());
            ParseAndValidate(GenerateCase3());
            ParseAndValidate(GenerateCase4());
            
            ParseAndValidate(GenerateCaseSittingOut());
        }

        private IEnumerable<string> GenerateCase1()
        {
            return new[]
            {
                "Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/4/5",

                "Holes-HK-HJ",
                "Decision-Chris-Call",
                "Decision-David-Reraise-20",
                "GetDecision-Reraise-17",
                "Decision-Frank-Fold",
                "Decision-Alice-Call",
                "Decision-Bob-Call",
                "Decision-Chris-Fold",
                "Decision-David-Call",  
                "Flops-DK-D2-D5",   //Alice/Bob/David/Ellen(Hero)
                "Decision-Alice-Check",
                "Decision-Bob-Check",
                "Decision-David-Check",
                "GetDecision-Check",
                "Turn-D6",
                "Decision-Alice-Check",
                "Decision-Bob-Raise-50",
                "Decision-David-Fold",
                "GetDecision-Fold",
            };
        }

        private IEnumerable<string> GenerateCase2()
        {
            return new[]
            {
                "Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/4/5",

                "Holes-HK-HJ",
                "Decision-Chris-Call",
                "Decision-David-Reraise-20",
                "GetDecision-Reraise-17",
                "Decision-Frank-Fold",
                "Decision-Alice-Call",
                "Decision-Bob-Call",
                "Decision-Chris-Fold",
                "Decision-David-Call",
                "Flops-DK-D2-S5",   //Alice/Bob/David/Ellen(Hero)
                "Decision-Alice-Check",
                "Decision-Bob-Check",
                "Decision-David-Check",
                "GetDecision-Check",
                "Turn-D6",
                "Decision-Alice-Check",
                "Decision-Bob-Raise-10",
                "Decision-David-Fold",
                "GetDecision-Fold",
            };
        }

        private IEnumerable<string> GenerateCase3()
        {
            return new[]
            {
                "Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/2/5",

                "Holes-HT-H9",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Ellen-Fold",
                "Decision-Frank-Call",
                "Decision-Alice-Fold",
                "Decision-Bob-Call",
                "Flops-H8-H7-H6",
                "Decision-Bob-Check",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Frank-Call",
                "Decision-Bob-Fold",
                "Turn-D6",
                "GetDecision-Raise",
            };
        }

        private IEnumerable<string> GenerateCase4()
        {
            return new[]
            {
                "Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/2/3",

                "Holes-HT-H9",
                "Decision-Alice-Call",
                "Decision-Bob-Call",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Ellen-Call",
                "Decision-Frank-Call",
                "Decision-Alice-Call",
                "Decision-Bob-Call",
                "Flops-H8-H7-H6",
                "Decision-Ellen-Check",
                "Decision-Frank-Check",
                "Decision-Alice-Check",
                "Decision-Bob-Check",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Ellen-Call",
                "Decision-Frank-Call",
                "Decision-Alice-Call",
                "Decision-Bob-Call",
                "Turn-D6",
                "Decision-Ellen-Check",
                "Decision-Frank-Check",
                "Decision-Alice-Check",
                "Decision-Bob-Check",
                "GetDecision-Raise",
            };
        }

        private IEnumerable<string> GenerateCaseSittingOut()
        {
            return new[]
            {
                "Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/2/3-1",

                "Holes-HT-H9",
                "Decision-Alice-Call",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Ellen-Call",
                "Decision-Frank-Call",
                "Decision-Alice-Call",
                "Flops-H8-H7-H6",
                "Decision-Ellen-Check",
                "Decision-Frank-Check",
                "Decision-Alice-Check",
                "GetDecision-Raise",
                "Decision-David-Call",
                "Decision-Ellen-Call",
                "Decision-Frank-Call",
                "Decision-Alice-Call",
                "Turn-D6",
                "Decision-Ellen-Check",
                "Decision-Frank-Check",
                "Decision-Alice-Check",
                "GetDecision-Raise",
            };
        }

        private void ParseAndValidate(IEnumerable<string> insturctions)
        {
            Guid roundId = Guid.Empty;
            foreach (var ins in insturctions)
            {
                var parts = ins.Split('-');
                switch (parts[0])
                {
                    case "Setup":
                        ProcessSetup(parts, out roundId);
                        break;
                    case "Holes":
                        ProcessHoles(parts, roundId);
                        break;
                    case "Decision":
                        ProcessDecision(parts, roundId);
                        break;
                    case "GetDecision":
                        ProcessGetDecision(parts, roundId);
                        break;
                    case "Flops":
                        ProcessFlops(parts, roundId);
                        break;
                    case "Turn":
                        ProcessTurn(parts, roundId);
                        break;
                    case "River":
                        ProcessRiver(parts, roundId);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private void ProcessSetup(string[] parts, out Guid roundId)
        {
            //"Setup-Alice:100-Bob:100-Chris:100-David:1000-Ellen:1000-Frank:1000-5/10/4/5",
            var request = new StartNewRoundRequest();
            request.Players = new List<Player>();
            for (int i = 1; i <= 6; i++)
            {
                var segments = parts[i].Split(':');
                request.Players.Add(new Player() {Name = segments[0], StackSize = int.Parse(segments[1])});
            }

            var numbers = parts[7].Split('/');
            request.SmallBlindSize = int.Parse(numbers[0]);
            request.BigBlindSize = int.Parse(numbers[1]);
            request.HeroIndex = int.Parse(numbers[2]);
            request.ButtonIndex = int.Parse(numbers[3]);

            if (parts.Length == 9)
            {
                var segments = parts[8].Split(':');
                foreach (var segment in segments)
                {
                    request.Players[int.Parse(segment)].SittingOut = true;
                }
            }

            roundId = _rm.StartNewRound(request).RoundId;
        }

        private void ProcessHoles(string[] parts, Guid roundId)
        {
            var hole1 = ParseCard(parts[1]);
            var hole2 = ParseCard(parts[2]);
            var request = new NotifyHeroHolesRequest()
            {
                Holes = new List<Card>() { hole1, hole2},
                RoundId = roundId
            };

            _rm.NotifyHeroHoles(request);
        }

        private Card ParseCard(string part)
        {
            // "Holes-HK-HJ",
            SuitEnum suit;
            switch (part[0])
            {
                case 'H':
                    suit = SuitEnum.Heart;
                    break;
                case 'S':
                    suit = SuitEnum.Spade;
                    break;
                case 'D':
                    suit = SuitEnum.Diamond;
                    break;
                case 'C':
                    suit = SuitEnum.Club;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            RankEnum rank;
            switch (part[1])
            {
                case 'A':
                    rank = RankEnum.Ace;
                    break;
                case '2':
                    rank = RankEnum.Two;
                    break;
                case '3':
                    rank = RankEnum.Three;
                    break;
                case '4':
                    rank = RankEnum.Four;
                    break;
                case '5':
                    rank = RankEnum.Five;
                    break;
                case '6':
                    rank = RankEnum.Six;
                    break;
                case '7':
                    rank = RankEnum.Seven;
                    break;
                case '8':
                    rank = RankEnum.Eight;
                    break;
                case '9':
                    rank = RankEnum.Nine;
                    break;
                case 'T':
                    rank= RankEnum.Ten;
                    break;
                case 'J':
                    rank = RankEnum.Jack;
                    break;
                case 'Q':
                    rank = RankEnum.Queen;
                    break;
                case 'K':
                    rank = RankEnum.King;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return new Card(suit, rank);
        }

        private void ProcessDecision(string[] parts, Guid roundId)
        {

            //"Decision-Chris-Call",
              //  "Decision-David-Reraise-20",
            var name = parts[1];
            DecisionType decisionType = DecisionType.Undefined;
            int chips = 0;
            switch (parts[2])
            {
                case "Check":
                    decisionType = DecisionType.Check;
                    break;
                case "Call":
                    decisionType = DecisionType.Call;
                    break;
                case "Raise":
                    decisionType = DecisionType.Raise;
                    chips = int.Parse(parts[3]);
                    break;
                case "Reraise":
                    decisionType = DecisionType.Reraise;
                    chips = int.Parse(parts[3]);
                    break;
                case "Fold":
                    decisionType = DecisionType.Fold;
                    break;
                case "AllIn":
                    decisionType = DecisionType.AllIn;
                    chips = int.Parse(parts[3]);
                    break;
                case "AllInRaise":
                    decisionType = DecisionType.AllInRaise;
                    chips = int.Parse(parts[3]);
                    break;
            }

            var request = new NotifyDecisionRequest()
            {
                Decision = new Decision(decisionType, chips),
                PlayerName = name,
                RoundId =  roundId
            };

            _rm.NotifyDecision(request);
        }

        private void ProcessGetDecision(string[] parts, Guid roundId)
        {
            var response = _rm.GetDecision(roundId);
            Assert.AreEqual(parts[1], response.Decision.DecisionType.ToString());
        }

        private void ProcessFlops(string[] parts, Guid roundId)
        {
            //"Flops-DK-D2-D5",
            var flop1 = ParseCard(parts[1]);
            var flop2 = ParseCard(parts[2]);
            var flop3 = ParseCard(parts[3]);

            var request = new NotifyFlopsRequest()
            {
                Flops = new List<Card>() { flop1, flop2, flop3},
                RoundId = roundId
            };

            _rm.NotifyFlops(request);
        }

        private void ProcessTurn(string[] parts, Guid roundId)
        {
            var turn = ParseCard(parts[1]);
            var request = new NotifyTurnRequest()
            {
                RoundId = roundId,
                Turn =  turn
            };

            _rm.NotifyTurn(request);
        }

        private void ProcessRiver(string[] parts, Guid roundId)
        {
            var river = ParseCard(parts[1]);
            var request = new NotifyRiverRequest()
            {
                RoundId = roundId,
                River = river
            };

            _rm.NotifyRiver(request);
        }

        private StartNewRoundRequest GenerateNewRoundRequest(int heroIndex, int buttonIndex)
        {
            return new StartNewRoundRequest()
            {
                Players = new List<Player>()
                {
                    new Player() { Name = "Alice", StackSize = 1000 },
                    new Player() { Name = "Bob", StackSize = 1000 },
                    new Player() { Name = "Chris", StackSize = 1000 },
                    new Player() { Name = "David", StackSize = 1000 },
                    new Player() { Name = "Ellen", StackSize = 1000 },
                    new Player() { Name = "Frank", StackSize = 1000 },
                },
                HeroIndex = heroIndex,
                ButtonIndex = buttonIndex,
                SmallBlindSize = 5,
                BigBlindSize = 10
            };
        }

        private NotifyHeroHolesRequest GenerateHeroHolesRequest(Guid roundId, SuitEnum suit1, RankEnum rank1,
            SuitEnum suit2, RankEnum rank2)
        {
            return new NotifyHeroHolesRequest()
            {
                RoundId = roundId,
                Holes = new List<Card> { new Card(suit1, rank1), new Card(suit2, rank2) }
            };
        }

        private NotifyFlopsRequest GenerateFlopsRequest(Guid roundId,
            SuitEnum suit1, RankEnum rank1,
            SuitEnum suit2, RankEnum rank2,
            SuitEnum suit3, RankEnum rank3)
        {
            return new NotifyFlopsRequest()
            {
                RoundId = roundId,
                Flops = new List<Card>()
                {
                    new Card(suit1, rank1),
                    new Card(suit2, rank2),
                    new Card(suit3, rank3),
                }
            };
        }

        private NotifyDecisionRequest GenerateNotifyDecisionRequest(Guid roundId,
            string playerName, DecisionType decisionType, int chips)
        {
            return new NotifyDecisionRequest()
            {
                RoundId = roundId,
                PlayerName = playerName,
                Decision = new Decision(decisionType, chips)
            };
        }
    }
}
