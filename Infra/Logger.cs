using System;
using System.Reflection;
using System.Text;
using log4net;
using Models;
using Models.Ranging;

namespace Infra
{
    public class Logger
    {
        public static Logger Instance = new Logger();

        private Logger()
        {

        }

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string _roundLoggingId;

        public void LogStartNewRound()
        {
            _log.Debug("");
            _roundLoggingId = Guid.NewGuid().ToString().Substring(0, 8);
        }

        public void Log(string logLine)
        {
            _log.Debug($"{_roundLoggingId}|{logLine}");
        }

        public void LogPlayerRange(PlayerRange range, string description = null)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(description))
            {
                sb.AppendLine(description);
            }
            sb.AppendLine($"Range begins==========");
            foreach (var grid in range.Grids)
            {
                sb.AppendLine($"{grid.Grid.ToStringFull()},RankStatus={grid.PlayerRangeGridStatus.RankWiseStatus}");
                switch (grid.Grid.Category)
                {
                    case GridCategoryEnum.Suited:
                        sb.AppendLine($"Suited:{grid.PlayerRangeGridStatus.SuitedStatus.ToString()}");
                        break;
                    case GridCategoryEnum.Offsuit:
                        sb.AppendLine($"Offsuit:{grid.PlayerRangeGridStatus.OffsuitStatus.ToString()}");
                        break;
                    case GridCategoryEnum.Paired:
                        sb.AppendLine($"Paired:{grid.PlayerRangeGridStatus.PairedStatus.ToString()}");
                        break;
                }
            }
            sb.AppendLine("Range ends==========");
            Log(sb.ToString());
        }

        public void LogSqueezing(PlayerRange previousRange, PlayerRange positionRange, PlayerRange intersectedRange,
            Move lastMove)
        {
            Log($"Squeezing range for {lastMove.Player.Name}, stage={lastMove.Stage}," +
                                $" lastMove={lastMove.Decision.DecisionType}," +
                                $" chips={lastMove.Decision.ChipsAdded}.");
            LogPlayerRange(previousRange, $"Before squeezing... lastMove={lastMove.Decision.DecisionType}, stage=Preflop");
            LogPlayerRange(positionRange, $"Based on position={lastMove.Player.Position}, lastMove={lastMove.Decision.DecisionType}");
            LogPlayerRange(intersectedRange, $"Intersected...");
        }
    }
}
