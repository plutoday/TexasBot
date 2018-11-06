namespace Models
{
    public class Move
    {
        public Player Player { get; set; }

        public Decision Decision { get; set; }

        public StageEnum Stage { get; set; }

        public Move(Player player, Decision decision, StageEnum stage)
        {
            Player = player;
            Decision = decision;
            Stage = stage;
        }
    }
}
