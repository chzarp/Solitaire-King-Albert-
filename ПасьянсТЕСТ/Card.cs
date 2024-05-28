namespace ПасьянсТЕСТ
{
    public class Card
    {
        public enum Suit { Hearts, Diamonds, Clubs, Spades }
        public enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

        public Suit CardSuit { get; private set; }
        public Rank CardRank { get; private set; }
        public bool IsFaceUp { get; set; }

        public Card(Suit suit, Rank rank)
        {
            CardSuit = suit;
            CardRank = rank;
            IsFaceUp = false;
        }

        public override string ToString()
        {
            return $"{CardRank} of {CardSuit}";
        }
    }
}
