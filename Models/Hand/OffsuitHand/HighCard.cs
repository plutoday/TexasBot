namespace Models.Hand.OffsuitHand
{
    public class HighCard : OffsuitHandBase
    {
        public RankEnum Card1 { get; set; }
        public RankEnum Card2 { get; set; }
        public RankEnum Card3 { get; set; }
        public RankEnum Card4 { get; set; }
        public RankEnum Card5 { get; set; }

        public HighCard(RankEnum card1, RankEnum card2, RankEnum card3, RankEnum card4, RankEnum card5)
        {
            Card1 = card1;
            Card2 = card2;
            Card3 = card3;
            Card4 = card4;
            Card5 = card5;
        }

        public override int HandRank => 1;
        public override int CompareToOffsuitHand(OffsuitHandBase other)
        {
            var otherHighCard = other as HighCard;
            if (otherHighCard == null)
            {
                return HandRank.CompareTo(other.HandRank);
            }

            if (Card1 != otherHighCard.Card1)
            {
                return Card1.CompareTo(otherHighCard.Card1);
            }
            if (Card2 != otherHighCard.Card2)
            {
                return Card2.CompareTo(otherHighCard.Card2);
            }
            if (Card3 != otherHighCard.Card3)
            {
                return Card3.CompareTo(otherHighCard.Card3);
            }
            if (Card4 != otherHighCard.Card4)
            {
                return Card4.CompareTo(otherHighCard.Card4);
            }
            return Card5.CompareTo(otherHighCard.Card5);
        }
    }
}
