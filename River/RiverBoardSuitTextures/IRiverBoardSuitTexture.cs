using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Ranging;

namespace River.RiverBoardSuitTextures
{
    interface IRiverBoardSuitTexture
    {
        Dictionary<Tuple<SuitEnum, SuitEnum>, bool> ShouldAGridFoldToBet(RangeGrid grid);
    }
}
