using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Ranging;

namespace Preflop.Charts
{
    public class Chart<T> : IChart<T>
    {
        private readonly Dictionary<string, T> _dict;

        public Chart()
        {
            _dict = new Dictionary<string, T>();
        }

        public void Add(RangeGrid grid, T value)
        {
            _dict.Add(grid.ToString(), value);
        }

        public T Get(RangeGrid grid)
        {
            if (_dict.ContainsKey(grid.ToString())) return _dict[grid.ToString()];
            return default(T);
        }
    }
}
