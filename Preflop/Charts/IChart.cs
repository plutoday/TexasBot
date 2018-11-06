using Models.Ranging;

namespace Preflop.Charts
{
    public interface IChart<out T>
    {
        T Get(RangeGrid grid);
    }
}
