using System;

namespace Common
{
    public class PkResult
    {
        public int VillainFoldCount { get; set; }
        public int HeroWinScenariosCount { get; set; }
        public int VillainWinScenariosCount { get; set; }
        public int TiedScenariosCount { get; set; }

        public PkResult(int heroWins, int villainWins, int ties, int villainFolds)
        {
            HeroWinScenariosCount = heroWins;
            VillainWinScenariosCount = villainWins;
            TiedScenariosCount = ties;
            VillainFoldCount = villainFolds;
        }

        public PkResult():this(0,0,0,0)
        { }

        public void Add(PkResult other)
        {
            if (other.HeroWinScenariosCount < 0 || other.TiedScenariosCount < 0 || other.VillainFoldCount < 0
                || other.VillainWinScenariosCount < 0)
            {
                throw new InvalidOperationException();
            }
            HeroWinScenariosCount += other.HeroWinScenariosCount;
            VillainWinScenariosCount += other.VillainWinScenariosCount;
            TiedScenariosCount += other.TiedScenariosCount;
            VillainFoldCount += other.VillainFoldCount;
        }

        public override string ToString()
        {
            return $"{HeroWinScenariosCount}|{TiedScenariosCount}|{VillainWinScenariosCount}|{VillainFoldCount}";
        }
    }
}
