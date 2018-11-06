namespace TexasBot.Games.Players
{
    public class PlayerDecision
    {
        public string PlayerName { get; set; }
        public Decision Decision { get; set; }

        public PlayerDecision(string playerName, Decision decision)
        {
            PlayerName = playerName;
            Decision = decision;
        }
    }
}
