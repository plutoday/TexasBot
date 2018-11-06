using System;
using System.Collections.Generic;
using Coaching.Postflop.Boards;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Ranging
{
    public class RangeEstimator
    {
        private InitialRangeGenerator _initialRangeGenerator = new InitialRangeGenerator();

        public PlayerRange EstimateRange(PostflopStatusSummary statusSummary, PostflopPlayerSummary villainSummary)
        {
            //todo: estimate the current Range based on
            //position
            //betting history
            //board
            //hero cards
            switch (statusSummary.BoardStatus.BoardStage)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        private PlayerRange EstimateFlopRange(PostflopStatusSummary statusSummary, PostflopPlayerSummary villainSummary)
        {
            var preflopRange = EstimateRange(statusSummary.BigBlindSize, villainSummary.PreflopDecisions, villainSummary.Position);
            var boardModel = new BoardModel(statusSummary.BoardStatus);

            foreach (var grid in preflopRange.GetAliveGrids())
            {
                var gridGrade = boardModel.GetGridGrade(grid.Grid);
                MarkGrid(villainSummary, GetPlayerStatusInRound(), gridGrade, grid.Grid);
            }

            return preflopRange;
        }

        private void MarkGrid(PostflopPlayerSummary villainSummary, StatusInRoundEnum villainStatus, GridGradeOnBoardEnum gridGrade, RangeGrid grid)
        {
            /*
            if (villainStatus == StatusInRoundEnum.Raised)
            {
                if (gridGrade == GridGradeOnBoardEnum.EliteWhenSuited)
                {
                    grid.InRange = PlayerRangeGridStatusEnum.IncludedIfSuitedWithBoard;
                }
                if (gridGrade == GridGradeOnBoardEnum.Trash)
                {
                    grid.InRange = PlayerRangeGridStatusEnum.Excluded;
                }
            }

            if (villainStatus == StatusInRoundEnum.Checked)
            {
                if (villainSummary.InRoundRole != InRoundRole.OopCaller)
                {
                    if (gridGrade == GridGradeOnBoardEnum.Elite)
                    {
                        grid.InRange = PlayerRangeGridStatusEnum.Excluded;
                    }
                    if (gridGrade == GridGradeOnBoardEnum.EliteWhenSuited)
                    {
                        grid.InRange = PlayerRangeGridStatusEnum.IncludedIfOffSuitWithBoard;
                    }
                }
            }
            */
        }

        public PlayerRange EstimateRange(int bigBlindSize, List<Decision> preflopDecisions, PositionEnum position)
        {
            var preflopPlayerStatus = Utils.DeterminesPreflopPlayerStatus(preflopDecisions,
                bigBlindSize);
            return _initialRangeGenerator.GeneratePreflopRange(position, preflopPlayerStatus);
        }

        private StatusInRoundEnum GetPlayerStatusInRound()
        {
            throw new NotImplementedException();
        }
    }

    public enum StatusInRoundEnum
    {
        NotPolled,
        Checked,
        Raised,
        Called,
    }
}
