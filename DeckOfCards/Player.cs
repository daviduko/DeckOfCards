using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards
{
    internal class Player
    {
        public string Name {  get; set; }
        public Deck Deck { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        public Card Draw()
        {
            return Deck.DrawCard();
        }

        public void AddCards(List<Card> cards)
        {
            Deck.AddCards(cards);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
