using System;
using System.Collections.Generic;
using Infra;
using Models;
using Models.Ranging;

namespace Common
{
    public class PlayerRangeSqueezer
    {
        public PlayerRange Squeeze<T>(PlayerRange previousRange, Func<RangeGrid, SuitTieredGridStatus<T>> gridTester, 
            Func<T, Tuple<bool, PlayerRangeGridStatusEnum>> excluder, List<Card> conflictCards)
        {
            var newRange = previousRange.Clone();

            Logger.Instance.Log($"Before squeezing:\r\n{newRange.ToString()}");

            foreach (var playerRangeGrid in newRange.GetAliveGrids())
            {
                playerRangeGrid.Grid.EliminateConflicts(conflictCards);
                SuitTieredGridStatus<T> result = gridTester(playerRangeGrid.Grid);
                Tuple<bool, PlayerRangeGridStatusEnum> excludeResult = excluder.Invoke(result.RankWiseStatus);
                if (excludeResult.Item1)
                {
                    playerRangeGrid.PlayerRangeGridStatus.RankWiseStatus = excludeResult.Item2;
                }
                
                switch (result.Category)
                {
                    case GridCategoryEnum.Suited:
                        excludeResult = excluder.Invoke(result.SuitedStatus.HeartStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.HeartStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.SuitedStatus.SpadeStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.SpadeStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.SuitedStatus.DiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.DiamondStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.SuitedStatus.ClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.SuitedStatus.ClubStatus = excludeResult.Item2;
                        break;
                    case GridCategoryEnum.Paired:
                        excludeResult = excluder.Invoke(result.PairedStatus.HeartSpadeStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartSpadeStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.PairedStatus.HeartDiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartDiamondStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.PairedStatus.HeartClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.HeartClubStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.PairedStatus.SpadeDiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.SpadeDiamondStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.PairedStatus.SpadeClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.SpadeClubStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.PairedStatus.DiamondClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.PairedStatus.DiamondClubStatus = excludeResult.Item2;
                        break;
                    case GridCategoryEnum.Offsuit:
                        excludeResult = excluder.Invoke(result.OffsuitStatus.HeartSpadeStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartSpadeStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.HeartDiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartDiamondStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.HeartClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.HeartClubStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.SpadeDiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeDiamondStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.SpadeClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeClubStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.DiamondClubStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondClubStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.SpadeHeartStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.SpadeHeartStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.DiamondHeartStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondHeartStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.ClubHeartStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubHeartStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.DiamondSpadeStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.DiamondSpadeStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.ClubSpadeStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubSpadeStatus = excludeResult.Item2;
                        excludeResult = excluder.Invoke(result.OffsuitStatus.ClubDiamondStatus);
                        if (excludeResult.Item1) playerRangeGrid.PlayerRangeGridStatus.OffsuitStatus.ClubDiamondStatus = excludeResult.Item2;
                        break;
                }

                Logger.Instance.Log($"grid={playerRangeGrid.Grid}, result={result.ToString()}, status={playerRangeGrid.PlayerRangeGridStatus.ToString()}.");
            }

            Logger.Instance.Log($"After squeezing:\r\n{newRange.ToString()}");

            return newRange;
        }
    }
}
