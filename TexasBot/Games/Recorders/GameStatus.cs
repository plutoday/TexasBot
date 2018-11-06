using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasBot.Games.Players;
using TexasBot.Models;

namespace TexasBot.Games.Recorders
{
    public class GameStatus
    {
        public List<PlayerDecision> HolesDecisions { get; set; }
        public List<PlayerDecision> FlopsDecisions { get; set; }
        public List<PlayerDecision> TurnDecisions { get; set; }
        public List<PlayerDecision> RiverDecisions { get; set; }

        public GameStage GameStage { get; set; }

        public List<PlayerRecord> PlayerRecords { get; set; }

        public int PotSize { get; set; }

        public void RecordDecision(PlayerDecision playerDecision)
        {
            switch (GameStage)
            {
                case GameStage.Initialized:
                    throw new InvalidOperationException("No decision needs to made at this stage");
                case GameStage.HolesReceived:
                    HolesDecisions.Add(playerDecision);
                    break;
                case GameStage.FlopsSeen:
                    FlopsDecisions.Add(playerDecision);
                    break;
                case GameStage.TurnSeen:
                    TurnDecisions.Add(playerDecision);
                    break;
                case GameStage.RiverSeen:
                    RiverDecisions.Add(playerDecision);
                    break;
            }

            var player = FindPlayerRecord(playerDecision.PlayerName);
            player.Polled = true;

            switch (playerDecision.Decision.DecisionEnum)
            {
                case DecisionEnum.Check:
                    break;
                case DecisionEnum.Fold:
                    player.Folded = true;
                    break;
                case DecisionEnum.Call:
                case DecisionEnum.Raise:
                    player.ChipsBet += playerDecision.Decision.Chips;
                    PotSize += playerDecision.Decision.Chips;
                    break;
            }
        }

        public void AddPlayer(string playerName)
        {
            PlayerRecords.Add(new PlayerRecord(playerName));
        }

        public void FinishStage()
        {
            GameStage++;
            foreach (var playerRecord in PlayerRecords.Where(p => !p.Folded))
            {
                playerRecord.Polled = false;
            }
        }

        /// <summary>
        /// Test if all players have agreed on current stage
        /// </summary>
        /// <returns></returns>
        public bool StageSettled()
        {
            if (PlayerRecords.Any(p => !p.Folded && !p.Polled))
            {
                return false;
            }

            var chipsBet = PlayerRecords.First(p => !p.Folded).ChipsBet;
            return PlayerRecords.Where(p => !p.Folded).All(p => p.ChipsBet == chipsBet);
        }

        public PlayerRecord FindPlayerRecord(string playerName)
        {
            return PlayerRecords.First(p => string.Equals(playerName, p.Name));
        }
    }
}
