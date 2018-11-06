namespace Models
{
    /// <summary>
    /// Starting Hand, Holes used in Post flop
    /// </summary>
    public class HoldingHoles
    {
        public Card Hole1 { get; set; }
        public Card Hole2 { get; set; }

        public HoldingHoles(Card hole1, Card hole2)
        {
            if (hole1.CompareTo(hole2) > 0)
            {
                Hole1 = hole1;
                Hole2 = hole2;
            }
            else
            {
                Hole1 = hole2;
                Hole2 = hole1;
            }
        }
    }
}
