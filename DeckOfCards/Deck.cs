using System;
using System.Collections.Generic;
using System.Linq;

namespace DeckOfCards
{
    internal class Deck
    {
        private readonly int numberOfCardsPerSuit = 12;
        List<Card> cards;

        public Deck() 
        {
            CreateDeck();
        }

        private void CreateDeck()
        {
            cards = new List<Card>();

            for (int i = 0; i < Enum.GetNames(typeof(eSuit)).Length; i++)
            {
                eSuit suit = (eSuit)i;
                for (int n = 1; n <= numberOfCardsPerSuit; n++)
                {
                    cards.Add(new Card(n, suit));
                }
            }
        }

        public int GetNumberOfCards()
        {
            return cards.Count;
        }

        public void Shuffle()
        {
            Random rnd = new Random();
            int n = cards.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int j = rnd.Next(i, n);

                if(j != i)
                {
                    Card temp = cards[i];
                    cards[i] = cards[j];
                    cards[j] = temp;
                }
            }
        }

        public Card Draw()
        {
            return RemoveCard(0);
        }

        public Card RandomDraw()
        {
            Random rnd = new Random();
            return RemoveCard(rnd.Next(0, cards.Count));
        }

        public Card DrawAt(int position)
        {
            return RemoveCard(position);
        }

        private Card RemoveCard(int position)
        {
            Card card = cards.ElementAt(position);
            cards.RemoveAt(position);
            return card;
        }

        public override string ToString()
        {
            string deck = string.Empty;

            foreach (Card card in cards) 
                deck += $"{card.ToString()}\n";

            return deck;
        }
    }
}
