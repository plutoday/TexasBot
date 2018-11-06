using Models;
using ServerLogic.Contracts;
using Strategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerLogic
{
    public class RoundManager
    {
        public static RoundManager Instance = new RoundManager();

        public Dictionary<Guid, Round> Rounds { get; set; }
        public Dictionary<Guid, RoundSetup> RoundSetups { get; set; }
        public Dictionary<Guid, Brain> Brains { get; set; }

        private RoundManager()
        {
            Rounds = new Dictionary<Guid, Round>();
            RoundSetups = new Dictionary<Guid, RoundSetup>();
            Brains = new Dictionary<Guid, Brain>();
        }
        public RoundIdResponse StartNewRound(StartNewRoundRequest newRoundRequest)
        {
            string heroName = newRoundRequest.Players[newRoundRequest.HeroIndex].Name;
            string buttonName = newRoundRequest.Players[newRoundRequest.ButtonIndex].Name;
            
            List<Contracts.Player> sittingInPlayers = newRoundRequest.Players.Where(p => !p.SittingOut).ToList();
            int heroIndex = sittingInPlayers.FindIndex(p => string.Equals(p.Name, heroName));
            int buttonIndex = sittingInPlayers.FindIndex(p => string.Equals(p.Name, buttonName));

            var round = new Round(new RoundInput(sittingInPlayers.Count, buttonIndex,
                sittingInPlayers.Select(p => p.Name).ToList(),
                sittingInPlayers.Select(p => (int)p.StackSize).ToList(),
                newRoundRequest.SmallBlindSize, newRoundRequest.BigBlindSize));

            Rounds.Add(round.RoundId, round);
            RoundSetups.Add(round.RoundId, new RoundSetup() {HeroIndex = heroIndex});
            Brains.Add(round.RoundId, new Brain());
            round.RecordMove(new Move(round.GetCurrentPlayer(), new Decision(DecisionType.Ante, round.SmallBlindSize), StageEnum.Preflop));
            round.MoveToNextPlayer();
            round.RecordMove(new Move(round.GetCurrentPlayer(), new Decision(DecisionType.Ante, round.BigBlindSize), StageEnum.Preflop));
            round.MoveToNextPlayer();

            return new RoundIdResponse()
            {
                RoundId = round.RoundId,
                Action = new ExpectedAction
                {
                    Action = ExpectedActionEnum.HeroHoles
                }
            };
        }

        public DummyResponse NotifyHeroHoles(NotifyHeroHolesRequest request)
        {
            if (!Rounds.ContainsKey(request.RoundId))
            {
                throw new InvalidOperationException($"Round with {request.RoundId} not found!");
            }

            var round = Rounds[request.RoundId];
            round.StageEnum = StageEnum.Preflop;
            var roundSetup = RoundSetups[request.RoundId];
            roundSetup.Hole1 = request.Holes[0];
            roundSetup.Hole2 = request.Holes[1];

            return new DummyResponse()
            {
                Action = new ExpectedAction
                {
                    Action = ExpectedActionEnum.Decision,
                    PlayerName = round.GetCurrentPlayer().Name
                }
            };
        }

        public DummyResponse NotifyFlops(NotifyFlopsRequest request)
        {
            if (!Rounds.ContainsKey(request.RoundId))
            {
                throw new InvalidOperationException($"Round with {request.RoundId} not found!");
            }

            var round = Rounds[request.RoundId];
            round.Flop1 = request.Flops[0];
            round.Flop2 = request.Flops[1];
            round.Flop3 = request.Flops[2];
            round.MoveToNextStage();

            return new DummyResponse()
            {
                Action = new ExpectedAction
                {
                    Action = ExpectedActionEnum.Decision,
                    PlayerName = round.GetCurrentPlayer().Name
                }
            };
        }

        public DummyResponse NotifyTurn(NotifyTurnRequest request)
        {
            if (!Rounds.ContainsKey(request.RoundId))
            {
                throw new InvalidOperationException($"Round with {request.RoundId} not found!");
            }

            var round = Rounds[request.RoundId];
            round.Turn = request.Turn;
            round.MoveToNextStage();

            return new DummyResponse()
            {
                Action = new ExpectedAction
                {
                    Action = ExpectedActionEnum.Decision,
                    PlayerName = round.GetCurrentPlayer().Name
                }
            };
        }

        public DummyResponse NotifyRiver(NotifyRiverRequest request)
        {
            if (!Rounds.ContainsKey(request.RoundId))
            {
                throw new InvalidOperationException($"Round with {request.RoundId} not found!");
            }

            var round = Rounds[request.RoundId];
            round.River = request.River;
            round.MoveToNextStage();

            return new DummyResponse()
            {
                Action = new ExpectedAction
                {
                    Action = ExpectedActionEnum.Decision,
                    PlayerName = round.GetCurrentPlayer().Name
                }
            };
        }

        public DummyResponse NotifyDecision(NotifyDecisionRequest request)
        {
            if (!Rounds.ContainsKey(request.RoundId))
            {
                throw new InvalidOperationException($"Round with {request.RoundId} not found!");
            }

            var round = Rounds[request.RoundId];
            var player = round.GetCurrentPlayer();
            if (!string.Equals(player.Name, request.PlayerName))
            {
                throw new InvalidOperationException($"Turn for {player.Name} but received decision from {request.PlayerName}.");
            }

            round.RecordMove(new Move(player, request.Decision, round.StageEnum));

            round.MoveToNextPlayer();

            return new DummyResponse()
            {
                Action = GetExpectedAction(round)
            };
        }

        public DecisionResponse GetDecision(Guid roundId)
        {
            if (!Rounds.ContainsKey(roundId))
            {
                throw new InvalidOperationException($"Round with {roundId} not found!");
            }

            var round = Rounds[roundId];

            var player = round.GetCurrentPlayer();
            if (player.Index != RoundSetups[roundId].HeroIndex)
            {
                throw new InvalidOperationException($"Current player {player.Name} is not Hero: {round.Players[RoundSetups[roundId].HeroIndex]}");
            }

            if (!Brains.ContainsKey(roundId))
            {
                throw new InvalidOperationException($"Brain with {roundId} not found!");
            }

            var brain = Brains[roundId];

            var decision = brain.GetDecision(round, RoundSetups[roundId]);

            round.RecordMove(new Move(player, decision, round.StageEnum));
            round.MoveToNextPlayer();

            return new DecisionResponse
            {
                Decision = decision,
                Action = GetExpectedAction(round)
            };
        }

        private ExpectedAction GetExpectedAction(Round round)
        {
            ExpectedActionEnum actionEnum = ExpectedActionEnum.Decision;
            if (round.IsSettled)
            {
                switch (round.StageEnum)
                {
                    case StageEnum.Preflop:
                        actionEnum = ExpectedActionEnum.Flops;
                        break;
                    case StageEnum.Flop:
                        actionEnum = ExpectedActionEnum.Turn;
                        break;
                    case StageEnum.Turn:
                        actionEnum = ExpectedActionEnum.River;
                        break;
                    case StageEnum.River:
                        actionEnum = ExpectedActionEnum.VillainHoles;
                        break;
                    default:
                        throw new InvalidOperationException($"{round.StageEnum} should not be here");
                }
            }

            var action = new ExpectedAction
            {
                Action = actionEnum
            };

            if (action.Action == ExpectedActionEnum.Decision || action.Action == ExpectedActionEnum.VillainHoles)
            {
                action.PlayerName = round.GetCurrentPlayer().Name;
            }

            return action;
        }

        private Round GetRound(Guid roundId)
        {
            if (!Rounds.ContainsKey(roundId))
            {
                throw new InvalidOperationException($"Round with {roundId} not found!");
            }

            return Rounds[roundId];
        }

        private Brain GetBrain(Guid roundId)
        {
            if (!Brains.ContainsKey(roundId))
            {
                throw new InvalidOperationException($"Brain with {roundId} not found!");
            }
            return Brains[roundId];
        }


    }
}
