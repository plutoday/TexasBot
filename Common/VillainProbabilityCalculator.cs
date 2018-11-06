using System;
using System.Collections.Generic;
using Models;
using Models.Ranging;

namespace Common
{
    public class VillainProbabilityCalculator
    {
        private readonly Func<RangeGrid, bool> _shouldAGridFoldToBoardByRank;
        private readonly Func<RangeGrid, Dictionary<Tuple<SuitEnum, SuitEnum>, bool>> _shouldAGridFoldToBoardBySuit;
        private readonly Func<IEnumerable<List<Card>>> _enumerate;

        public VillainProbabilityCalculator(Func<RangeGrid, bool> shouldAGridFoldToBoardByRank,
            Func<RangeGrid, Dictionary<Tuple<SuitEnum, SuitEnum>, bool>> shouldAGridFoldToBoardBySuit,
            Func<IEnumerable<List<Card>>> enumerate)
        {
            _shouldAGridFoldToBoardByRank = shouldAGridFoldToBoardByRank;
            _shouldAGridFoldToBoardBySuit = shouldAGridFoldToBoardBySuit;
            _enumerate = enumerate;
        }

        public VillainProbabilityResult Calculate(HoldingHoles heroHoles, PlayerRoundProfile villainProfile, bool considerFolding = true)
        {
            var pkStage = new PkStage(_enumerate, Utils.EnumerateUnfoldedHoles);
            var villainRange = villainProfile.PlayerRange.CloneToPkRange();

            var aliveRanges = villainRange.GetAliveGrids();
            int foldCombos = 0, totalCombos = 0;

            foreach (var playerGrid in aliveRanges)
            {
                if (considerFolding)
                {
                    SetFoldToBoard(playerGrid);
                    foldCombos += playerGrid.FoldCombCount;
                }
                totalCombos += playerGrid.AvailableCombCount;
            }

            double villainFoldP = (double)foldCombos / totalCombos;
            double villainCallP = 1 - villainFoldP;

            var pkResult = pkStage.Pk(heroHoles, villainRange);

            int totalGrids = pkResult.HeroWinScenariosCount + pkResult.TiedScenariosCount +
                             pkResult.VillainWinScenariosCount + pkResult.VillainFoldCount;

            double villainCallWinP = villainCallP * pkResult.VillainWinScenariosCount / totalGrids;
            double villainCallLoseP = villainCallP * pkResult.HeroWinScenariosCount / totalGrids;
            double tieP = villainCallP * pkResult.TiedScenariosCount / totalGrids;

            return new VillainProbabilityResult(new Dictionary<ProbabilityEnum, double>()
            {
                { ProbabilityEnum.Fold, villainFoldP},
                { ProbabilityEnum.CallWin, villainCallWinP},
                { ProbabilityEnum.CallTie, tieP},
                { ProbabilityEnum.CallLose, villainCallLoseP}
            },
            villainProfile.Name);
        }

        /// <summary>
        /// Set Fold if a grid or part of it should fold to board
        /// </summary>
        private void SetFoldToBoard(PlayerRangePkGrid grid)
        {
            if (_shouldAGridFoldToBoardByRank.Invoke(grid.Grid))
            {
                grid.GridPkStatus.RankWiseStatus = PlayerGridPkStatusEnum.Fold;
            }

            SetFoldToBoardBySuit(grid);
        }

        private void SetFoldToBoardBySuit(PlayerRangePkGrid grid)
        {
            var shouldFoldDict = _shouldAGridFoldToBoardBySuit(grid.Grid);

            var hhKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Heart, SuitEnum.Heart);
            var ssKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Spade, SuitEnum.Spade);
            var ddKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Diamond, SuitEnum.Diamond);
            var ccKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Club, SuitEnum.Club);
            var hsKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Heart, SuitEnum.Spade);
            var hdKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Heart, SuitEnum.Diamond);
            var hcKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Heart, SuitEnum.Club);
            var shKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Spade, SuitEnum.Heart);
            var sdKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Spade, SuitEnum.Diamond);
            var scKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Spade, SuitEnum.Club);
            var dhKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Diamond, SuitEnum.Heart);
            var dsKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Diamond, SuitEnum.Spade);
            var dcKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Diamond, SuitEnum.Club);
            var chKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Club, SuitEnum.Heart);
            var csKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Club, SuitEnum.Spade);
            var cdKey = new Tuple<SuitEnum, SuitEnum>(SuitEnum.Club, SuitEnum.Diamond);

            switch (grid.GridPkStatus.Category)
            {
                case GridCategoryEnum.Suited:
                    if (!shouldFoldDict.ContainsKey(hhKey))
                    {
                        grid.GridPkStatus.SuitedStatus.HeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    else if (shouldFoldDict[hhKey])
                    {
                        grid.GridPkStatus.SuitedStatus.HeartStatus = PlayerGridPkStatusEnum.Fold;
                    }

                    if (!shouldFoldDict.ContainsKey(ssKey))
                    {
                        grid.GridPkStatus.SuitedStatus.SpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    else if (shouldFoldDict[ssKey])
                    {
                        grid.GridPkStatus.SuitedStatus.SpadeStatus = PlayerGridPkStatusEnum.Fold;
                    }

                    if (!shouldFoldDict.ContainsKey(ddKey))
                    {
                        grid.GridPkStatus.SuitedStatus.DiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    else if (shouldFoldDict[ddKey])
                    {
                        grid.GridPkStatus.SuitedStatus.DiamondStatus = PlayerGridPkStatusEnum.Fold;
                    }

                    if (!shouldFoldDict.ContainsKey(ccKey))
                    {
                        grid.GridPkStatus.SuitedStatus.ClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    }
                    else if (shouldFoldDict[ccKey])
                    {
                        grid.GridPkStatus.SuitedStatus.ClubStatus = PlayerGridPkStatusEnum.Fold;
                    }
                    break;
                case GridCategoryEnum.Paired:
                    if (!shouldFoldDict.ContainsKey(hsKey)) grid.GridPkStatus.PairedStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hsKey]) grid.GridPkStatus.PairedStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(hdKey)) grid.GridPkStatus.PairedStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hdKey]) grid.GridPkStatus.PairedStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(hcKey)) grid.GridPkStatus.PairedStatus.HeartClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hcKey]) grid.GridPkStatus.PairedStatus.HeartClubStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(sdKey)) grid.GridPkStatus.PairedStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[sdKey]) grid.GridPkStatus.PairedStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(scKey)) grid.GridPkStatus.PairedStatus.SpadeClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[scKey]) grid.GridPkStatus.PairedStatus.SpadeClubStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(dcKey)) grid.GridPkStatus.PairedStatus.DiamondClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[dcKey]) grid.GridPkStatus.PairedStatus.DiamondClubStatus = PlayerGridPkStatusEnum.Fold;

                    break;
                case GridCategoryEnum.Offsuit:
                    if (!shouldFoldDict.ContainsKey(hsKey)) grid.GridPkStatus.OffsuitStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hsKey]) grid.GridPkStatus.OffsuitStatus.HeartSpadeStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(hdKey)) grid.GridPkStatus.OffsuitStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hdKey]) grid.GridPkStatus.OffsuitStatus.HeartDiamondStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(hcKey)) grid.GridPkStatus.OffsuitStatus.HeartClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[hcKey]) grid.GridPkStatus.OffsuitStatus.HeartClubStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(sdKey)) grid.GridPkStatus.OffsuitStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[sdKey]) grid.GridPkStatus.OffsuitStatus.SpadeDiamondStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(scKey)) grid.GridPkStatus.OffsuitStatus.SpadeClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[scKey]) grid.GridPkStatus.OffsuitStatus.SpadeClubStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(dcKey)) grid.GridPkStatus.OffsuitStatus.DiamondClubStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[dcKey]) grid.GridPkStatus.OffsuitStatus.DiamondClubStatus = PlayerGridPkStatusEnum.Fold;


                    if (!shouldFoldDict.ContainsKey(shKey)) grid.GridPkStatus.OffsuitStatus.SpadeHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[shKey]) grid.GridPkStatus.OffsuitStatus.SpadeHeartStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(dhKey)) grid.GridPkStatus.OffsuitStatus.DiamondHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[dhKey]) grid.GridPkStatus.OffsuitStatus.DiamondHeartStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(chKey)) grid.GridPkStatus.OffsuitStatus.ClubHeartStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[chKey]) grid.GridPkStatus.OffsuitStatus.ClubHeartStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(dsKey)) grid.GridPkStatus.OffsuitStatus.DiamondSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[dsKey]) grid.GridPkStatus.OffsuitStatus.DiamondSpadeStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(csKey)) grid.GridPkStatus.OffsuitStatus.ClubSpadeStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[csKey]) grid.GridPkStatus.OffsuitStatus.ClubSpadeStatus = PlayerGridPkStatusEnum.Fold;

                    if (!shouldFoldDict.ContainsKey(cdKey)) grid.GridPkStatus.OffsuitStatus.ClubDiamondStatus = PlayerGridPkStatusEnum.NotAvailable;
                    else if (shouldFoldDict[cdKey]) grid.GridPkStatus.OffsuitStatus.ClubDiamondStatus = PlayerGridPkStatusEnum.Fold;

                    break;
            }
        }
    }
}
