using System;
using System.Collections.Generic;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.Boards.BoardSpectrums
{
    public class BoardSpectrum
    {
        public List<BoardSpectrumUnit> BoardSpectrumUnits { get; set; }

        public List<BoardSpectrumGridUnit> BoardSpectrumGridUnits { get; set; } 

        public BoardSpectrum(List<BoardSpectrumUnit> units)
        {
            BoardSpectrumUnits = new List<BoardSpectrumUnit>(units);
        }

        public BoardSpectrum(List<BoardSpectrumGridUnit> units)
        {
            BoardSpectrumGridUnits = new List<BoardSpectrumGridUnit>(units);
        }

        public double GetEquity(HoldingHoles holes)
        {
            throw new NotImplementedException();
        }

        public BoardSpectrumFightResult Fight(HoldingHoles heroHoles, HoldingHoles villainHoles)
        {
            throw new NotImplementedException();
        }
    }

    public class BoardSpectrumFightResult
    {
        public HoldingHoles HeroHoles { get; set; }
        public HoldingHoles VillainHoles { get; set; }
        public bool HeroWin { get; set; }
        public List<Card> HeroOuts { get; set; } 
        public List<Card> VillainOuts { get; set; } 
    }

    public class BoardSpectrumUnit : IComparable<BoardSpectrumUnit>
    {
        public BoardHandTypeEnum HandType { get; set; }
        /// <summary>
        /// Hands that can make the HandType with the board
        /// </summary>
        public List<HoldingHoles> MakingHands { get; set; }

        public BoardSpectrumUnit(BoardHandTypeEnum handType, List<HoldingHoles> makingHankds)
        {
            HandType = handType;
            MakingHands = new List<HoldingHoles>(makingHankds);
        }


        public int CompareTo(BoardSpectrumUnit other)
        {
            return HandType.CompareTo(other.HandType);
        }
    }

    public class BoardSpectrumGridUnit : IComparable<BoardSpectrumGridUnit>
    {
        public BoardHandTypeEnum HandType { get; set; }

        public List<PlayerRangeGrid> MakingGrids { get; set; }

        public int CompareTo(BoardSpectrumGridUnit other)
        {
            return HandType.CompareTo(other.HandType);
        }

        public BoardSpectrumGridUnit(BoardHandTypeEnum handType, List<PlayerRangeGrid> makingGrids)
        {
            HandType = handType;
            MakingGrids = new List<PlayerRangeGrid>(makingGrids);
        }
    }
}
