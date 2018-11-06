using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// Used for Round 
    /// </summary>
    public class Player
    {
        public string Name { get; set; }

        public int StackSize { get; set; }

        public int ChipsBetAlready => ChipsBetByStage.Sum(e => e.Value);

        public int ChipsBetPostFlop => ChipsBetByStage.Where(e => e.Key != StageEnum.Preflop).Sum(e => e.Value);

        public Dictionary<StageEnum, int> ChipsBetByStage { get; set; }

        public PlayerStatusEnum PlayerStatus { get; set; }

        public bool IsAlive => PlayerStatus != PlayerStatusEnum.Folded;

        public int Index { get; set; }

        public PositionEnum Position { get; set; }

        public Player(string name, int stackSize, int index, PositionEnum position)
        {
            Name = name;
            StackSize = stackSize;
            PlayerStatus = PlayerStatusEnum.Unpolled;

            Index = index;
            Position = position;
            ChipsBetByStage = new Dictionary<StageEnum, int>
            {
                {StageEnum.Preflop, 0},
                {StageEnum.Flop, 0},
                {StageEnum.Turn, 0},
                {StageEnum.River, 0}
            };
        }

        public void Fold()
        {
            PlayerStatus = PlayerStatusEnum.Folded;
        }

        public void AllIn()
        {
            PlayerStatus = PlayerStatusEnum.AllIned;
        }

        public void ResetPolled()
        {
            if (PlayerStatus == PlayerStatusEnum.Polled)
            {
                PlayerStatus = PlayerStatusEnum.Unpolled;
            }
        }

        public void SetPolled()
        {
            PlayerStatus = PlayerStatusEnum.Polled;
        }

        public override bool Equals(object obj)
        {
            var otherPlayer = obj as Player;
            if (otherPlayer == null)
            {
                return false;
            }

            return string.Equals(Name, otherPlayer.Name);
        }
    }
}
