using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Ranging
{
    public class GridStatusHittingTurn : SuitTieredGridStatus<GridHitNewRoundResultEnum>
    {
        public GridStatusHittingTurn(GridCategoryEnum category) : base(category)
        {
        }
    }
}
