using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coaching.Postflop.Boards;
using Coaching.Postflop.Ranging;
using Models;
using Models.Ranging;

namespace Coaching.Postflop.EquityCalculation
{
    public interface IEquityCalculator
    {
        double CalculateEquityAgainstARange(HoldingHoles heroHoles, PlayerRange villainPlayerRange, BoardStatus boardStatus);
    }
}
