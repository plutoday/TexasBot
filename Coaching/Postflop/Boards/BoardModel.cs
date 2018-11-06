using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coaching.Postflop.Boards.BoardSpectrums;
using Coaching.Postflop.Ranging;
using Models.Ranging;

namespace Coaching.Postflop.Boards
{
    public class BoardModel
    {
        public Board Board { get; set; }
        public BoardStageEnum BoardStage { get; set; }
        public bool Dynamic { get; set; }
        public bool Wet { get; set; }
        public List<RangeGrid> EliteRangeGrids { get; set; }
        public List<RangeGrid> EliteWhenSuitedRangeGrids { get; set; }
        public List<RangeGrid> MarginalRangeGrids { get; set; }
        public List<RangeGrid> TrashRangeGrids { get; set; }

        public BoardModel(BoardStatus boardStatus)
        {
            
        }

        public GridGradeOnBoardEnum GetGridGrade(RangeGrid rangeGrid)
        {
            if (EliteRangeGrids.Contains(rangeGrid))
            {
                return GridGradeOnBoardEnum.Elite;
            }

            if (EliteWhenSuitedRangeGrids.Contains(rangeGrid))
            {
                return GridGradeOnBoardEnum.EliteWhenSuited;
            }

            if (TrashRangeGrids.Contains(rangeGrid))
            {
                return GridGradeOnBoardEnum.Trash;
            }

            return GridGradeOnBoardEnum.Marginal;
        }
    }

    public enum GridGradeOnBoardEnum
    {
        Elite,  //should raise it
        EliteWhenSuited,    //should raise it when suited
        Marginal,   //marginal
        Trash,  //check fold
    }
}
