
namespace DeckOfCards
{
    enum eSuit
    {
        Clubs,
        Cups,
        Swords,
        Golds
    }
    internal class Card
    {
        int number;
        eSuit suit;

        public int Number {  get { return number; } set { number = value; } }
        public eSuit Suit { get { return suit; } set { suit = value; } }

        public Card(int number, eSuit suit)
        {
            this.number = number;
            this.suit = suit;
        }

        public override string ToString()
        {
            return $"{number} of {suit.ToString()}";
        }
    }
}
