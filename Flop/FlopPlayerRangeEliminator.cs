using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;

namespace Flop
{
    public class FlopPlayerRangeEliminator
    {
        /// <summary>
        /// 根據Board和Move是否match將一些Grid從PlayerRange中去掉
        /// </summary>
        /// <param name="preflopRange"></param>
        /// <param name="tester"></param>
        /// <param name="playerProfile"></param>
        public void EliminateGrids(PlayerRange preflopRange, Func<RangeGrid, BoardRangeGridStatusEnum> tester, PlayerRoundProfile playerProfile)
        {
            foreach (var playerRangeGrid in preflopRange.GetAliveGrids())
            {
                var outcome = tester(playerRangeGrid.Grid);
                if (!MoveMatchesOutcome(outcome, playerProfile))
                {
                    playerRangeGrid.PlayerRangeGridStatus.RankWiseStatus = PlayerRangeGridStatusEnum.Excluded;
                    //todo support suit
                }
            }
        }

        private bool MoveMatchesOutcome(BoardRangeGridStatusEnum outcome, PlayerRoundProfile playerProfile)
        {
            var flopStatus = playerProfile.FlopPlayerStatus;
            if (flopStatus == PlayerStreetStatusEnum.NotPolledYet)
            {
                return true;
            }

            if (outcome == BoardRangeGridStatusEnum.Nuts)
            {
                return (flopStatus == PlayerStreetStatusEnum.Raise)
                       || (flopStatus == PlayerStreetStatusEnum.Reraise)
                       || (flopStatus == PlayerStreetStatusEnum.AllIn);
            }

            if (outcome == BoardRangeGridStatusEnum.Trash)
            {
                return flopStatus == PlayerStreetStatusEnum.Check || flopStatus == PlayerStreetStatusEnum.Fold;
            }

            if (outcome == BoardRangeGridStatusEnum.Marginal)
            {
                return true;
            }

            switch (outcome)
            {
                case BoardRangeGridStatusEnum.Nuts:
                    return (flopStatus == PlayerStreetStatusEnum.Raise)
                           || (flopStatus == PlayerStreetStatusEnum.Reraise)
                           || (flopStatus == PlayerStreetStatusEnum.AllIn);
                case BoardRangeGridStatusEnum.Marginal:
                    return true;
                case BoardRangeGridStatusEnum.Trash:
                    return flopStatus == PlayerStreetStatusEnum.Check || flopStatus == PlayerStreetStatusEnum.Fold;
                case BoardRangeGridStatusEnum.Elite:
                    if (!playerProfile.InPositionAgainstHero || !playerProfile.IsPreflopRaiser)
                    {
                        return true;
                    }
                    return (flopStatus == PlayerStreetStatusEnum.Call)
                        || (flopStatus == PlayerStreetStatusEnum.Raise)
                        || (flopStatus == PlayerStreetStatusEnum.Reraise)
                        || (flopStatus == PlayerStreetStatusEnum.AllIn);
                default:
                    return true;
            }
        }
    }
}
