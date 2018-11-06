using System;
using System.Collections.Generic;
using System.Linq;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Boards.BoardSpectrums
{
    public class BoardSpectrumMaker
    {
        private readonly BoardHandDetector _handDetector = new BoardHandDetector();

        public BoardSpectrum MakeSpectrum(BoardStatus boardStatus)
        {
            var allCards = TexasBot.Tools.Utils.GenerateAllCards().Select(c => c.ConvertCard()).ToList();
            allCards.RemoveElementEqualsTo(boardStatus.Flop1); 
            allCards.RemoveElementEqualsTo(boardStatus.Flop2);
            allCards.RemoveElementEqualsTo(boardStatus.Flop3);
            if (boardStatus.Turn != null)
            {
                allCards.RemoveElementEqualsTo(boardStatus.Turn);
            }
            if (boardStatus.River != null)
            {
                allCards.RemoveElementEqualsTo(boardStatus.River);
            }

            var enumeratedTwoCards = TexasBot.Tools.Utils.Enumerate(allCards.ToArray(), 0, 2);

            var holdingHands = new List<HoldingHoles>();
            foreach (var twoCards in enumeratedTwoCards)
            {
                var twoCardsList = twoCards.ToList();
                holdingHands.Add(new HoldingHoles(twoCardsList[0], twoCardsList[1]));
            }

            var spectrumDict = new Dictionary<BoardHandTypeEnum, List<HoldingHoles>>();

            foreach (var holes in holdingHands)
            {
                var boardHand = _handDetector.DetectBoardHand(boardStatus, holes);
                if (!spectrumDict.ContainsKey(boardHand))
                {
                    spectrumDict.Add(boardHand, new List<HoldingHoles>());
                }
                spectrumDict[boardHand].Add(holes);
            }

            var spectrumUnitList = new List<BoardSpectrumUnit>();
            foreach (var entry in spectrumDict)
            {
                spectrumUnitList.Add(new BoardSpectrumUnit(entry.Key, entry.Value));
            }

            spectrumUnitList.Sort((u1, u2) => -u1.CompareTo(u2));

            return new BoardSpectrum(spectrumUnitList);
        }

        public BoardSpectrum MakeBoardSpectrumOfGrids(Board board)
        {
            var boardCards = new List<Card>() {board.Flop1, board.Flop2, board.Flop3};

            if (board.Turn != null)
            {
                boardCards.Add(board.Turn);
            }

            if (board.River != null)
            {
                boardCards.Add(board.River);
            }
            
            Dictionary<BoardHandTypeEnum, List<PlayerRangeGrid> > grids = new Dictionary<BoardHandTypeEnum, List<PlayerRangeGrid>>();

            var range = new PlayerRange();
            foreach (var rangeGrid in range.Grids)
            {
                var handType = GetHandTypeForGrid(boardCards, rangeGrid);
                if (!grids.ContainsKey(handType))
                {
                    grids.Add(handType, new List<PlayerRangeGrid>());
                }
                grids[handType].Add(rangeGrid);
            }

            var list = new List<BoardSpectrumGridUnit>();
            foreach (var entry in grids)
            {
                list.Add(new BoardSpectrumGridUnit(entry.Key, entry.Value));
            }

            list.Sort();

            return new BoardSpectrum(list);
        }

        private BoardHandTypeEnum GetHandTypeForGrid(List<Card> boardCards, PlayerRangeGrid rangeGrid)
        {
            throw new NotImplementedException();
        }

        private BoardHandTypeEnum GetHandTypeFromRanks(List<RankEnum> ranks)
        {
            ranks.Sort();
            Dictionary<RankEnum, int> rankCounts = new Dictionary<RankEnum, int>();
            foreach (var rankEnum in ranks)
            {
                if (!rankCounts.ContainsKey(rankEnum))
                {
                    rankCounts.Add(rankEnum, 0);
                }

                rankCounts[rankEnum]++;
            }

            List<Tuple<RankEnum, int>> rankCountsList = rankCounts.Select(rankCount => new Tuple<RankEnum, int>(rankCount.Key, rankCount.Value)).ToList();

            rankCountsList.Sort((r1, r2) => r1.Item2 != r2.Item2 ? r2.Item2.CompareTo(r1.Item2) : r2.Item1.CompareTo(r1.Item1));

            if (rankCountsList[0].Item2 == 4)
            {
                return BoardHandTypeEnum.FourOfAKind;
            }
            if (rankCountsList[0].Item2 == 3 && rankCountsList[1].Item2 == 2)
            {
                return BoardHandTypeEnum.LowPairFullHouse;
            }

            throw new NotImplementedException();
        }
    }
}
