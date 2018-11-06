using Models;
using System;
using System.Linq;
using Infra;

namespace Terminal
{
    public class RoundDriver
    {
        private readonly Input _input;

        public void Drive(Round round, RoundSetup roundSetup)
        {
            Player winner = null;

            round.StageEnum = StageEnum.Preflop;
            roundSetup.Hole1 = _input.GetHole1();
            roundSetup.Hole2 = _input.GetHole2();
            winner = PollPlayers(round, roundSetup);
            if (winner != null)
            {
                Logger.Instance.Log($"{round.RoundId}|Game ended on preflop, and the winner is {winner.Position}-{winner.Name}, potSize={round.CurrentPotSize}");
                return;
            }

            round.StageEnum = StageEnum.Flop;

            round.Flop1 = _input.GetFlop1();
            round.Flop2 = _input.GetFlop2();
            round.Flop3 = _input.GetFlop3();
            winner = PollPlayers(round, roundSetup);
            if (winner != null)
            {
                Logger.Instance.Log($"{round.RoundId}|Game ended on flop, and the winner is {winner.Position}-{winner.Name}, potSize={round.CurrentPotSize}");
                return;
            }

            round.StageEnum = StageEnum.Turn;
            round.Turn = _input.GetTurn();
            winner = PollPlayers(round, roundSetup);
            if (winner != null)
            {
                Logger.Instance.Log($"{round.RoundId}|Game ended on turn, and the winner is {winner.Position}-{winner.Name}, potSize={round.CurrentPotSize}");
                return;
            }

            round.StageEnum = StageEnum.River;
            round.River = _input.GetRiver();
            winner = PollPlayers(round, roundSetup);
            if (winner != null)
            {
                Logger.Instance.Log($"{round.RoundId}|Game ended on river, and the winner is {winner.Position}-{winner.Name}, potSize={round.CurrentPotSize}");
                return;
            }

            //todo Showdown, implement the comparison of hands
        }

        private Player PollPlayers(Round round, RoundSetup roundSetup)
        {
            Logger.Instance.Log($"{round.RoundId}|Polling players for {round.StageEnum}");
            foreach (var player in round.Players.Where(p => p.IsAlive))
            {
                Logger.Instance.Log($"{round.RoundId}|{player.Position}:{player.Name} still alive.");
                player.ResetPolled();
            }

            round.Index = 0;
            round.IsRaised = false;

            if (round.StageEnum == StageEnum.Preflop)
            {
                round.Index = 2;
                round.RecordMove(new Move(round.Players[0], new Decision(DecisionType.Ante, round.SmallBlindSize), round.StageEnum));
                round.RecordMove(new Move(round.Players[1], new Decision(DecisionType.Ante, round.BigBlindSize), round.StageEnum));
                round.IsRaised = true;
            }

            while (true)
            {
                var player = round.GetNextPlayer();

                var candidateDecisionTypes = Utils.GetCandidateDecisionTypes(round.IsRaised);
                int chipsToCall = round.MostChipsBetByRound[round.StageEnum] - player.ChipsBetByStage[round.StageEnum];
                var decision = player.Index == roundSetup.HeroIndex ? _input.GetMyDecision(round, candidateDecisionTypes, roundSetup)
                    : _input.GetDecision(player, round.CurrentRaiser, candidateDecisionTypes, chipsToCall);

                if (!candidateDecisionTypes.Contains(decision.DecisionType))
                {
                    throw new InvalidOperationException($"{decision.DecisionType} is out of the candidates {string.Join("/", candidateDecisionTypes)}");
                }

                var move = new Move(player, decision, round.StageEnum);
                round.RecordMove(move);

                if (round.IsSettled)
                {
                    break;
                }
            }

            if (round.Players.Count(p => p.IsAlive) == 1)
            {
                //Winner
                return round.Players.First(p => p.IsAlive);
            }

            //Still more than one alive players, game continues
            return null;
        }
    }
}
